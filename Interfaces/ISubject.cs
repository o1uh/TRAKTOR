namespace Traktor.Interfaces
{
    /// <summary>
    /// ��������� ��� �������� (Subject/Publisher).
    /// ���������� ������ ��� ���������� ������������ � �� �����������.
    /// </summary>
    public interface ISubject
    {
        /// <summary>
        /// ��������� ����������� (����������� �� ����������).
        /// </summary>
        /// <param name="observer">����������� ��� ����������.</param>
        void Attach(IObserver observer);

        /// <summary>
        /// ������� ����������� (���������� �� ����������).
        /// </summary>
        /// <param name="observer">����������� ��� ��������.</param>
        void Detach(IObserver observer);

        /// <summary>
        /// ���������� ���� ����������� ������������ �� ���������.
        /// </summary>
        /// <param name="message">��������� ��� ������ �� ���������.</param>
        void Notify(string message);
    }
}