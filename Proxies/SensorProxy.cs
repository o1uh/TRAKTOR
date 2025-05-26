using Traktor.Interfaces; 
using Traktor.Core;      
namespace Traktor.Proxies
{
    /// <summary>
    /// Прокси для доступа к объектам, реализующим <see cref="ISensors{T}"/>.
    /// Обеспечивает ленивую инициализацию, кэширование данных и логирование.
    /// </summary>
    /// <typeparam name="T">Тип данных, возвращаемых сенсором.</typeparam>
    public class SensorProxy<T> : ISensors<T>
    {
        private readonly Func<ISensors<T>> _sensorFactory; // Фабрика для создания реального сенсора
        private ISensors<T> _realSensor;                   // Экземпляр реального сенсора
        private T _cachedData;                             // Кэшированные данные
        private DateTime _lastCacheUpdateTime;             // Время последнего обновления кэша
        private readonly TimeSpan _cacheDuration;          // Длительность жизни кэша

        private bool _isSensorInitialized = false;
        private static readonly object _lock = new object(); // Для потокобезопасной инициализации

        private const string SourceFilePath = "Proxies/SensorProxy.cs"; // Определяем константу

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SensorProxy{T}"/>.
        /// </summary>
        /// <param name="sensorFactory">Функция (фабричный метод) для создания экземпляра реального сенсора.
        /// Будет вызвана при первом обращении к GetData(), если сенсор еще не был создан.</param>
        /// <param name="cacheDuration">Продолжительность времени, на которое данные сенсора будут кэшироваться.
        /// Если <see cref="TimeSpan.Zero"/>, кэширование не используется.</param>
        public SensorProxy(Func<ISensors<T>> sensorFactory, TimeSpan cacheDuration)
        {
            _sensorFactory = sensorFactory ?? throw new ArgumentNullException(nameof(sensorFactory));
            _cacheDuration = cacheDuration;
            _lastCacheUpdateTime = DateTime.MinValue; // Гарантирует, что первое чтение обновит кэш

            Logger.Instance.Info(SourceFilePath, $"SensorProxy<{typeof(T).Name}> создан. Кэширование: {_cacheDuration.TotalSeconds}с. Реальный сенсор еще не инициализирован.");
        }

        /// <summary>
        /// Инициализирует реальный сенсор, если он еще не был инициализирован.
        /// Потокобезопасная реализация.
        /// </summary>
        private void EnsureSensorInitialized()
        {
            if (!_isSensorInitialized)
            {
                lock (_lock) 
                {
                    if (!_isSensorInitialized) 
                    {
                        Logger.Instance.Debug(SourceFilePath, $"SensorProxy<{typeof(T).Name}>: Инициализация реального сенсора через фабрику...");
                        _realSensor = _sensorFactory();
                        if (_realSensor == null)
                        {
                            Logger.Instance.Fatal(SourceFilePath, $"SensorProxy<{typeof(T).Name}>: ОШИБКА! Фабрика сенсоров вернула null.");
                            
                            throw new InvalidOperationException("Фабрика сенсоров не смогла создать экземпляр сенсора.");
                        }
                        _isSensorInitialized = true;
                        Logger.Instance.Info(SourceFilePath, $"SensorProxy<{typeof(T).Name}>: Реальный сенсор ({_realSensor.GetType().Name}) успешно инициализирован.");
                    }
                }
            }
        }

        /// <inheritdoc/>
        public T GetData()
        {
            EnsureSensorInitialized(); // Гарантируем, что реальный сенсор создан

            // Проверка, не истекло ли время кэша и есть ли валидные данные в кэше
            bool useCache = _cacheDuration > TimeSpan.Zero &&
                            _cachedData != null && 
                            (DateTime.Now - _lastCacheUpdateTime) < _cacheDuration;

            if (useCache)
            {
                Logger.Instance.Debug(SourceFilePath, $"SensorProxy<{typeof(T).Name}>: GetData() - Возвращаем данные из кэша. Кэш валиден до: {(_lastCacheUpdateTime + _cacheDuration):yyyy-MM-dd HH:mm:ss.fff}");
                return _cachedData;
            }

            // Если кэш не используется или истек
            Logger.Instance.Debug(SourceFilePath, $"SensorProxy<{typeof(T).Name}>: GetData() - Кэш не используется или истек. Запрос данных от реального сенсора ({_realSensor.GetType().Name})...");

            T newData = default(T);
            try
            {
                newData = _realSensor.GetData();
                _cachedData = newData;
                _lastCacheUpdateTime = DateTime.Now;
                Logger.Instance.Debug(SourceFilePath, $"SensorProxy<{typeof(T).Name}>: GetData() - Данные получены от реального сенсора и кэшированы. Новое время обновления кэша: {_lastCacheUpdateTime:yyyy-MM-dd HH:mm:ss.fff}");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"SensorProxy<{typeof(T).Name}>: GetData() - ОШИБКА при получении данных от реального сенсора: {ex.Message}. Возвращаем default(T).", ex);
                // if (_cachedData != null) return _cachedData;
                // else return default(T);
            }
            return newData;
        }
    }
}