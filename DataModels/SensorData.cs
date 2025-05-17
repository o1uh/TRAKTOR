using System;

namespace TractorAutopilot.DataModels
{
    /// <summary>
    /// Базовый класс для данных, получаемых с датчиков.
    /// </summary>
    public abstract class SensorDataBase
    {
        /// <summary>
        /// Получает временную метку получения данных.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="SensorDataBase"/>.
        /// </summary>
        protected SensorDataBase()
        {
            Timestamp = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Представляет данные, полученные с датчика.
    /// </summary>
    /// <typeparam name="T">Тип значения данных.</typeparam>
    public class SensorData<T> : SensorDataBase
    {
        /// <summary>
        /// Получает значение данных.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Получает или задает тип датчика или данных.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SensorData{T}"/>.
        /// </summary>
        /// <param name="value">Значение данных.</param>
        /// <param name="dataType">Тип данных/датчика (опционально).</param>
        public SensorData(T value, string dataType = null)
        {
            Value = value;
            DataType = dataType ?? typeof(T).Name;
        }

        /// <summary>
        /// Возвращает строковое представление данных датчика.
        /// </summary>
        /// <returns>Строка, описывающая данные датчика.</returns>
        public override string ToString() => $"Тип: {DataType}, Значение: {Value} ({Timestamp})";
    }

    // Примеры конкретных типов данных для сенсоров:
    public class DistanceData
    {
        public float Meters { get; }
        public DistanceData(float meters) { Meters = meters; }
        public override string ToString() => $"{Meters} м";
    }

    public class SoilMoistureData
    {
        public float Percentage { get; }
        public SoilMoistureData(float percentage) { Percentage = percentage; }
        public override string ToString() => $"{Percentage}%";
    }

    public class ImageData // Может содержать байты изображения или путь к файлу
    {
        public byte[] RawData { get; }
        public int Width { get; }
        public int Height { get; }
        public ImageData(byte[] rawData, int width, int height)
        {
            RawData = rawData;
            Width = width;
            Height = height;
        }
        public override string ToString() => $"Изображение {Width}x{Height}, {RawData?.Length ?? 0} байт";
    }
}