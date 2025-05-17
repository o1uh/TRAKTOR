namespace Traktor.DataModels
{
    public struct Coordinates
    {
        public double Latitude { get; set; }  // ������
        public double Longitude { get; set; } // �������

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            return $"���: {Latitude}, ����: {Longitude}";
        }
    }
}