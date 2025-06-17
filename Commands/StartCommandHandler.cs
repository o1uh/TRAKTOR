using System; 
using Traktor.Interfaces; 
using Traktor.Core;       
using Traktor.DataModels; 
using Traktor.Implements; 

namespace Traktor.Commands
{
    /// <summary>
    /// ���������� ���������� ��� ������� "start".
    /// </summary>
    public class StartCommandHandler : CommandHandlerBase
    {
        private const string SourceFilePath = "Commands/StartCommandHandler.cs";
        private const string CommandKeyword = "start";

        public override ICommand HandleRequest(string userInput, IControlUnitCommands receiver)
        {
            Logger.Instance.Debug(SourceFilePath, $"������� ���������� ����: '{userInput}'.");
            string[] parts = userInput.Trim().ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 0 && parts[0] == CommandKeyword)
            {
                Logger.Instance.Info(SourceFilePath, $"���������� ������� '{CommandKeyword}'. ������� ����������...");
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
                                Logger.Instance.Warning(SourceFilePath, $"�� ������� ���������� ��� ������������: '{parts[3]}'. ������������ ImplementType.None.");
                                implement = ImplementType.None; 
                            }
                        }
                        Logger.Instance.Info(SourceFilePath, $"��������� ��� StartAutopilotCommand ������� ���������. �������� �������.");
                        return new StartAutopilotCommand(receiver, target, implement, boundaries);
                    }
                    else
                    {
                        Logger.Instance.Warning(SourceFilePath, $"�������� ������ ��������� ��� ������� '{CommandKeyword}': '{parts[1]}', '{parts[2]}'.");
                    }
                }
                else
                {
                    Logger.Instance.Warning(SourceFilePath, $"������������ ���������� ��� ������� '{CommandKeyword}'. ���������: {CommandKeyword} <lat> <lon> [implement_type]");
                }
                return null; // ������ �������� ����������, �� ������� ����������, ������� �� �������� ������
            }

            // ���� �� ������� "start", �������� ����������
            return PassToNext(userInput, receiver, SourceFilePath);
        }
    }
}