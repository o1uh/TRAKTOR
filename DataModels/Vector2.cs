namespace TractorAutopilot.DataModels
{
    /// <summary>
    /// Представляет двумерный вектор или координату.
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// Координата X.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Координата Y.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр структуры <see cref="Vector2"/>.
        /// </summary>
        /// <param name="x">Координата X.</param>
        /// <param name="y">Координата Y.</param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Возвращает строковое представление вектора.
        /// </summary>
        /// <returns>Строка, представляющая вектор.</returns>
        public override string ToString() => $"({X}, {Y})";
    }
}