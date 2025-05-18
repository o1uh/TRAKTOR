using Traktor.Interfaces;

namespace Traktor.Sensors
{
    /// <summary>
    /// Имитирует работу датчика расстояния.
    /// </summary>
    public class DistanceSensor : ISensors<double>
    {
        private static readonly Random _random = new Random(); // Лучше использовать один экземпляр Random

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DistanceSensor"/>.
        /// </summary>
        public DistanceSensor()
        {
            // Конструктор может быть использован для инициализации подключения к реальному датчику
            // или для установки начальных параметров симуляции.
            Console.WriteLine($"[Sensors/DistanceSensor.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Датчик расстояния инициализирован.");
        }

        /// <summary>
        /// Имитирует получение данных о расстоянии от датчика.
        /// </summary>
        /// <returns>Измеренное расстояние в метрах (случайное значение от 0.1 до 50.0 для макета).</returns>
        public double GetData()
        {
            // В реальной системе здесь будет код для взаимодействия с аппаратным датчиком.
            // Для макета имитируем данные.

            // Предположим, датчик может "видеть" от 0.1 до 50 метров.
            double distance = 0.1 + _random.NextDouble() * (50.0 - 0.1);
            Console.WriteLine($"[Sensors/DistanceSensor.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Зафиксировано расстояние: {distance:F2} м");
            return distance;
        }
    }
}