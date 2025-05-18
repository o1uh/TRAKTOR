namespace Traktor.DataModels
{
    /// <summary>
    /// ������������ ���������� �� ������������ �����������.
    /// </summary>
    public struct ObstacleData
    {
        /// <summary>
        /// ���������� �����������.
        /// </summary>
        public Coordinates Position { get; }

        /// <summary>
        /// �������� ��� ��� �����������.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// �������������� ����� ��������� ��������� <see cref="ObstacleData"/>.
        /// </summary>
        /// <param name="position">���������� �����������.</param>
        /// <param name="description">�������� ��� ��� �����������. ���� null, ����� ����������� ������ ������.</param>
        public ObstacleData(Coordinates position, string description)
        {
            Position = position;
            Description = description ?? string.Empty;
        }

        /// <summary>
        /// ���������� ��������� ������������� ������ � �����������.
        /// </summary>
        /// <returns>������ � ��������� � �������� �����������.</returns>
        public override string ToString()
        {
            return $"�����������: \"{Description}\" � {Position}";
        }
    }
}