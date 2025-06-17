namespace Traktor.Interfaces
{
    /// <summary>
    /// ��������� ��� ���������, ������� ����� ���� "��������" �����������.
    /// </summary>
    public interface IFieldElement
    {
        /// <summary>
        /// ��������� ����������.
        /// </summary>
        /// <param name="visitor">������ ����������.</param>
        void Accept(IVisitor visitor);

        /// <summary>
        /// ��������������� ����� ��� ��������� �������� �������� (��� �����������).
        /// </summary>
        string GetDescription();
    }
}