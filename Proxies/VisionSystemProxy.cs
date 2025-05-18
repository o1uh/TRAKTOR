using Traktor.Interfaces; // ��� IComputerVisionSystem
using Traktor.DataModels; // ��� ObstacleData, FieldFeatureData

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

        private const string LogPrefix = "[Proxies/VisionSystemProxy.cs]";
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

            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: VisionSystemProxy ������. �����������: {_cacheDuration.TotalSeconds}�. ��������� �������: {(_backupSystemFactory != null ? "�������������" : "���")}.");
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
                        Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ������������� �������� ������� ������...");
                        _primarySystem = _primarySystemFactory();
                        if (_primarySystem == null) throw new InvalidOperationException("������� �������� ������� ������ ������� null.");
                        _isPrimarySystemInitialized = true;
                        Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: �������� ������� ������ ({_primarySystem.GetType().Name}) ������� ����������������.");
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
                    if (!_isBackupSystemInitialized) // ������� ��������
                    {
                        Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ������������� ��������� ������� ������...");
                        _backupSystem = _backupSystemFactory();
                        if (_backupSystem == null)
                        {
                            // �� ��������, ���� ��������� �� ���������, ������ ��������
                            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ������: ������� ��������� ������� ������ ������� null. ��������� ������� �� ����� ��������.");
                            _isBackupSystemInitialized = false; // ���� ���������, ��� ������������� �� �������
                        }
                        else
                        {
                            _isBackupSystemInitialized = true;
                            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ��������� ������� ������ ({_backupSystem.GetType().Name}) ������� ����������������.");
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
                // ���� ����� ������ ��� ��������������, �� �� �����������������, �������� ��������
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ��������� ������� ���� �������, �� �� ����������������. ������� ������������ ��������.");
                _useBackupSystem = false; // ���������� ����, ��� ����� �� ������
            }

            EnsurePrimarySystemInitialized();
            // ����� ����� �������� ������ �������� "��������" �������� �������,
            // � ���� ��� �� ��������, � activateBackupIfAvailable=true, �� ������������� �� ���������:
            return _primarySystem;
        }

        /// <summary>
        /// ������������� ����������� �� ������������� ��������� �������, ���� ��� ��������.
        /// </summary>
        public void SwitchToBackupSystem()
        {
            if (_backupSystemFactory == null)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SwitchToBackupSystem: ��������� ������� �� �������������.");
                return;
            }
            EnsureBackupSystemInitialized(); // ��������, ��� ��� �������
            if (_isBackupSystemInitialized)
            {
                _useBackupSystem = true;
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SwitchToBackupSystem: ������ ���������� �� ������������� ��������� ������� ������ ({_backupSystem.GetType().Name}).");
            }
            else
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SwitchToBackupSystem: �� ������� ������������� �� ��������� ������� (�� ����������������).");
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
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SwitchToPrimarySystem: ������ ���������� �� ������������� �������� ������� ������.");
            }
        }


        /// <inheritdoc/>
        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            IComputerVisionSystem activeSystem = GetActiveSystem(true); // true - ��������� ������������ �� ����� ��� ���� (���� �� �����������)
            if (activeSystem == null)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: �������� ������� ������ ����������.");
                return new List<ObstacleData>();
            }
            string systemTypeName = activeSystem.GetType().Name;

            bool positionChangedSignificantly = Math.Abs(currentTractorPosition.Latitude - _lastObstaclesQueryPosition.Latitude) > CACHE_POSITION_TOLERANCE ||
                                                Math.Abs(currentTractorPosition.Longitude - _lastObstaclesQueryPosition.Longitude) > CACHE_POSITION_TOLERANCE;

            if (_cacheDuration > TimeSpan.Zero && !positionChangedSignificantly && _cachedObstacles != null && (DateTime.Now - _lastObstaclesCacheTime) < _cacheDuration)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles ({systemTypeName}): ���������� ������ �� ���� ��� ������� {currentTractorPosition}.");
                return new List<ObstacleData>(_cachedObstacles); // ���������� ����� ����
            }

            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles ({systemTypeName}): ��� �� ������������ ��� �����/������� ����������. ������ � ������� ({systemTypeName}) ��� ������� {currentTractorPosition}.");
            try
            {
                List<ObstacleData> newData = activeSystem.DetectObstacles(currentTractorPosition);
                _cachedObstacles = newData != null ? new List<ObstacleData>(newData) : new List<ObstacleData>(); // �������� ����� ��� ������ ������
                _lastObstaclesCacheTime = DateTime.Now;
                _lastObstaclesQueryPosition = currentTractorPosition;
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles ({systemTypeName}): ������ �������� � ���������� ({_cachedObstacles.Count} �����������).");
                return newData; // ���������� ��������, ���������� �� �������
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles ({systemTypeName}): ������ ��� ������ DetectObstacles � �������� �������: {ex.Message}");
                // ����� ����� ���������� ������������� �� ��������� �������, ���� ��� �� ������� ���������
                if (activeSystem == _primarySystem && _backupSystemFactory != null)
                {
                    Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: ������� ������������ ��������� ������� ����� ���� ��������...");
                    SwitchToBackupSystem();
                    IComputerVisionSystem backup = GetActiveSystem(); // �������� ����� (�� ������ ������ ���� ��������)
                    if (backup != null && backup != _primarySystem) // ���������� ��� ��� ������������� �����
                    {
                        try { return backup.DetectObstacles(currentTractorPosition); } // ��� ����������� ��� ������ ��� ����
                        catch (Exception backupEx) { Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: ������ � � ��������� �������: {backupEx.Message}"); }
                    }
                }
                return new List<ObstacleData>(); // ���������� ������ ������ � ������ ������
            }
        }

        /// <inheritdoc/>
        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition)
        {
            IComputerVisionSystem activeSystem = GetActiveSystem(true);
            if (activeSystem == null)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures: �������� ������� ������ ����������.");
                return new List<FieldFeatureData>();
            }
            string systemTypeName = activeSystem.GetType().Name;

            bool positionChangedSignificantly = Math.Abs(currentTractorPosition.Latitude - _lastFeaturesQueryPosition.Latitude) > CACHE_POSITION_TOLERANCE ||
                                                Math.Abs(currentTractorPosition.Longitude - _lastFeaturesQueryPosition.Longitude) > CACHE_POSITION_TOLERANCE;

            if (_cacheDuration > TimeSpan.Zero && !positionChangedSignificantly && _cachedFeatures != null && (DateTime.Now - _lastFeaturesCacheTime) < _cacheDuration)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures ({systemTypeName}): ���������� ������ �� ���� ��� ������� {currentTractorPosition}.");
                return new List<FieldFeatureData>(_cachedFeatures); // ����� ����
            }

            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures ({systemTypeName}): ��� �� ������������ ��� �����/������� ����������. ������ � ������� ({systemTypeName}) ��� ������� {currentTractorPosition}.");
            try
            {
                List<FieldFeatureData> newData = activeSystem.AnalyzeFieldFeatures(currentTractorPosition);
                _cachedFeatures = newData != null ? new List<FieldFeatureData>(newData) : new List<FieldFeatureData>();
                _lastFeaturesCacheTime = DateTime.Now;
                _lastFeaturesQueryPosition = currentTractorPosition;
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures ({systemTypeName}): ������ �������� � ���������� ({_cachedFeatures.Count} ������������).");
                return newData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures ({systemTypeName}): ������ ��� ������ AnalyzeFieldFeatures � �������� �������: {ex.Message}");
                if (activeSystem == _primarySystem && _backupSystemFactory != null)
                {
                    Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures: ������� ������������ ��������� ������� ����� ���� ��������...");
                    SwitchToBackupSystem();
                    IComputerVisionSystem backup = GetActiveSystem();
                    if (backup != null && backup != _primarySystem)
                    {
                        try { return backup.AnalyzeFieldFeatures(currentTractorPosition); }
                        catch (Exception backupEx) { Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures: ������ � � ��������� �������: {backupEx.Message}"); }
                    }
                }
                return new List<FieldFeatureData>();
            }
        }
    }
}