using System; 
using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.Commands
{
    /// <summary>
    /// Конкретный обработчик для команды "stop".
    /// </summary>
    public class StopCommandHandler : CommandHandlerBase
    {
        private const string SourceFilePath = "Commands/StopCommandHandler.cs";
        private const string CommandKeyword = "stop";

        public override ICommand HandleRequest(string userInput, IControlUnitCommands receiver)
        {
            Logger.Instance.Debug(SourceFilePath, $"Попытка обработать ввод: '{userInput}'.");
            string[] parts = userInput.Trim().ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 0 && parts[0] == CommandKeyword)
            {
                Logger.Instance.Info(SourceFilePath, $"Распознана команда '{CommandKeyword}'. Создание команды StopAutopilotCommand.");
                return new StopAutopilotCommand(receiver);
            }

            // Если не команда "stop", передаем следующему
            return PassToNext(userInput, receiver, SourceFilePath);
        }
    }
}