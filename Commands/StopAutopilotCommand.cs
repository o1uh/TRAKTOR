using System; 
using Traktor.Core;       
using Traktor.Interfaces; 

namespace Traktor.Commands
{
    /// <summary>
    /// ������� ��� ��������� �������� ����������.
    /// </summary>
    public class StopAutopilotCommand : ICommand
    {
        private const string SourceFilePath = "Commands/StopAutopilotCommand.cs";
        private readonly IControlUnitCommands _receiver; // ���������� (Receiver)

        /// <summary>
        /// �������������� ����� ��������� ������� ��������� ����������.
        /// </summary>
        /// <param name="controlUnit">��������� ControlUnit, ������� ����� ��������� ��������.</param>
        public StopAutopilotCommand(IControlUnitCommands receiver)
        {
            _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
            Logger.Instance.Debug(SourceFilePath, "StopAutopilotCommand �������.");
        }

        /// <summary>
        /// ��������� ������� ��������� ����������.
        /// </summary>
        public void Execute()
        {
            Logger.Instance.Info(SourceFilePath, "���������� StopAutopilotCommand: ��������� ����������.");
            try
            {
                _receiver.StopOperation();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"������ ��� ���������� StopAutopilotCommand: {ex.Message}", ex);
            }
        }
    }
}