using Traktor.Interfaces; // ��� ISensors<T>

namespace Traktor.Proxies
{
    /// <summary>
    /// ������ ��� ������� � ��������, ����������� <see cref="ISensors{T}"/>.
    /// ������������ ������� �������������, ����������� ������ � �����������.
    /// </summary>
    /// <typeparam name="T">��� ������, ������������ ��������.</typeparam>
    public class SensorProxy<T> : ISensors<T>
    {
        private readonly Func<ISensors<T>> _sensorFactory; // ������� ��� �������� ��������� �������
        private ISensors<T> _realSensor;                   // ��������� ��������� �������
        private T _cachedData;                             // ������������ ������
        private DateTime _lastCacheUpdateTime;             // ����� ���������� ���������� ����
        private readonly TimeSpan _cacheDuration;          // ������������ ����� ����

        private bool _isSensorInitialized = false;
        private static readonly object _lock = new object(); // ��� ���������������� �������������

        private const string LogPrefix = "[Proxies/SensorProxy.cs]";

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SensorProxy{T}"/>.
        /// </summary>
        /// <param name="sensorFactory">������� (��������� �����) ��� �������� ���������� ��������� �������.
        /// ����� ������� ��� ������ ��������� � GetData(), ���� ������ ��� �� ��� ������.</param>
        /// <param name="cacheDuration">����������������� �������, �� ������� ������ ������� ����� ������������.
        /// ���� <see cref="TimeSpan.Zero"/>, ����������� �� ������������.</param>
        public SensorProxy(Func<ISensors<T>> sensorFactory, TimeSpan cacheDuration)
        {
            _sensorFactory = sensorFactory ?? throw new ArgumentNullException(nameof(sensorFactory));
            _cacheDuration = cacheDuration;
            _lastCacheUpdateTime = DateTime.MinValue; // �����������, ��� ������ ������ ������� ���

            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SensorProxy<{typeof(T).Name}> ������. �����������: {_cacheDuration.TotalSeconds}�. �������� ������ ��� �� ���������������.");
        }

        /// <summary>
        /// �������������� �������� ������, ���� �� ��� �� ��� ���������������.
        /// ���������������� ����������.
        /// </summary>
        private void EnsureSensorInitialized()
        {
            if (!_isSensorInitialized)
            {
                lock (_lock) // ���������� ��� �������������� ������������ ������������� � ������������� �����
                {
                    if (!_isSensorInitialized) // ������� �������� �� ������, ���� ������ ����� ��� ���������������
                    {
                        Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SensorProxy<{typeof(T).Name}>: ������������� ��������� ������� ����� �������...");
                        _realSensor = _sensorFactory();
                        if (_realSensor == null)
                        {
                            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SensorProxy<{typeof(T).Name}>: ������! ������� �������� ������� null.");
                            // ����� ��������� ����������, ���� ��� ��������
                            throw new InvalidOperationException("������� �������� �� ������ ������� ��������� �������.");
                        }
                        _isSensorInitialized = true;
                        Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SensorProxy<{typeof(T).Name}>: �������� ������ ({_realSensor.GetType().Name}) ������� ���������������.");
                    }
                }
            }
        }

        /// <inheritdoc/>
        public T GetData()
        {
            EnsureSensorInitialized(); // �����������, ��� �������� ������ ������

            // ��������, �� ������� �� ����� ���� � ���� �� �������� ������ � ����
            bool useCache = _cacheDuration > TimeSpan.Zero &&
                            _cachedData != null && // ��������, ��� ��� �� ������ (�������� ��� reference types)
                                                   // ��� value types (_cachedData is T defaultT && defaultT.Equals(_cachedData)) ���� �� �������,
                                                   // ������� DateTime.MinValue ��� _lastCacheUpdateTime - ������� ��������� "��� ����"
                            (DateTime.Now - _lastCacheUpdateTime) < _cacheDuration;

            if (useCache)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SensorProxy<{typeof(T).Name}>: GetData() - ���������� ������ �� ����. ��� ������� ��: {(_lastCacheUpdateTime + _cacheDuration):yyyy-MM-dd HH:mm:ss.fff}");
                return _cachedData;
            }

            // ���� ��� �� ������������ ��� �����
            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SensorProxy<{typeof(T).Name}>: GetData() - ��� �� ������������ ��� �����. ������ ������ �� ��������� ������� ({_realSensor.GetType().Name})...");

            T newData = default(T); // �������� �� ��������� �� ������ ������
            try
            {
                newData = _realSensor.GetData();
                _cachedData = newData;
                _lastCacheUpdateTime = DateTime.Now;
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SensorProxy<{typeof(T).Name}>: GetData() - ������ �������� �� ��������� ������� � ����������. ����� ����� ���������� ����: {_lastCacheUpdateTime:yyyy-MM-dd HH:mm:ss.fff}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SensorProxy<{typeof(T).Name}>: GetData() - ������ ��� ��������� ������ �� ��������� �������: {ex.Message}. ���������� default(T).");
                // ����� ������, ��� ������ � ����� � ������ ������ - ���������� ��� ��������� ������.
                // ���� ��������� ������ (���� �� ���), ��� default(T) ���� ��� �� ����.
                // ���� ����� ������� ��������� ������� �������� �� ����, ���� ���� �� "�����", �� ���� ������:
                // if (_cachedData != null) return _cachedData;
                // else return default(T);
            }
            return newData;
        }
    }
}