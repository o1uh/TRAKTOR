using System; 
using Traktor.Interfaces; 
using Traktor.Core;       
using Traktor.DataModels; 
using Traktor.Implements; 

namespace Traktor.Commands
{
    /// <summary>
    /// Конкретный обработчик для команды "start".
    /// </summary>
    public class StartCommandHandler : CommandHandlerBase
    {
        private const string SourceFilePath = "Commands/StartCommandHandler.cs";
        private const string CommandKeyword = "start";

        public override ICommand HandleRequest(string userInput, IControlUnitCommands receiver)
        {
            Logger.Instance.Debug(SourceFilePath, $"Попытка обработать ввод: '{userInput}'.");
            string[] parts = userInput.Trim().ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 0 && parts[0] == CommandKeyword)
            {
                Logger.Instance.Info(SourceFilePath, $"Распознана команда '{CommandKeyword}'. Парсинг аргументов...");
                if (parts.Length >= 3)
                {
                    if (double.TryParse(parts[1].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lat) &&
                        double.TryParse(parts[2].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lon))
                    {
                        Coordinates target = new Coordinates(lat, lon);
                        ImplementType implement = ImplementType.None;
                        FieldBoundaries boundaries = null; 

                        if (parts.Length > 3)
                        {
                            if (!Enum.TryParse<ImplementType>(parts[3], true, out implement))
                            {
                                Logger.Instance.Warning(SourceFilePath, $"Не удалось распознать тип оборудования: '{parts[3]}'. Используется ImplementType.None.");
                                implement = ImplementType.None; 
                            }
                        }
                        Logger.Instance.Info(SourceFilePath, $"Аргументы для StartAutopilotCommand успешно разобраны. Создание команды.");
                        return new StartAutopilotCommand(receiver, target, implement, boundaries);
                    }
                    else
                    {
                        Logger.Instance.Warning(SourceFilePath, $"Неверный формат координат для команды '{CommandKeyword}': '{parts[1]}', '{parts[2]}'.");
                    }
                }
                else
                {
                    Logger.Instance.Warning(SourceFilePath, $"Недостаточно аргументов для команды '{CommandKeyword}'. Ожидается: {CommandKeyword} <lat> <lon> [implement_type]");
                }
                return null; // Ошибка парсинга аргументов, но команду распознали, поэтому не передаем дальше
            }

            // Если не команда "start", передаем следующему
            return PassToNext(userInput, receiver, SourceFilePath);
        }
    }
}