namespace TractorAutopilot.DataModels
{
    /// <summary>
    /// ������������ ������������ �����������.
    /// </summary>
    public class Obstacle
    {
        /// <summary>
        /// �������� ��� ������ ������� �����������.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// �������� ��� ������ ������ �����������.
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// �������� ��� ������ ��� �����������.
        /// </summary>
        public string Type { get; set; } = "����������";

        /// <summary>
        /// ���������� ��������� ������������� �����������.
        /// </summary>
        /// <returns>������, ����������� �����������.</returns>
        public override string ToString() => $"����������� ���� '{Type}' � {Position} �������� {Size}�.";
    }
}