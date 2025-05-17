namespace Traktor.DataModels
{
    public struct ObstacleData
    {
        public Coordinates Position { get; } // Координаты препятствия
        public string Description { get; }   // Описание/причина/тип препятствия в виде строки

        public ObstacleData(Coordinates position, string description)
        {
            Position = position;
            Description = description ?? string.Empty; // Гарантируем, что не null
        }

        public override string ToString()
        {
            return $"Препятствие: {Description} в {Position}";
        }
    }
}