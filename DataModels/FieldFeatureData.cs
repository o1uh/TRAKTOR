namespace Traktor.DataModels
{
    /// <summary>
    /// ������������ ����� ������������ ����, ������� ����� ���� ���������� �������� ������������� ������.
    /// </summary>
    public enum FeatureType
    {
        /// <summary>
        /// ����������� ��� �������������� ��� �����������.
        /// </summary>
        Unknown,
        /// <summary>
        /// ��������� ������� ������.
        /// </summary>
        DangerousWeed,
        /// <summary>
        /// ��������� ������������ ������� (��������, �� ������� ����� ��� ������������� �����������).
        /// </summary>
        WaterLogging,
        /// <summary>
        /// ��������� ���� ��������� ����������� (��������, �� ��������� ����� ��� �������� ��������).
        /// </summary>
        PestInfestation
        // ����� �������� ������ ���� �� ���� �������������
    }

    /// <summary>
    /// ������������ ���������� �� ������������ ����������� �� ����.
    /// </summary>
    public struct FieldFeatureData
    {
        /// <summary>
        /// ����������, ��� ���������� �����������.
        /// </summary>
        public Coordinates Position { get; }

        /// <summary>
        /// ��� ������������ �����������.
        /// </summary>
        public FeatureType Type { get; }

        /// <summary>
        /// �������������� ������ ��� �������� �����������, ���� ���������.
        /// </summary>
        public string Details { get; }

        /// <summary>
        /// �������������� ����� ��������� ��������� <see cref="FieldFeatureData"/>.
        /// </summary>
        /// <param name="position">���������� �����������.</param>
        /// <param name="type">��� �����������.</param>
        /// <param name="details">�������������� ������ (�����������).</param>
        public FieldFeatureData(Coordinates position, FeatureType type, string details = "")
        {
            Position = position;
            Type = type;
            Details = details ?? string.Empty; // �����������, ��� details �� null
        }

        /// <summary>
        /// ���������� ��������� ������������� ������ �� ����������� ����.
        /// </summary>
        /// <returns>������ � �����, �������� � �������� (���� ����) �����������.</returns>
        public override string ToString()
        {
            return $"����������� ����: {Type} � {Position}{(string.IsNullOrEmpty(Details) ? "" : $" ({Details})")}";
        }
    }
}