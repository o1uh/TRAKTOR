namespace TractorAutopilot.DataModels
{
    /// <summary>
    /// Представляет обнаруженное препятствие.
    /// </summary>
    public class Obstacle
    {
        /// <summary>
        /// Получает или задает позицию препятствия.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Получает или задает размер препятствия.
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// Получает или задает тип препятствия.
        /// </summary>
        public string Type { get; set; } = "Неизвестно";

        /// <summary>
        /// Возвращает строковое представление препятствия.
        /// </summary>
        /// <returns>Строка, описывающая препятствие.</returns>
        public override string ToString() => $"Препятствие типа '{Type}' в {Position} размером {Size}м.";
    }
}