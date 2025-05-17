// Interfaces/IUserInterface.cs
namespace TractorAutopilot.Interfaces
{
    public interface IUserInterface
    {
        /// <summary>
        /// ���������� ������ �������.
        /// </summary>
        /// <param name="status">������ ��� �����������.</param>
        void DisplayStatus(string status);

        /// <summary>
        /// ���������� �������������� � ������.
        /// </summary>
        /// <param name="message">��������� ��� �����������.</param>
        void ShowAlerts(string message);

        /// <summary>
        /// ��������� ������������ ��������������� �������.
        /// </summary>
        void ConfigureSystem();
    }
}