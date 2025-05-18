namespace Traktor.DataModels
{
    /// <summary>
    /// Представляет данные, полученные от датчика состояния почвы.
    /// </summary>
    public struct SoilSensorData
    {
        /// <summary>
        /// Влажность почвы (например, в процентах).
        /// </summary>
        public double Moisture { get; }

        /// <summary>
        /// Температура почвы (например, в градусах Цельсия).
        /// </summary>
        public double Temperature { get; }

        // Можно добавить и другие параметры в будущем, если нужно (pH, электропроводность и т.д.)

        /// <summary>
        /// Инициализирует новый экземпляр структуры <see cref="SoilSensorData"/>.
        /// </summary>
        /// <param name="moisture">Значение влажности почвы.</param>
        /// <param name="temperature">Значение температуры почвы.</param>
        public SoilSensorData(double moisture, double temperature)
        {
            Moisture = moisture;
            Temperature = temperature;
        }

        /// <summary>
        /// Возвращает строковое представление данных о состоянии почвы.
        /// </summary>
        /// <returns>Строка с информацией о влажности и температуре почвы.</returns>
        public override string ToString()
        {
            return $"Почва: Влажность={Moisture:F1}%, Температура={Temperature:F1}°C";
        }
    }
}