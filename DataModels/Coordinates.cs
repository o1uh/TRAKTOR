namespace Traktor.DataModels
{
    /// <summary>
    /// Представляет географические координаты.
    /// </summary>
    public struct Coordinates
    {
        /// <summary>
        /// Широта.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Долгота.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр структуры <see cref="Coordinates"/>.
        /// </summary>
        /// <param name="latitude">Широта.</param>
        /// <param name="longitude">Долгота.</param>
        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Возвращает строковое представление координат.
        /// </summary>
        /// <returns>Строка с широтой и долготой.</returns>
        public override string ToString()
        {
            return $"Широта: {Latitude:F6}, Долгота: {Longitude:F6}";
        }
    }
}