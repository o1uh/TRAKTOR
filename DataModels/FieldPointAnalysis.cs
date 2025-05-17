namespace Traktor.DataModels
{
    public struct FieldPointAnalysis
    {
        public Coordinates Position { get; }        // ���������� ������������� �����/�������
        public double SoilMoisture { get; }         // ��������� ����� (��������, � %)
        public double SoilDensity { get; }          // ��������� ����� (��������, � �/��^3 ��� �������� ��������)

        public FieldPointAnalysis(Coordinates position, double soilMoisture, double soilDensity)
        {
            Position = position;
            SoilMoisture = soilMoisture;
            SoilDensity = soilDensity;
        }

        public override string ToString()
        {
            return $"������ ����� {Position}: ���������={SoilMoisture:F2}, ���������={SoilDensity:F2}";
        }
    }
}