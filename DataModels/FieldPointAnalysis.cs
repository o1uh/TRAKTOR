namespace Traktor.DataModels
{
    public struct FieldPointAnalysis
    {
        public Coordinates Position { get; }        // Координаты анализируемой точки/участка
        public double SoilMoisture { get; }         // Влажность почвы (например, в %)
        public double SoilDensity { get; }          // Плотность почвы (например, в г/см^3 или условных единицах)

        public FieldPointAnalysis(Coordinates position, double soilMoisture, double soilDensity)
        {
            Position = position;
            SoilMoisture = soilMoisture;
            SoilDensity = soilDensity;
        }

        public override string ToString()
        {
            return $"Анализ точки {Position}: Влажность={SoilMoisture:F2}, Плотность={SoilDensity:F2}";
        }
    }
}