using Traktor.Interfaces;

namespace Traktor.Sensors
{
    public class DistanceSensor : ISensors<double>
    {
        // Можно добавить конструктор, если датчику нужна какая-то конфигурация
        // public DistanceSensor() { }

        /// <summary>
        /// Имитирует получение данных о расстоянии от датчика.
        /// </summary>
        /// <returns>Измеренное расстояние в метрах.</returns>
        public double GetData()
        {
            // В реальной системе здесь будет код для взаимодействия с аппаратным датчиком.
            // Для макета имитируем данные.
            Random rnd = new Random();
            // Предположим, датчик может "видеть" от 0.1 до 50 метров.
            // И иногда может вернуть "ошибку" или невалидное значение (например, -1 или double.NaN).
            // Пока для простоты вернем случайное валидное значение для тестов.
            double distance = 0.1 + rnd.NextDouble() * (50.0 - 0.1);
            Console.WriteLine($"[DistanceSensor]: Зафиксировано расстояние: {distance:F2} м"); // Временный вывод для отладки
            return distance;
        }
    }
}