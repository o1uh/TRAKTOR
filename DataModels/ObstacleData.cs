namespace Traktor.DataModels
{
    /// <summary>
    /// Представляет информацию об обнаруженном препятствии.
    /// </summary>
    public struct ObstacleData
    {
        /// <summary>
        /// Координаты препятствия.
        /// </summary>
        public Coordinates Position { get; }

        /// <summary>
        /// Описание или тип препятствия.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Инициализирует новый экземпляр структуры <see cref="ObstacleData"/>.
        /// </summary>
        /// <param name="position">Координаты препятствия.</param>
        /// <param name="description">Описание или тип препятствия. Если null, будет установлена пустая строка.</param>
        public ObstacleData(Coordinates position, string description)
        {
            Position = position;
            Description = description ?? string.Empty;
        }

        /// <summary>
        /// Возвращает строковое представление данных о препятствии.
        /// </summary>
        /// <returns>Строка с описанием и позицией препятствия.</returns>
        public override string ToString()
        {
            return $"Препятствие: \"{Description}\" в {Position}";
        }
    }
}