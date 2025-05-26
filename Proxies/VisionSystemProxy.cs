using Traktor.Interfaces; 
using Traktor.DataModels; 
using Traktor.Core;       

namespace Traktor.Proxies
{
    /// <summary>
    /// ������ ��� ������� � <see cref="IComputerVisionSystem"/>.
    /// ������������ ������� �������������, ����������� ����������� � �����������.
    /// ����� ��������� �������� � ��������� ��������� ������������ ������.
    /// </summary>
    public class VisionSystemProxy : IComputerVisionSystem
    {
        private readonly Func<IComputerVisionSystem> _primarySystemFactory;
        private IComputerVisionSystem _primarySystem;
        private bool _isPrimarySystemInitialized = false;

        // ������������ ��������� ������� (��������, LiDAR, ���� �������� - ������)
        private readonly Func<IComputerVisionSystem> _backupSystemFactory;
        private IComputerVisionSystem _backupSystem;
        private bool _isBackupSystemInitialized = false;
        private bool _useBackupSystem = false; // ���� ��� ������������ �� ��������� �������

        // ����������� ��� DetectObstacles
        private List<ObstacleData> _cachedObstacles;
        private DateTime _lastObstaclesCacheTime;
        private Coordinates _lastObstaclesQueryPosition; // ��� ������ �������� �� �������

        // ����������� ��� AnalyzeFieldFeatures
        private List<FieldFeatureData> _cachedFeatures;
        private DateTime _lastFeaturesCacheTime;
        private Coordinates _lastFeaturesQueryPosition; // ��� ������ �������� �� �������

        private readonly TimeSpan _cacheDuration;
        private static readonly object _primaryLock = new object();
        private static readonly object _backupLock = new object();

        private const string SourceFilePath = "Proxies/VisionSystemProxy.cs";
        private const double CACHE_POSITION_TOLERANCE = 0.00001; // ������ ��� ��������� ��������� ��� �����������

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="VisionSystemProxy"/>.
        /// </summary>
        /// <param name="primarySystemFactory">������� ��� �������� ��������� ���������� <see cref="IComputerVisionSystem"/>.</param>
        /// <param name="cacheDuration">����������������� �������, �� ������� ���������� ����� ������������.</param>
        /// <param name="backupSystemFactory">������������ ������� ��� �������� ���������� ���������� <see cref="IComputerVisionSystem"/>.</param>
        public VisionSystemProxy(Func<IComputerVisionSystem> primarySystemFactory, TimeSpan cacheDuration, Func<IComputerVisionSystem> backupSystemFactory = null)
        {
            _primarySystemFactory = primarySystemFactory ?? throw new ArgumentNullException(nameof(primarySystemFactory));
            _cacheDuration = cacheDuration;
            _backupSystemFactory = backupSystemFactory; // ����� ���� null

            _lastObstaclesCacheTime = DateTime.MinValue;
            _lastFeaturesCacheTime = DateTime.MinValue;

            Logger.Instance.Info(SourceFilePath, $"VisionSystemProxy ������. �����������: {_cacheDuration.TotalSeconds}�. ��������� �������: {(_backupSystemFactory != null ? "�������������" : "���")}.");
        }

        /// <summary>
        /// �������������� �������� ������� ������������ ������, ���� ��� ��� �� ���� ����������������.
        /// </summary>
        private void EnsurePrimarySystemInitialized()
        {
            if (!_isPrimarySystemInitialized)
            {
                lock (_primaryLock)
                {
                    if (!_isPrimarySystemInitialized)
                    {
                        Logger.Instance.Debug(SourceFilePath, "������������� �������� ������� ������...");
                        _primarySystem = _primarySystemFactory();
                        if (_primarySystem == null)
                        {
                            Logger.Instance.Fatal(SourceFilePath, "������� �������� ������� ������ ������� null.");
                            throw new InvalidOperationException("������� �������� ������� ������ ������� null.");
                        }
                        _isPrimarySystemInitialized = true;
                        Logger.Instance.Info(SourceFilePath, $"�������� ������� ������ ({_primarySystem.GetType().Name}) ������� ����������������.");
                    }
                }
            }
        }

        /// <summary>
        /// �������������� ��������� ������� ������������ ������, ���� ��� ������������� � ��� �� ���� ����������������.
        /// </summary>
        private void EnsureBackupSystemInitialized()
        {
            if (_backupSystemFactory != null && !_isBackupSystemInitialized)
            {
                lock (_backupLock)
                {
                    if (!_isBackupSystemInitialized)
                    {
                        Logger.Instance.Debug(SourceFilePath, "������������� ��������� ������� ������...");
                        _backupSystem = _backupSystemFactory();
                        if (_backupSystem == null)
                        {
                            Logger.Instance.Error(SourceFilePath, "������: ������� ��������� ������� ������ ������� null. ��������� ������� �� ����� ��������.");
                            _isBackupSystemInitialized = false; 
                        }
                        else
                        {
                            _isBackupSystemInitialized = true;
                            Logger.Instance.Info(SourceFilePath, $"��������� ������� ������ ({_backupSystem.GetType().Name}) ������� ����������������.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ����������, ������� �� � ������ ������ ��������� �������.
        /// </summary>
        /// <param name="activateBackupIfAvailable">���� true, ���������� ������������ ��������� �������, ���� �������� �� ��������.</param>
        /// <returns>��������� �������� ������� ������ ��� null, ���� �� ���� �� ��������.</returns>
        private IComputerVisionSystem GetActiveSystem(bool activateBackupIfAvailable = false)
        {
            if (_useBackupSystem && _backupSystemFactory != null)
            {
                EnsureBackupSystemInitialized();
                if (_isBackupSystemInitialized) return _backupSystem;
                Logger.Instance.Warning(SourceFilePath, "��������� ������� ���� �������, �� �� ����������������. ������� ������������ ��������.");
                _useBackupSystem = false;
            }

            EnsurePrimarySystemInitialized();
            // ����� ����� �������� ������ �������� "��������" �������� �������,
            // � ���� ��� �� ��������, � activateBackupIfAvailable=true, �� ������������� �� ���������.
            return _primarySystem;
        }

        /// <summary>
        /// ������������� ����������� �� ������������� ��������� �������, ���� ��� ��������.
        /// </summary>
        public void SwitchToBackupSystem()
        {
            if (_backupSystemFactory == null)
            {
                Logger.Instance.Info(SourceFilePath, "SwitchToBackupSystem: ��������� ������� �� �������������.");
                return;
            }
            EnsureBackupSystemInitialized(); 
            if (_isBackupSystemInitialized)
            {
                _useBackupSystem = true;
                Logger.Instance.Info(SourceFilePath, $"SwitchToBackupSystem: ������ ���������� �� ������������� ��������� ������� ������ ({_backupSystem.GetType().Name}).");
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "SwitchToBackupSystem: �� ������� ������������� �� ��������� ������� (�� ����������������).");
            }
        }

        /// <summary>
        /// ����������� �� ������������� �������� �������.
        /// </summary>
        public void SwitchToPrimarySystem()
        {
            if (_useBackupSystem)
            {
                _useBackupSystem = false;
                Logger.Instance.Info(SourceFilePath, "SwitchToPrimarySystem: ������ ���������� �� ������������� �������� ������� ������.");
            }
            else { Logger.Instance.Debug(SourceFilePath, "SwitchToPrimarySystem: ��� ������������ �������� �������."); }
        }


        /// <inheritdoc/>
        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            IComputerVisionSystem activeSystem = GetActiveSystem(true);
            if (activeSystem == null)
            {
                Logger.Instance.Error(SourceFilePath, "DetectObstacles: �������� ������� ������ ����������.");
                return new List<ObstacleData>();
            }
            string systemTypeName = activeSystem.GetType().Name;

            bool positionChangedSignificantly = Math.Abs(currentTractorPosition.Latitude - _lastObstaclesQueryPosition.Latitude) > CACHE_POSITION_TOLERANCE ||
                                                Math.Abs(currentTractorPosition.Longitude - _lastObstaclesQueryPosition.Longitude) > CACHE_POSITION_TOLERANCE;

            if (_cacheDuration > TimeSpan.Zero && !positionChangedSignificantly && _cachedObstacles != null && (DateTime.Now - _lastObstaclesCacheTime) < _cacheDuration)
            {
                Logger.Instance.Debug(SourceFilePath, $"DetectObstacles ({systemTypeName}): ���������� ������ �� ���� ��� ������� {currentTractorPosition}.");
                return new List<ObstacleData>(_cachedObstacles); // ���������� ����� ����
            }

            Logger.Instance.Debug(SourceFilePath, $"DetectObstacles ({systemTypeName}): ��� �� ������������ ��� �����/������� ����������. ������ � ������� ({systemTypeName}) ��� ������� {currentTractorPosition}.");
            try
            {
                List<ObstacleData> newData = activeSystem.DetectObstacles(currentTractorPosition);
                _cachedObstacles = newData != null ? new List<ObstacleData>(newData) : new List<ObstacleData>();
                _lastObstaclesCacheTime = DateTime.Now;
                _lastObstaclesQueryPosition = currentTractorPosition;
                Logger.Instance.Debug(SourceFilePath, $"DetectObstacles ({systemTypeName}): ������ �������� � ���������� ({_cachedObstacles.Count} �����������).");
                return newData;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"DetectObstacles ({systemTypeName}): ������ ��� ������ DetectObstacles � �������� �������: {ex.Message}", ex);
                if (activeSystem == _primarySystem && _backupSystemFactory != null)
                {
                    Logger.Instance.Warning(SourceFilePath, "DetectObstacles: ������� ������������ ��������� ������� ����� ���� ��������...");
                    SwitchToBackupSystem(); 
                    IComputerVisionSystem backup = GetActiveSystem();
                    if (backup != null && backup == _backupSystem) 
                    {
                        try
                        {
                            Logger.Instance.Info(SourceFilePath, "DetectObstacles: ��������� ����� DetectObstacles � ��������� �������.");
                            return backup.DetectObstacles(currentTractorPosition); 
                        }
                        catch (Exception backupEx)
                        {
                            Logger.Instance.Error(SourceFilePath, $"DetectObstacles: ������ � � ��������� ������� ({backup.GetType().Name}): {backupEx.Message}", backupEx);
                        }
                    }
                    else if (backup == _primarySystem) 
                    {
                        Logger.Instance.Error(SourceFilePath, "DetectObstacles: ��������� ������� �� ������ �������������� ����� ���� ��������.");
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
                Logger.Instance.Error(SourceFilePath, "AnalyzeFieldFeatures: �������� ������� ������ ����������.");
                return new List<FieldFeatureData>();
            }
            string systemTypeName = activeSystem.GetType().Name;

            bool positionChangedSignificantly = Math.Abs(currentTractorPosition.Latitude - _lastFeaturesQueryPosition.Latitude) > CACHE_POSITION_TOLERANCE ||
                                                Math.Abs(currentTractorPosition.Longitude - _lastFeaturesQueryPosition.Longitude) > CACHE_POSITION_TOLERANCE;

            if (_cacheDuration > TimeSpan.Zero && !positionChangedSignificantly && _cachedFeatures != null && (DateTime.Now - _lastFeaturesCacheTime) < _cacheDuration)
            {
                Logger.Instance.Debug(SourceFilePath, $"AnalyzeFieldFeatures ({systemTypeName}): ���������� ������ �� ���� ��� ������� {currentTractorPosition}.");
                return new List<FieldFeatureData>(_cachedFeatures);
            }

            Logger.Instance.Debug(SourceFilePath, $"AnalyzeFieldFeatures ({systemTypeName}): ��� �� ������������ ��� �����/������� ����������. ������ � ������� ({systemTypeName}) ��� ������� {currentTractorPosition}.");
            try
            {
                List<FieldFeatureData> newData = activeSystem.AnalyzeFieldFeatures(currentTractorPosition);
                _cachedFeatures = newData != null ? new List<FieldFeatureData>(newData) : new List<FieldFeatureData>();
                _lastFeaturesCacheTime = DateTime.Now;
                _lastFeaturesQueryPosition = currentTractorPosition;
                Logger.Instance.Debug(SourceFilePath, $"AnalyzeFieldFeatures ({systemTypeName}): ������ �������� � ���������� ({_cachedFeatures.Count} ������������).");
                return newData;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"AnalyzeFieldFeatures ({systemTypeName}): ������ ��� ������ AnalyzeFieldFeatures � �������� �������: {ex.Message}", ex);
                if (activeSystem == _primarySystem && _backupSystemFactory != null)
                {
                    Logger.Instance.Warning(SourceFilePath, "AnalyzeFieldFeatures: ������� ������������ ��������� ������� ����� ���� ��������...");
                    SwitchToBackupSystem();
                    IComputerVisionSystem backup = GetActiveSystem();
                    if (backup != null && backup == _backupSystem)
                    {
                        try
                        {
                            Logger.Instance.Info(SourceFilePath, "AnalyzeFieldFeatures: ��������� ����� AnalyzeFieldFeatures � ��������� �������.");
                            return backup.AnalyzeFieldFeatures(currentTractorPosition);
                        }
                        catch (Exception backupEx)
                        {
                            Logger.Instance.Error(SourceFilePath, $"AnalyzeFieldFeatures: ������ � � ��������� ������� ({backup.GetType().Name}): {backupEx.Message}", backupEx);
                        }
                    }
                    else if (backup == _primarySystem)
                    {
                        Logger.Instance.Error(SourceFilePath, "AnalyzeFieldFeatures: ��������� ������� �� ������ �������������� ����� ���� ��������.");
                    }
                }
                return new List<FieldFeatureData>();
            }
        }
    }
}