using System; 
using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.Commands
{
    /// <summary>
    /// ���������� ���������� ��� ������� "stop".
    /// </summary>
    public class StopCommandHandler : CommandHandlerBase
    {
        private const string SourceFilePath = "Commands/StopCommandHandler.cs";
        private const string CommandKeyword = "stop";

        public override ICommand HandleRequest(string userInput, IControlUnitCommands receiver)
        {
            Logger.Instance.Debug(SourceFilePath, $"������� ���������� ����: '{userInput}'.");
            string[] parts = userInput.Trim().ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 0 && parts[0] == CommandKeyword)
            {
                Logger.Instance.Info(SourceFilePath, $"���������� ������� '{CommandKeyword}'. �������� ������� StopAutopilotCommand.");
                return new StopAutopilotCommand(receiver);
            }

            // ���� �� ������� "stop", �������� ����������
            return PassToNext(userInput, receiver, SourceFilePath);
        }
    }
}