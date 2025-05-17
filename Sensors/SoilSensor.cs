using Traktor.Interfaces;
using Traktor.DataModels;

namespace Traktor.Sensors
{
    public class SoilSensor : ISensors<SoilSensorData>
    {
        /// <summary>
        /// Имитирует получение данных о состоянии почвы.
        /// </summary>
        /// <returns>Структура <see cref="SoilSensorData"/> с данными о почве.</returns>
        public SoilSensorData GetData()
        {
            // В реальной системе здесь будет код для взаимодействия с аппаратным датчиком.
            // Для макета имитируем данные.
            Random rnd = new Random();

            double moisture = 10.0 + rnd.NextDouble() * (60.0 - 10.0); // Влажность от 10% до 60%
            double temperature = 5.0 + rnd.NextDouble() * (25.0 - 5.0);  // Температура от 5°C до 25°C

            var data = new SoilSensorData(moisture, temperature);
            Console.WriteLine($"[SoilSensor]: {data}"); // Временный вывод
            return data;
        }
    }
}