namespace Traktor.Interfaces
{
    /// <summary>
    /// ���������� �������� ��� �������������� � ���������.
    /// ������� ����� ���������� ��������� ���� ������.
    /// </summary>
    /// <typeparam name="T">��� ������, ������������ ��������.</typeparam>
    public interface ISensors<T>
    {
        /// <summary>
        /// �������� ������ � �������.
        /// </summary>
        /// <returns>������ ������� ���������� ���� T.</returns>
        T GetData();
    }
}