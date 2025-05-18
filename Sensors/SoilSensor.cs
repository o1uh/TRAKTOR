using Traktor.Interfaces;
using Traktor.DataModels;

namespace Traktor.Sensors
{
    /// <summary>
    /// Имитирует работу датчика состояния почвы.
    /// </summary>
    public class SoilSensor : ISensors<SoilSensorData>
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SoilSensor"/>.
        /// </summary>
        public SoilSensor()
        {
            Console.WriteLine($"[Sensors/SoilSensor.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Датчик почвы инициализирован.");
        }

        /// <summary>
        /// Имитирует получение данных о состоянии почвы.
        /// </summary>
        /// <returns>Структура <see cref="SoilSensorData"/> с симулированными данными о влажности и температуре почвы.</returns>
        public SoilSensorData GetData()
        {
            // В реальной системе здесь будет код для взаимодействия с аппаратным датчиком.
            // Для макета имитируем данные.
            double moisture = 10.0 + _random.NextDouble() * (60.0 - 10.0); // Влажность от 10% до 60%
            double temperature = 5.0 + _random.NextDouble() * (25.0 - 5.0);  // Температура от 5°C до 25°C

            var data = new SoilSensorData(moisture, temperature);
            Console.WriteLine($"[Sensors/SoilSensor.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Получены данные от датчика почвы: {data}");
            return data;
        }
    }
}