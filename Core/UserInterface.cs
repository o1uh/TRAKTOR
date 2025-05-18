using Traktor.DataModels;       // ��� Coordinates � ������ �������, ���� ����� �� ����������

namespace Traktor.Core
{
    public class UserInterface
    {
        public UserInterface()
        {
            Console.WriteLine("[UserInterface]: ���������� ��������� ������������ ���������������.");
            Console.Title = "������� ���������� ��������� (�����)";
        }

        /// <summary>
        /// ���������� ����� ��������� ��� ������������.
        /// </summary>
        public void DisplayMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.White; // ����������� ����
            Console.WriteLine($"[INFO] {message}");
        }

        /// <summary>
        /// ���������� ��������� � ��������� ��� ������� �������.
        /// </summary>
        public void DisplayStatus(string statusMessage)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[STATUS] {statusMessage}");
            Console.ResetColor();
        }

        /// <summary>
        /// ���������� ��������������.
        /// </summary>
        public void ShowWarning(string warningMessage)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[��������������] {warningMessage}");
            Console.ResetColor();
        }

        /// <summary>
        /// ���������� ��������� �� ������.
        /// </summary>
        public void ShowError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[������] {errorMessage}");
            Console.ResetColor();
        }

        /// <summary>
        /// ����������� ������� �� ���������.
        /// </summary>
        /// <returns>��������� ������������� ������ �������.</returns>
        public string GetOperatorCommand(string prompt = "������� ������� > ")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(prompt);
            string command = Console.ReadLine();
            Console.ResetColor();
            return command?.Trim().ToLower() ?? string.Empty; // �������� � ������� �������� � ������� �������
        }

        /// <summary>
        /// ���������� ������� ���������� ��������.
        /// </summary>
        public void DisplayTractorPosition(Coordinates position)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[������� ��������]: {position}");
            Console.ResetColor();
        }

        /// <summary>
        /// ���������� ���������� �� ������������ ������������.
        /// </summary>
        public void DisplayObstacles(List<ObstacleData> obstacles)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            if (obstacles == null || !obstacles.Any())
            {
                // Console.WriteLine("[�����������]: �� ����������."); // ����� �� ��������, ���� ���
                return;
            }
            Console.WriteLine("[���������� �����������]:");
            foreach (var obstacle in obstacles)
            {
                Console.WriteLine($"  - {obstacle}");
            }
            Console.ResetColor();
        }

        /// <summary>
        /// ���������� ���������� �� ������������ ���� (��������, ��������).
        /// </summary>
        public void DisplayFieldFeatures(List<FieldFeatureData> features)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (features == null || !features.Any())
            {
                // Console.WriteLine("[����������� ����]: �� ����������."); // ����� �� ��������, ���� ���
                return;
            }
            Console.WriteLine("[���������� ����������� ����]:");
            foreach (var feature in features)
            {
                Console.WriteLine($"  - {feature}");
            }
            Console.ResetColor();
        }

        /// <summary>
        /// ���������� ������� �������.
        /// </summary>
        public void DisplayRoute(List<Coordinates> route, int currentTargetIndex = -1)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (route == null || !route.Any())
            {
                Console.WriteLine("[�������]: �� ����� ��� ����.");
                Console.ResetColor();
                return;
            }
            Console.WriteLine("[������� �������]:");
            for (int i = 0; i < route.Count; i++)
            {
                string prefix = (i == currentTargetIndex) ? "  -> ����: " : "     �����: ";
                Console.WriteLine($"{prefix}{route[i]}");
            }
            Console.ResetColor();
        }

        /// <summary>
        /// ����������� � ������������ ������������� (��/���).
        /// </summary>
        public bool RequestConfirmation(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{message} (��/���): ");
            string input = Console.ReadLine()?.Trim().ToLower();
            Console.ResetColor();
            return input == "��" || input == "�" || input == "y" || input == "yes";
        }

        /// <summary>
        /// ���������� ���� ��������� ������ (������).
        /// </summary>
        public void ShowAvailableCommands()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n��������� ������� (������):");
            Console.WriteLine("  start gps <lat_����> <lon_����> - ��������� GPS ��������� � ���������� �������");
            Console.WriteLine("  start ins <lat_����> <lon_����> - ��������� ��� ��������� � ���������� �������");
            Console.WriteLine("  stop nav                             - ���������� ������� ������������� �������");
            Console.WriteLine("  status                               - �������� ������� ������");
            Console.WriteLine("  move                                 - ������� ��� �������� �������� (���� ���� �������)");
            Console.WriteLine("  scan obstacles                       - ����������� �� ������� �����������");
            Console.WriteLine("  scan features                        - ����������� �� ������� ������������ ����");
            Console.WriteLine("  attach <����|������|�������������>   - ���������� ������������");
            Console.WriteLine("  activate imp                         - ������������ ������������");
            Console.WriteLine("  deactivate imp                       - �������������� ������������");
            Console.WriteLine("  set depth <��������>                 - ���������� ������� �����");
            Console.WriteLine("  set rate <��������>                  - ���������� ����� ������");
            Console.WriteLine("  set intensity <��������>             - ���������� ������������� ������������");
            Console.WriteLine("  exit                                 - �����");
            Console.ResetColor();
        }
    }
}