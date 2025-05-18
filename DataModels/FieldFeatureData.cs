namespace Traktor.DataModels
{
    /// <summary>
    /// Перечисление типов особенностей поля, которые могут быть обнаружены системой компьютерного зрения.
    /// </summary>
    public enum FeatureType
    {
        /// <summary>
        /// Неизвестный или неопределенный тип особенности.
        /// </summary>
        Unknown,
        /// <summary>
        /// Обнаружен опасный сорняк.
        /// </summary>
        DangerousWeed,
        /// <summary>
        /// Обнаружен заболоченный участок (например, по анализу цвета или отражательной способности).
        /// </summary>
        WaterLogging,
        /// <summary>
        /// Обнаружен очаг заражения вредителями (например, по изменению цвета или текстуры растений).
        /// </summary>
        PestInfestation
        // Можно добавить другие типы по мере необходимости
    }

    /// <summary>
    /// Представляет информацию об обнаруженной особенности на поле.
    /// </summary>
    public struct FieldFeatureData
    {
        /// <summary>
        /// Координаты, где обнаружена особенность.
        /// </summary>
        public Coordinates Position { get; }

        /// <summary>
        /// Тип обнаруженной особенности.
        /// </summary>
        public FeatureType Type { get; }

        /// <summary>
        /// Дополнительные детали или описание особенности, если применимо.
        /// </summary>
        public string Details { get; }

        /// <summary>
        /// Инициализирует новый экземпляр структуры <see cref="FieldFeatureData"/>.
        /// </summary>
        /// <param name="position">Координаты особенности.</param>
        /// <param name="type">Тип особенности.</param>
        /// <param name="details">Дополнительные детали (опционально).</param>
        public FieldFeatureData(Coordinates position, FeatureType type, string details = "")
        {
            Position = position;
            Type = type;
            Details = details ?? string.Empty; // Гарантируем, что details не null
        }

        /// <summary>
        /// Возвращает строковое представление данных об особенности поля.
        /// </summary>
        /// <returns>Строка с типом, позицией и деталями (если есть) особенности.</returns>
        public override string ToString()
        {
            return $"Особенность поля: {Type} в {Position}{(string.IsNullOrEmpty(Details) ? "" : $" ({Details})")}";
        }
    }
}