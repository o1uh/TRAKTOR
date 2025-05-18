namespace Traktor.DataModels
{
    /// <summary>
    /// ������������ ������� ����, ������������ ������� ������ (���������).
    /// ��������������, ��� ������� ������ �������������.
    /// </summary>
    public class FieldBoundaries
    {
        /// <summary>
        /// ������ ������, ������������ ������� ����.
        /// </summary>
        public List<Coordinates> Vertices { get; private set; } 

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="FieldBoundaries"/>.
        /// </summary>
        /// <param name="vertices">������ ���������, �������������� ������� ������ ����.
        /// ���� �������� null, ����� ������ ������ ������.</param>
        public FieldBoundaries(List<Coordinates> vertices)
        {
            Vertices = vertices ?? new List<Coordinates>();
        }
        // ����� �������� ������, ���� �����������, ��������, ��� ��������, ��������� �� ����� ������ ������.
    }
}