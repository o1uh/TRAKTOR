using System; 
using Traktor.Core;       
using Traktor.Interfaces; 

namespace Traktor.Commands
{
    /// <summary>
    /// Команда для остановки операции автопилота.
    /// </summary>
    public class StopAutopilotCommand : ICommand
    {
        private const string SourceFilePath = "Commands/StopAutopilotCommand.cs";
        private readonly IControlUnitCommands _receiver; // Получатель (Receiver)

        /// <summary>
        /// Инициализирует новый экземпляр команды остановки автопилота.
        /// </summary>
        /// <param name="controlUnit">Экземпляр ControlUnit, который будет выполнять действие.</param>
        public StopAutopilotCommand(IControlUnitCommands receiver)
        {
            _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
            Logger.Instance.Debug(SourceFilePath, "StopAutopilotCommand создана.");
        }

        /// <summary>
        /// Выполняет команду остановки автопилота.
        /// </summary>
        public void Execute()
        {
            Logger.Instance.Info(SourceFilePath, "Выполнение StopAutopilotCommand: Остановка автопилота.");
            try
            {
                _receiver.StopOperation();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Ошибка при выполнении StopAutopilotCommand: {ex.Message}", ex);
            }
        }
    }
}