namespace Traktor.Interfaces
{
    /// <summary>
    /// ��������� �������� (Product) ��� �������� ��������� �����.
    /// ������������ ��������� ����� � ��������� ����.
    /// </summary>
    public interface IFieldReportProduct
    {
        /// <summary>
        /// "���������" ��� "����������" ���������� ������.
        /// </summary>
        void DisplayFormat();

        /// <summary>
        /// �������� ��� ������.
        /// </summary>
        string GetReportType();
    }
}