namespace TractorAutopilot.DataModels
{
    /// <summary>
    /// ������������ ��������� ������ ��� ����������.
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// ���������� X.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// ���������� Y.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// �������������� ����� ��������� ��������� <see cref="Vector2"/>.
        /// </summary>
        /// <param name="x">���������� X.</param>
        /// <param name="y">���������� Y.</param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// ���������� ��������� ������������� �������.
        /// </summary>
        /// <returns>������, �������������� ������.</returns>
        public override string ToString() => $"({X}, {Y})";
    }
}