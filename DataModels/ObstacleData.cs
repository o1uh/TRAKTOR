namespace Traktor.DataModels
{
    public struct ObstacleData
    {
        public Coordinates Position { get; } // ���������� �����������
        public string Description { get; }   // ��������/�������/��� ����������� � ���� ������

        public ObstacleData(Coordinates position, string description)
        {
            Position = position;
            Description = description ?? string.Empty; // �����������, ��� �� null
        }

        public override string ToString()
        {
            return $"�����������: {Description} � {Position}";
        }
    }
}