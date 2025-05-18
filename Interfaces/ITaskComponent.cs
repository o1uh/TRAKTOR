namespace Traktor.Interfaces 
{
    /// <summary>
    /// ����� ��������� ��� ����������� ������ (��� ��� �������, ��� � ��� ���������).
    /// ���������� ��������, ����� ��� ���� �����������.
    /// </summary>
    public interface ITaskComponent
    {
        /// <summary>
        /// "���������" ������. � ������ ��� ����� �����������.
        /// </summary>
        /// <param name="indent">������ ������� ��� ������������ �������� � �����.</param>
        void Execute(string indent = "");

        /// <summary>
        /// ���������� ��� ������.
        /// </summary>
        /// <returns>��� ������.</returns>
        string GetName();
    }
}