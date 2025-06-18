using System; 
using Traktor.Core;

namespace Traktor.ObjectPoolPattern
{
    /// <summary>
    /// Переиспользуемый объект: Обработчик данных сенсоров.
    /// Имитирует ресурсоемкий объект, который выгодно переиспользовать.
    /// </summary>
    public class SensorDataProcessor
    {
        private const string SourceFilePath = "ObjectPoolPattern/SensorDataProcessor.cs";
        public Guid Id { get; } 
        private bool _isInUse;

        public SensorDataProcessor()
        {
            Id = Guid.NewGuid();
            _isInUse = false; 
            Logger.Instance.Info(SourceFilePath, $"СОЗДАН НОВЫЙ SensorDataProcessor. ID: {Id}. Имитация дорогой инициализации...");
            System.Threading.Thread.Sleep(100); 
        }

        /// <summary>
        /// Имитирует обработку данных.
        /// </summary>
        /// <param name="data">Данные для обработки.</param>
        public void Process(string data)
        {
            if (_isInUse)
            {
                Logger.Instance.Warning(SourceFilePath, $"SensorDataProcessor (ID: {Id}): Попытка использовать объект, который уже используется!");
            }
            _isInUse = true;
            Logger.Instance.Info(SourceFilePath, $"SensorDataProcessor (ID: {Id}): Начало обработки данных '{data}'...");
            System.Threading.Thread.Sleep(50);
            Logger.Instance.Info(SourceFilePath, $"SensorDataProcessor (ID: {Id}): Обработка данных '{data}' завершена.");
        }

        /// <summary>
        /// "Сбрасывает" состояние объекта для подготовки к повторному использованию.
        /// Вызывается при возвращении объекта в пул.
        /// </summary>
        public void Reset()
        {
            _isInUse = false;
            Logger.Instance.Info(SourceFilePath, $"SensorDataProcessor (ID: {Id}): Состояние сброшено. Готов к повторному использованию.");
        }

        public bool IsInUse() => _isInUse;
    }
}