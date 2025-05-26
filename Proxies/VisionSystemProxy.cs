using Traktor.Interfaces; 
using Traktor.DataModels; 
using Traktor.Core;       

namespace Traktor.Proxies
{
    /// <summary>
    /// Прокси для доступа к <see cref="IComputerVisionSystem"/>.
    /// Обеспечивает ленивую инициализацию, кэширование результатов и логирование.
    /// Может управлять основной и резервной системами технического зрения.
    /// </summary>
    public class VisionSystemProxy : IComputerVisionSystem
    {
        private readonly Func<IComputerVisionSystem> _primarySystemFactory;
        private IComputerVisionSystem _primarySystem;
        private bool _isPrimarySystemInitialized = false;

        // Опциональная резервная система (например, LiDAR, если основная - камера)
        private readonly Func<IComputerVisionSystem> _backupSystemFactory;
        private IComputerVisionSystem _backupSystem;
        private bool _isBackupSystemInitialized = false;
        private bool _useBackupSystem = false; // Флаг для переключения на резервную систему

        // Кэширование для DetectObstacles
        private List<ObstacleData> _cachedObstacles;
        private DateTime _lastObstaclesCacheTime;
        private Coordinates _lastObstaclesQueryPosition; // Кэш должен зависеть от позиции

        // Кэширование для AnalyzeFieldFeatures
        private List<FieldFeatureData> _cachedFeatures;
        private DateTime _lastFeaturesCacheTime;
        private Coordinates _lastFeaturesQueryPosition; // Кэш должен зависеть от позиции

        private readonly TimeSpan _cacheDuration;
        private static readonly object _primaryLock = new object();
        private static readonly object _backupLock = new object();

        private const string SourceFilePath = "Proxies/VisionSystemProxy.cs";
        private const double CACHE_POSITION_TOLERANCE = 0.00001; // Допуск для сравнения координат при кэшировании

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="VisionSystemProxy"/>.
        /// </summary>
        /// <param name="primarySystemFactory">Фабрика для создания основного экземпляра <see cref="IComputerVisionSystem"/>.</param>
        /// <param name="cacheDuration">Продолжительность времени, на которое результаты будут кэшироваться.</param>
        /// <param name="backupSystemFactory">Опциональная фабрика для создания резервного экземпляра <see cref="IComputerVisionSystem"/>.</param>
        public VisionSystemProxy(Func<IComputerVisionSystem> primarySystemFactory, TimeSpan cacheDuration, Func<IComputerVisionSystem> backupSystemFactory = null)
        {
            _primarySystemFactory = primarySystemFactory ?? throw new ArgumentNullException(nameof(primarySystemFactory));
            _cacheDuration = cacheDuration;
            _backupSystemFactory = backupSystemFactory; // Может быть null

            _lastObstaclesCacheTime = DateTime.MinValue;
            _lastFeaturesCacheTime = DateTime.MinValue;

            Logger.Instance.Info(SourceFilePath, $"VisionSystemProxy создан. Кэширование: {_cacheDuration.TotalSeconds}с. Резервная система: {(_backupSystemFactory != null ? "предусмотрена" : "нет")}.");
        }

        /// <summary>
        /// Инициализирует основную систему технического зрения, если она еще не была инициализирована.
        /// </summary>
        private void EnsurePrimarySystemInitialized()
        {
            if (!_isPrimarySystemInitialized)
            {
                lock (_primaryLock)
                {
                    if (!_isPrimarySystemInitialized)
                    {
                        Logger.Instance.Debug(SourceFilePath, "Инициализация основной системы зрения...");
                        _primarySystem = _primarySystemFactory();
                        if (_primarySystem == null)
                        {
                            Logger.Instance.Fatal(SourceFilePath, "Фабрика основной системы зрения вернула null.");
                            throw new InvalidOperationException("Фабрика основной системы зрения вернула null.");
                        }
                        _isPrimarySystemInitialized = true;
                        Logger.Instance.Info(SourceFilePath, $"Основная система зрения ({_primarySystem.GetType().Name}) успешно инициализирована.");
                    }
                }
            }
        }

        /// <summary>
        /// Инициализирует резервную систему технического зрения, если она предусмотрена и еще не была инициализирована.
        /// </summary>
        private void EnsureBackupSystemInitialized()
        {
            if (_backupSystemFactory != null && !_isBackupSystemInitialized)
            {
                lock (_backupLock)
                {
                    if (!_isBackupSystemInitialized)
                    {
                        Logger.Instance.Debug(SourceFilePath, "Инициализация резервной системы зрения...");
                        _backupSystem = _backupSystemFactory();
                        if (_backupSystem == null)
                        {
                            Logger.Instance.Error(SourceFilePath, "ОШИБКА: Фабрика резервной системы зрения вернула null. Резервная система не будет доступна.");
                            _isBackupSystemInitialized = false; 
                        }
                        else
                        {
                            _isBackupSystemInitialized = true;
                            Logger.Instance.Info(SourceFilePath, $"Резервная система зрения ({_backupSystem.GetType().Name}) успешно инициализирована.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Определяет, активна ли в данный момент резервная система.
        /// </summary>
        /// <param name="activateBackupIfAvailable">Если true, попытается активировать резервную систему, если основная не отвечает.</param>
        /// <returns>Экземпляр активной системы зрения или null, если ни одна не доступна.</returns>
        private IComputerVisionSystem GetActiveSystem(bool activateBackupIfAvailable = false)
        {
            if (_useBackupSystem && _backupSystemFactory != null)
            {
                EnsureBackupSystemInitialized();
                if (_isBackupSystemInitialized) return _backupSystem;
                Logger.Instance.Warning(SourceFilePath, "Резервная система была выбрана, но не инициализирована. Попытка использовать основную.");
                _useBackupSystem = false;
            }

            EnsurePrimarySystemInitialized();
            // Здесь можно добавить логику проверки "здоровья" основной системы,
            // и если она не отвечает, и activateBackupIfAvailable=true, то переключиться на резервную.
            return _primarySystem;
        }

        /// <summary>
        /// Принудительно переключает на использование резервной системы, если она доступна.
        /// </summary>
        public void SwitchToBackupSystem()
        {
            if (_backupSystemFactory == null)
            {
                Logger.Instance.Info(SourceFilePath, "SwitchToBackupSystem: Резервная система не предусмотрена.");
                return;
            }
            EnsureBackupSystemInitialized(); 
            if (_isBackupSystemInitialized)
            {
                _useBackupSystem = true;
                Logger.Instance.Info(SourceFilePath, $"SwitchToBackupSystem: Прокси переключен на использование резервной системы зрения ({_backupSystem.GetType().Name}).");
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "SwitchToBackupSystem: Не удалось переключиться на резервную систему (не инициализирована).");
            }
        }

        /// <summary>
        /// Переключает на использование основной системы.
        /// </summary>
        public void SwitchToPrimarySystem()
        {
            if (_useBackupSystem)
            {
                _useBackupSystem = false;
                Logger.Instance.Info(SourceFilePath, "SwitchToPrimarySystem: Прокси переключен на использование основной системы зрения.");
            }
            else { Logger.Instance.Debug(SourceFilePath, "SwitchToPrimarySystem: Уже используется основная система."); }
        }


        /// <inheritdoc/>
        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            IComputerVisionSystem activeSystem = GetActiveSystem(true);
            if (activeSystem == null)
            {
                Logger.Instance.Error(SourceFilePath, "DetectObstacles: Активная система зрения недоступна.");
                return new List<ObstacleData>();
            }
            string systemTypeName = activeSystem.GetType().Name;

            bool positionChangedSignificantly = Math.Abs(currentTractorPosition.Latitude - _lastObstaclesQueryPosition.Latitude) > CACHE_POSITION_TOLERANCE ||
                                                Math.Abs(currentTractorPosition.Longitude - _lastObstaclesQueryPosition.Longitude) > CACHE_POSITION_TOLERANCE;

            if (_cacheDuration > TimeSpan.Zero && !positionChangedSignificantly && _cachedObstacles != null && (DateTime.Now - _lastObstaclesCacheTime) < _cacheDuration)
            {
                Logger.Instance.Debug(SourceFilePath, $"DetectObstacles ({systemTypeName}): Возвращаем данные из кэша для позиции {currentTractorPosition}.");
                return new List<ObstacleData>(_cachedObstacles); // Возвращаем копию кэша
            }

            Logger.Instance.Debug(SourceFilePath, $"DetectObstacles ({systemTypeName}): Кэш не используется или истек/позиция изменилась. Запрос к системе ({systemTypeName}) для позиции {currentTractorPosition}.");
            try
            {
                List<ObstacleData> newData = activeSystem.DetectObstacles(currentTractorPosition);
                _cachedObstacles = newData != null ? new List<ObstacleData>(newData) : new List<ObstacleData>();
                _lastObstaclesCacheTime = DateTime.Now;
                _lastObstaclesQueryPosition = currentTractorPosition;
                Logger.Instance.Debug(SourceFilePath, $"DetectObstacles ({systemTypeName}): Данные получены и кэшированы ({_cachedObstacles.Count} препятствий).");
                return newData;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"DetectObstacles ({systemTypeName}): ОШИБКА при вызове DetectObstacles у реальной системы: {ex.Message}", ex);
                if (activeSystem == _primarySystem && _backupSystemFactory != null)
                {
                    Logger.Instance.Warning(SourceFilePath, "DetectObstacles: Попытка использовать резервную систему после сбоя основной...");
                    SwitchToBackupSystem(); 
                    IComputerVisionSystem backup = GetActiveSystem();
                    if (backup != null && backup == _backupSystem) 
                    {
                        try
                        {
                            Logger.Instance.Info(SourceFilePath, "DetectObstacles: Повторный вызов DetectObstacles у резервной системы.");
                            return backup.DetectObstacles(currentTractorPosition); 
                        }
                        catch (Exception backupEx)
                        {
                            Logger.Instance.Error(SourceFilePath, $"DetectObstacles: ОШИБКА и у резервной системы ({backup.GetType().Name}): {backupEx.Message}", backupEx);
                        }
                    }
                    else if (backup == _primarySystem) 
                    {
                        Logger.Instance.Error(SourceFilePath, "DetectObstacles: Резервная система не смогла активироваться после сбоя основной.");
                    }
                }
                return new List<ObstacleData>();
            }
        }

        /// <inheritdoc/>
        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition)
        {
            IComputerVisionSystem activeSystem = GetActiveSystem(true);
            if (activeSystem == null)
            {
                Logger.Instance.Error(SourceFilePath, "AnalyzeFieldFeatures: Активная система зрения недоступна.");
                return new List<FieldFeatureData>();
            }
            string systemTypeName = activeSystem.GetType().Name;

            bool positionChangedSignificantly = Math.Abs(currentTractorPosition.Latitude - _lastFeaturesQueryPosition.Latitude) > CACHE_POSITION_TOLERANCE ||
                                                Math.Abs(currentTractorPosition.Longitude - _lastFeaturesQueryPosition.Longitude) > CACHE_POSITION_TOLERANCE;

            if (_cacheDuration > TimeSpan.Zero && !positionChangedSignificantly && _cachedFeatures != null && (DateTime.Now - _lastFeaturesCacheTime) < _cacheDuration)
            {
                Logger.Instance.Debug(SourceFilePath, $"AnalyzeFieldFeatures ({systemTypeName}): Возвращаем данные из кэша для позиции {currentTractorPosition}.");
                return new List<FieldFeatureData>(_cachedFeatures);
            }

            Logger.Instance.Debug(SourceFilePath, $"AnalyzeFieldFeatures ({systemTypeName}): Кэш не используется или истек/позиция изменилась. Запрос к системе ({systemTypeName}) для позиции {currentTractorPosition}.");
            try
            {
                List<FieldFeatureData> newData = activeSystem.AnalyzeFieldFeatures(currentTractorPosition);
                _cachedFeatures = newData != null ? new List<FieldFeatureData>(newData) : new List<FieldFeatureData>();
                _lastFeaturesCacheTime = DateTime.Now;
                _lastFeaturesQueryPosition = currentTractorPosition;
                Logger.Instance.Debug(SourceFilePath, $"AnalyzeFieldFeatures ({systemTypeName}): Данные получены и кэшированы ({_cachedFeatures.Count} особенностей).");
                return newData;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"AnalyzeFieldFeatures ({systemTypeName}): ОШИБКА при вызове AnalyzeFieldFeatures у реальной системы: {ex.Message}", ex);
                if (activeSystem == _primarySystem && _backupSystemFactory != null)
                {
                    Logger.Instance.Warning(SourceFilePath, "AnalyzeFieldFeatures: Попытка использовать резервную систему после сбоя основной...");
                    SwitchToBackupSystem();
                    IComputerVisionSystem backup = GetActiveSystem();
                    if (backup != null && backup == _backupSystem)
                    {
                        try
                        {
                            Logger.Instance.Info(SourceFilePath, "AnalyzeFieldFeatures: Повторный вызов AnalyzeFieldFeatures у резервной системы.");
                            return backup.AnalyzeFieldFeatures(currentTractorPosition);
                        }
                        catch (Exception backupEx)
                        {
                            Logger.Instance.Error(SourceFilePath, $"AnalyzeFieldFeatures: ОШИБКА и у резервной системы ({backup.GetType().Name}): {backupEx.Message}", backupEx);
                        }
                    }
                    else if (backup == _primarySystem)
                    {
                        Logger.Instance.Error(SourceFilePath, "AnalyzeFieldFeatures: Резервная система не смогла активироваться после сбоя основной.");
                    }
                }
                return new List<FieldFeatureData>();
            }
        }
    }
}