using Traktor.Interfaces; 

namespace Traktor.Core
{
    /// <summary>
    /// ������� ��������� (Invoker) ������.
    /// ������ � ��������� ���������� ��� �������.
    /// � ����� ������� ���������� ��� �� ����� ������� ������, ������� � �.�.
    /// </summary>
    public class CommandInvoker
    {
        private const string SourceFilePath = "Commands/CommandInvoker.cs"; // ��������, ���� ����� ������
        private ICommand _command;

        public CommandInvoker()
        {
            Logger.Instance.Debug(SourceFilePath, "CommandInvoker ������.");
        }

        /// <summary>
        /// ������������� �������, ������� ����� ���������.
        /// </summary>
        /// <param name="command">������� ��� ����������.</param>
        public void SetCommand(ICommand command)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            Logger.Instance.Info(SourceFilePath, $"CommandInvoker: ����������� ������� ���� '{_command.GetType().Name}'.");
        }

        /// <summary>
        /// ��������� ������������� �������.
        /// </summary>
        public void ExecuteCommand()
        {
            if (_command != null)
            {
                Logger.Instance.Info(SourceFilePath, $"CommandInvoker: ���������� ������� '{_command.GetType().Name}'...");
                try
                {
                    _command.Execute();
                    Logger.Instance.Info(SourceFilePath, $"CommandInvoker: ������� '{_command.GetType().Name}' ������� ���������.");
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(SourceFilePath, $"CommandInvoker: ������ ��� ���������� ������� '{_command.GetType().Name}': {ex.Message}", ex);
                    // � ����������� �� ����������, ����� ������, ������ �� Invoker ������������ ���������� ��� ������������ ��
                }
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "CommandInvoker: ������� ��������� �������, �� ������� �� �����������.");
            }
        }
    }
}