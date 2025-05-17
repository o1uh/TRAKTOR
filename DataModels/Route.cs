using System.Collections.Generic;

namespace TractorAutopilot.DataModels
{
    /// <summary>
    /// Представляет маршрут движения.
    /// </summary>
    public class Route
    {
        /// <summary>
        /// Получает или задает список путевых точек маршрута.
        /// </summary>
        public List<Vector2> Waypoints { get; set; } = new List<Vector2>();

        // Можно добавить другие свойства, например, идентификатор маршрута, тип и т.д.

        /// <summary>
        /// Возвращает строковое представление маршрута.
        /// </summary>
        /// <returns>Строка, описывающая маршрут.</returns>
        public override string ToString() => $"Маршрут с {Waypoints.Count} путевыми точками.";
    }
}