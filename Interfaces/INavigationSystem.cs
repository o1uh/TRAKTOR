// Interfaces/INavigationSystem.cs
namespace TractorAutopilot.Interfaces
{
    public interface INavigationSystem
    {
        /// <summary>
        /// Возвращает текущее местоположение трактора.
        /// </summary>
        /// <returns>Текущее местоположение трактора.</returns>
        Location GetPosition();

        /// <summary>
        /// Вычисляет оптимальный маршрут движения трактора.
        /// </summary>
        /// <param name="destination">Конечная точка маршрута.</param>
        /// <returns>Маршрут движения.</returns>
        Route CalculateRoute(Location destination);

        /// <summary>
        /// Корректирует текущий маршрут движения.
        /// </summary>
        /// <returns>Скорректированный маршрут движения.</returns>
        Route AdjustRoute();
    }

    // Вспомогательные структуры для представления данных
    public struct Location
    {
        public double Latitude;
        public double Longitude;
    }

    public struct Route
    {
        public Location[] Waypoints;
    }
}