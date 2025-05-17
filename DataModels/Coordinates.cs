namespace Traktor.DataModels
{
    public struct Coordinates
    {
        public double Latitude { get; set; }  // Широта
        public double Longitude { get; set; } // Долгота

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            return $"Шир: {Latitude}, Долг: {Longitude}";
        }
    }
}