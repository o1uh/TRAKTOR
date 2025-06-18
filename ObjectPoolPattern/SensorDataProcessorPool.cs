using System.Collections.Generic;
using Traktor.Core;             

namespace Traktor.ObjectPoolPattern
{
    /// <summary>
    /// Пул объектов для SensorDataProcessor.
    /// Управляет созданием, хранением и выдачей переиспользуемых объектов.
    /// </summary>
    public class SensorDataProcessorPool
    {
        private const string SourceFilePath = "ObjectPoolPattern/SensorDataProcessorPool.cs";
        private readonly List<SensorDataProcessor> _availableProcessors = new List<SensorDataProcessor>();
        private readonly List<SensorDataProcessor> _inUseProcessors = new List<SensorDataProcessor>();
        private readonly int _maxPoolSize;
        private readonly object _lock = new object(); 

        /// <summary>
        /// Инициализирует новый экземпляр пула объектов.
        /// </summary>
        /// <param name="initialSize">Начальное количество объектов в пуле.</param>
        /// <param name="maxSize">Максимальный размер пула. Если 0 или меньше, размер не ограничен (но не рекомендуется).</param>
        public SensorDataProcessorPool(int initialSize = 2, int maxSize = 5)
        {
            _maxPoolSize = maxSize > 0 ? maxSize : int.MaxValue; 

            Logger.Instance.Info(SourceFilePath, $"SensorDataProcessorPool создан. Начальный размер: {initialSize}, Максимальный размер: {(maxSize > 0 ? maxSize.ToString() : "не ограничен")}.");

            for (int i = 0; i < initialSize; i++)
            {
                if (_availableProcessors.Count + _inUseProcessors.Count < _maxPoolSize)
                {
                    _availableProcessors.Add(new SensorDataProcessor());
                }
                else break;
            }
            Logger.Instance.Info(SourceFilePath, $"Пул инициализирован. Доступно процессоров: {_availableProcessors.Count}.");
        }

        /// <summary>
        /// Получает доступный SensorDataProcessor из пула.
        /// Если в пуле нет свободных объектов и размер пула не достиг максимума, создает новый.
        /// Если пул пуст и максимум достигнут, может вернуть null или ожидать (в макете вернет null).
        /// </summary>
        /// <returns>Экземпляр SensorDataProcessor или null, если получить не удалось.</returns>
        public SensorDataProcessor AcquireProcessor()
        {
            lock (_lock) 
            {
                if (_availableProcessors.Count > 0)
                {
                    SensorDataProcessor processor = _availableProcessors[0];
                    _availableProcessors.RemoveAt(0);
                    _inUseProcessors.Add(processor);
                    Logger.Instance.Info(SourceFilePath, $"Процессор (ID: {processor.Id}) ВЗЯТ из пула. Доступно: {_availableProcessors.Count}, Используется: {_inUseProcessors.Count}.");
                    return processor;
                }
                else if (_inUseProcessors.Count < _maxPoolSize)
                {
                    Logger.Instance.Info(SourceFilePath, "В пуле нет свободных процессоров. Попытка создать новый (лимит не достигнут)...");
                    SensorDataProcessor newProcessor = new SensorDataProcessor();
                    _inUseProcessors.Add(newProcessor);
                    Logger.Instance.Info(SourceFilePath, $"Новый процессор (ID: {newProcessor.Id}) создан и выдан. Доступно: {_availableProcessors.Count}, Используется: {_inUseProcessors.Count}.");
                    return newProcessor;
                }
                else
                {
                    Logger.Instance.Warning(SourceFilePath, "В пуле нет свободных процессоров, и максимальный размер пула достигнут! Не удалось выдать процессор.");
                    return null; 
                }
            }
        }

        /// <summary>
        /// Возвращает SensorDataProcessor обратно в пул.
        /// </summary>
        /// <param name="processor">Объект для возврата в пул.</param>
        public void ReleaseProcessor(SensorDataProcessor processor)
        {
            if (processor == null)
            {
                Logger.Instance.Warning(SourceFilePath, "Попытка вернуть null процессор в пул.");
                return;
            }

            lock (_lock)
            {
                if (_inUseProcessors.Contains(processor))
                {
                    processor.Reset(); 
                    _inUseProcessors.Remove(processor);
                    _availableProcessors.Add(processor);
                    Logger.Instance.Info(SourceFilePath, $"Процессор (ID: {processor.Id}) ВОЗВРАЩЕН в пул. Доступно: {_availableProcessors.Count}, Используется: {_inUseProcessors.Count}.");
                }
                else
                {
                    Logger.Instance.Warning(SourceFilePath, $"Попытка вернуть в пул процессор (ID: {processor.Id}), который не был из него взят или уже возвращен.");
                }
            }
        }

        public int GetAvailableCount() => _availableProcessors.Count;
        public int GetInUseCount() => _inUseProcessors.Count;
    }
}