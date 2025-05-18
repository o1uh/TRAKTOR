using Traktor.Interfaces; // Для IComputerVisionSystem
using Traktor.DataModels; // Для ObstacleData, FieldFeatureData

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

        private const string LogPrefix = "[Proxies/VisionSystemProxy.cs]";
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

            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: VisionSystemProxy создан. Кэширование: {_cacheDuration.TotalSeconds}с. Резервная система: {(_backupSystemFactory != null ? "предусмотрена" : "нет")}.");
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
                        Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Инициализация основной системы зрения...");
                        _primarySystem = _primarySystemFactory();
                        if (_primarySystem == null) throw new InvalidOperationException("Фабрика основной системы зрения вернула null.");
                        _isPrimarySystemInitialized = true;
                        Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Основная система зрения ({_primarySystem.GetType().Name}) успешно инициализирована.");
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
                    if (!_isBackupSystemInitialized) // Двойная проверка
                    {
                        Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Инициализация резервной системы зрения...");
                        _backupSystem = _backupSystemFactory();
                        if (_backupSystem == null)
                        {
                            // Не фатально, если резервная не создалась, просто логируем
                            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ОШИБКА: Фабрика резервной системы зрения вернула null. Резервная система не будет доступна.");
                            _isBackupSystemInitialized = false; // Явно указываем, что инициализация не удалась
                        }
                        else
                        {
                            _isBackupSystemInitialized = true;
                            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Резервная система зрения ({_backupSystem.GetType().Name}) успешно инициализирована.");
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
                // Если бэкап должен был использоваться, но не инициализировался, пытаемся основную
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Резервная система была выбрана, но не инициализирована. Попытка использовать основную.");
                _useBackupSystem = false; // Сбрасываем флаг, раз бэкап не удался
            }

            EnsurePrimarySystemInitialized();
            // Здесь можно добавить логику проверки "здоровья" основной системы,
            // и если она не отвечает, и activateBackupIfAvailable=true, то переключиться на резервную:
            return _primarySystem;
        }

        /// <summary>
        /// Принудительно переключает на использование резервной системы, если она доступна.
        /// </summary>
        public void SwitchToBackupSystem()
        {
            if (_backupSystemFactory == null)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SwitchToBackupSystem: Резервная система не предусмотрена.");
                return;
            }
            EnsureBackupSystemInitialized(); // Убедимся, что она создана
            if (_isBackupSystemInitialized)
            {
                _useBackupSystem = true;
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SwitchToBackupSystem: Прокси переключен на использование резервной системы зрения ({_backupSystem.GetType().Name}).");
            }
            else
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SwitchToBackupSystem: Не удалось переключиться на резервную систему (не инициализирована).");
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
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SwitchToPrimarySystem: Прокси переключен на использование основной системы зрения.");
            }
        }


        /// <inheritdoc/>
        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            IComputerVisionSystem activeSystem = GetActiveSystem(true); // true - разрешить переключение на бэкап при сбое (пока не реализовано)
            if (activeSystem == null)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: Активная система зрения недоступна.");
                return new List<ObstacleData>();
            }
            string systemTypeName = activeSystem.GetType().Name;

            bool positionChangedSignificantly = Math.Abs(currentTractorPosition.Latitude - _lastObstaclesQueryPosition.Latitude) > CACHE_POSITION_TOLERANCE ||
                                                Math.Abs(currentTractorPosition.Longitude - _lastObstaclesQueryPosition.Longitude) > CACHE_POSITION_TOLERANCE;

            if (_cacheDuration > TimeSpan.Zero && !positionChangedSignificantly && _cachedObstacles != null && (DateTime.Now - _lastObstaclesCacheTime) < _cacheDuration)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles ({systemTypeName}): Возвращаем данные из кэша для позиции {currentTractorPosition}.");
                return new List<ObstacleData>(_cachedObstacles); // Возвращаем копию кэша
            }

            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles ({systemTypeName}): Кэш не используется или истек/позиция изменилась. Запрос к системе ({systemTypeName}) для позиции {currentTractorPosition}.");
            try
            {
                List<ObstacleData> newData = activeSystem.DetectObstacles(currentTractorPosition);
                _cachedObstacles = newData != null ? new List<ObstacleData>(newData) : new List<ObstacleData>(); // Кэшируем копию или пустой список
                _lastObstaclesCacheTime = DateTime.Now;
                _lastObstaclesQueryPosition = currentTractorPosition;
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles ({systemTypeName}): Данные получены и кэшированы ({_cachedObstacles.Count} препятствий).");
                return newData; // Возвращаем оригинал, полученный от системы
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles ({systemTypeName}): ОШИБКА при вызове DetectObstacles у реальной системы: {ex.Message}");
                // Здесь можно попытаться переключиться на резервную систему, если это не текущая резервная
                if (activeSystem == _primarySystem && _backupSystemFactory != null)
                {
                    Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: Попытка использовать резервную систему после сбоя основной...");
                    SwitchToBackupSystem();
                    IComputerVisionSystem backup = GetActiveSystem(); // Получаем бэкап (он теперь должен быть активным)
                    if (backup != null && backup != _primarySystem) // Убеждаемся что это действительно бэкап
                    {
                        try { return backup.DetectObstacles(currentTractorPosition); } // Без кэширования для бэкапа при сбое
                        catch (Exception backupEx) { Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: ОШИБКА и у резервной системы: {backupEx.Message}"); }
                    }
                }
                return new List<ObstacleData>(); // Возвращаем пустой список в случае ошибки
            }
        }

        /// <inheritdoc/>
        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition)
        {
            IComputerVisionSystem activeSystem = GetActiveSystem(true);
            if (activeSystem == null)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures: Активная система зрения недоступна.");
                return new List<FieldFeatureData>();
            }
            string systemTypeName = activeSystem.GetType().Name;

            bool positionChangedSignificantly = Math.Abs(currentTractorPosition.Latitude - _lastFeaturesQueryPosition.Latitude) > CACHE_POSITION_TOLERANCE ||
                                                Math.Abs(currentTractorPosition.Longitude - _lastFeaturesQueryPosition.Longitude) > CACHE_POSITION_TOLERANCE;

            if (_cacheDuration > TimeSpan.Zero && !positionChangedSignificantly && _cachedFeatures != null && (DateTime.Now - _lastFeaturesCacheTime) < _cacheDuration)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures ({systemTypeName}): Возвращаем данные из кэша для позиции {currentTractorPosition}.");
                return new List<FieldFeatureData>(_cachedFeatures); // Копия кэша
            }

            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures ({systemTypeName}): Кэш не используется или истек/позиция изменилась. Запрос к системе ({systemTypeName}) для позиции {currentTractorPosition}.");
            try
            {
                List<FieldFeatureData> newData = activeSystem.AnalyzeFieldFeatures(currentTractorPosition);
                _cachedFeatures = newData != null ? new List<FieldFeatureData>(newData) : new List<FieldFeatureData>();
                _lastFeaturesCacheTime = DateTime.Now;
                _lastFeaturesQueryPosition = currentTractorPosition;
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures ({systemTypeName}): Данные получены и кэшированы ({_cachedFeatures.Count} особенностей).");
                return newData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures ({systemTypeName}): ОШИБКА при вызове AnalyzeFieldFeatures у реальной системы: {ex.Message}");
                if (activeSystem == _primarySystem && _backupSystemFactory != null)
                {
                    Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures: Попытка использовать резервную систему после сбоя основной...");
                    SwitchToBackupSystem();
                    IComputerVisionSystem backup = GetActiveSystem();
                    if (backup != null && backup != _primarySystem)
                    {
                        try { return backup.AnalyzeFieldFeatures(currentTractorPosition); }
                        catch (Exception backupEx) { Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures: ОШИБКА и у резервной системы: {backupEx.Message}"); }
                    }
                }
                return new List<FieldFeatureData>();
            }
        }
    }
}