// Interfaces/IUserInterface.cs
namespace TractorAutopilot.Interfaces
{
    public interface IUserInterface
    {
        /// <summary>
        /// Отображает статус системы.
        /// </summary>
        /// <param name="status">Статус для отображения.</param>
        void DisplayStatus(string status);

        /// <summary>
        /// Отображает предупреждения и ошибки.
        /// </summary>
        /// <param name="message">Сообщение для отображения.</param>
        void ShowAlerts(string message);

        /// <summary>
        /// Позволяет пользователю конфигурировать систему.
        /// </summary>
        void ConfigureSystem();
    }
}