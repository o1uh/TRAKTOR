// ����: Core/UserInterface.cs
using System;
using System.Collections.Generic;
using System.Drawing; // ��� Bitmap
using System.Linq;    // ��� Any()
using Traktor.DataModels;
using Traktor.Implements;
using Traktor.Core; // ��� Logger � ControlUnit (���� �� ����� �����)

namespace Traktor.Core
{
    /// <summary>
    /// ������������ ���������� ���������������� ��������� ��� �������������� � �������� ����������.
    /// ���������� ��������� ������� � ��������� ������������ ���������� ������� �������.
    /// </summary>
    public class UserInterface
    {
        private readonly ControlUnit _controlUnit;
        private const string SourceFilePath = "Core/UserInterface.cs";
        private bool _keepRunning = true;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="UserInterface"/>.
        /// </summary>
        /// <param name="controlUnit">����������� ����������� ����, � ������� ����� ����������������� ���������.</param>
        /// <exception cref="ArgumentNullException">���� controlUnit ����� null.</exception>
        public UserInterface(ControlUnit controlUnit)
        {
            _controlUnit = controlUnit ?? throw new ArgumentNullException(nameof(controlUnit));
            Logger.Instance.Info(SourceFilePath, "UserInterface ���������������.");
        }

        /// <summary>
        /// ��������� �������� ���� ������ ����������������� ����������.
        /// ���������� ������ � ������� ������� ������������.
        /// </summary>
        public void Run()
        {
            Logger.Instance.Info(SourceFilePath, "������ ��������� ����� UserInterface.");
            DisplayHelp();

            while (_keepRunning)
            {
                DisplayStatus(); // ���������� ������� ������
                Console.Write("������� �������: ");
                string input = Console.ReadLine()?.Trim().ToLower();
                ProcessInput(input);

                if (_controlUnit.IsOperating && _keepRunning) // ���� ��������� ��������, ������ ��� ���������
                {
                    _controlUnit.SimulateOneStep();
                    System.Threading.Thread.Sleep(1000); // ��������� �������� ��� ����������� ���������
                }
                else if (!_controlUnit.IsOperating && _keepRunning)
                {
                    System.Threading.Thread.Sleep(200); // ������� �������� � ������ �������� ������
                }
            }
            Logger.Instance.Info(SourceFilePath, "UserInterface ��������� ������.");
        }

        /// <summary>
        /// ���������� ������� ��������� �������.
        /// </summary>
        private void DisplayStatus()
        {
            Console.Clear(); // ������� ������� ��� ���������� �������
            Console.WriteLine("--- ������ ������� ���������� �������� ---");
            Console.WriteLine($"��������� ����������: {(_controlUnit.IsOperating ? "��������" : "����������")}");

            Coordinates currentPosition = _controlUnit.GetCurrentPosition();
            Console.WriteLine($"������� �������: {currentPosition}");

            if (_controlUnit.IsOperating)
            {
                Console.WriteLine($"������� ���� �� ��������: {_controlUnit.GetCurrentTargetPoint()}");
                List<Coordinates> route = _controlUnit.GetCurrentRoute();
                if (route != null && route.Any())
                {
                    Console.WriteLine($"����� � ��������: {route.Count}. �������: [{string.Join(" -> ", route.Take(5).Select(c => $"({c.Latitude:F3},{c.Longitude:F3})"))}{(route.Count > 5 ? "..." : "")}]");
                }
                else
                {
                    Console.WriteLine("�������: �� ����� ��� ����.");
                }
            }

            Console.WriteLine("\n--- ������ �������� ---");
            Console.WriteLine($"������ ���������� (��������): {_controlUnit.GetCurrentDistanceValue():F2} �");
            Console.WriteLine($"������ �����: {_controlUnit.GetCurrentSoilData()}");

            // ����������� ����� � ������ ����� ���� ������� � �������.
            // ���� ������ ������� ���������� � ���, ��� ���� �������.
            using (Bitmap frame = _controlUnit.GetCurrentCameraFrame()) // ����� ������������� Bitmap
            {
                if (frame != null)
                {
                    Console.WriteLine($"������: ������� ���� {frame.Width}x{frame.Height} (� ������� �� ������������)");
                }
                else
                {
                    Console.WriteLine("������: �� ������� �������� ����.");
                }
            }


            Console.WriteLine("\n--- �������� ������������ ---");
            ImplementControlSystem implementCtrl = _controlUnit.ImplementControl;
            Console.WriteLine($"����������: {implementCtrl.GetAttachedImplementType()}");
            Console.WriteLine($"��������� ��������: {(implementCtrl.IsOperationActive() ? "�������" : "�� �������")}");
            if (implementCtrl.GetAttachedImplementType() == ImplementType.Plough)
                Console.WriteLine($"  ������� �������: {implementCtrl.GetCurrentPloughDepth():F2} �");
            else if (implementCtrl.GetAttachedImplementType() == ImplementType.Seeder)
                Console.WriteLine($"  ����� ������: {implementCtrl.GetCurrentSeedingRate():F2}");
            else if (implementCtrl.GetAttachedImplementType() == ImplementType.Sprayer)
                Console.WriteLine($"  ������������� ������������: {implementCtrl.GetCurrentSprayerIntensity():F1}%");

            Console.WriteLine("\n--------------------------------------------");
        }

        /// <summary>
        /// ������������ ���� ������������.
        /// </summary>
        /// <param name="input">�������, ��������� �������������.</param>
        private void ProcessInput(string input)
        {
            Logger.Instance.Debug(SourceFilePath, $"�������� ������� �� ������������: '{input}'");
            if (string.IsNullOrWhiteSpace(input)) return;

            string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0];

            try
            {
                switch (command)
                {
                    case "start": // ������: start <lat> <lon> [plough|seeder|sprayer]
                        HandleStartCommand(parts);
                        break;
                    case "stop":
                        _controlUnit.StopOperation();
                        Logger.Instance.Info(SourceFilePath, "������� 'stop' ���������.");
                        break;
                    case "status": // �������������� ���������� ������� (���� �� � ��� �����������)
                        // DisplayStatus() ���������� � �����, ��� ��� ��������� ������� �� ������ �����
                        Logger.Instance.Info(SourceFilePath, "������� 'status' (���������� �� ���������, ���������� �������������).");
                        break;
                    case "implement": // ������: implement plough depth 0.2 | implement seeder rate 50 | implement sprayer intensity 75
                        HandleImplementCommand(parts);
                        break;
                    case "exit":
                        _keepRunning = false;
                        Logger.Instance.Info(SourceFilePath, "������� 'exit'. ���������� ������ ����������.");
                        break;
                    case "help":
                        DisplayHelp();
                        break;
                    default:
                        Console.WriteLine("����������� �������. ������� 'help' ��� ������ ������.");
                        Logger.Instance.Warning(SourceFilePath, $"����������� �������: '{input}'");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������ ��� ���������� �������: {ex.Message}");
                Logger.Instance.Error(SourceFilePath, $"������ ��� ���������� ������� '{input}': {ex.Message}", ex);
            }
        }

        private void HandleStartCommand(string[] parts)
        {
            if (_controlUnit.IsOperating)
            {
                Console.WriteLine("��������� ��� ��������. ������� ���������� ('stop').");
                return;
            }
            if (parts.Length < 3)
            {
                Console.WriteLine("�������������: start <�������_������> <�������_�������> [���_������������ (plough/seeder/sprayer)]");
                return;
            }
            if (!double.TryParse(parts[1].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lat) ||
                !double.TryParse(parts[2].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lon))
            {
                Console.WriteLine("������: �������� ������ ���������.");
                return;
            }
            Coordinates target = new Coordinates(lat, lon);
            ImplementType implement = ImplementType.None;
            if (parts.Length > 3)
            {
                if (!Enum.TryParse<ImplementType>(parts[3], true, out implement))
                {
                    Console.WriteLine($"������: ����������� ��� ������������ '{parts[3]}'. ��������: Plough, Seeder, Sprayer.");
                    return;
                }
            }
            // ��� �������, �������� ��������� ������� �������� (����� ������� ���������������)
            _controlUnit.GetCurrentPosition(); // ����� "��������" ���������, ���� ��� �������.
            _controlUnit.StartOperation(target, null, implement); // Boundaries ���� null
        }

        private void HandleImplementCommand(string[] parts)
        {
            // implement plough depth 0.2
            // implement seeder rate 50
            // implement sprayer intensity 75
            // implement attach plough
            // implement activate / deactivate
            if (parts.Length < 2)
            {
                Console.WriteLine("�������������: implement <���> <��������> <��������> | implement attach <���> | implement <activate|deactivate>");
                return;
            }

            ImplementControlSystem implementCtrl = _controlUnit.ImplementControl;

            switch (parts[1].ToLower())
            {
                case "attach":
                    if (parts.Length < 3 || !Enum.TryParse<ImplementType>(parts[2], true, out ImplementType typeToAttach))
                    { Console.WriteLine("�������������: implement attach <plough|seeder|sprayer>"); return; }
                    implementCtrl.AttachImplement(typeToAttach);
                    break;
                case "activate":
                    implementCtrl.ActivateOperation();
                    break;
                case "deactivate":
                    implementCtrl.DeactivateOperation();
                    break;
                default: // ������������, ��� ��� ��������� ���������
                    if (parts.Length < 4) { Console.WriteLine("�������������: implement <���_������������> <��������> <��������>"); return; }
                    if (!Enum.TryParse<ImplementType>(parts[1], true, out ImplementType targetImplement))
                    { Console.WriteLine($"����������� ��� ������������: {parts[1]}"); return; }

                    string paramName = parts[2].ToLower();
                    if (!double.TryParse(parts[3].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double paramValue))
                    { Console.WriteLine($"�������� �������� ���������: {parts[3]}"); return; }

                    if (implementCtrl.GetAttachedImplementType() != targetImplement)
                    {
                        Console.WriteLine($"������� ���������� ������������ '{targetImplement}' �������� 'implement attach {targetImplement.ToString().ToLower()}'");
                        return;
                    }

                    switch (targetImplement)
                    {
                        case ImplementType.Plough:
                            if (paramName == "depth") implementCtrl.SetPloughDepth(paramValue);
                            else Console.WriteLine($"����������� �������� '{paramName}' ��� �����.");
                            break;
                        case ImplementType.Seeder:
                            if (paramName == "rate") implementCtrl.SetSeederRate(paramValue);
                            else Console.WriteLine($"����������� �������� '{paramName}' ��� ������.");
                            break;
                        case ImplementType.Sprayer:
                            if (paramName == "intensity") implementCtrl.SetSprayerIntensity(paramValue);
                            else Console.WriteLine($"����������� �������� '{paramName}' ��� �������������.");
                            break;
                        default:
                            Console.WriteLine($"��������� ���������� ��� '{targetImplement}' �� �������������� ��� ������������ �� ����������.");
                            break;
                    }
                    break;
            }
        }


        /// <summary>
        /// ���������� ���������� ���������� �� ��������� ��������.
        /// </summary>
        private void DisplayHelp()
        {
            Console.WriteLine("\n��������� �������:");
            Console.WriteLine("  start <������> <�������> [���_������] - ��������� ��������� � ����. ������������: plough, seeder, sprayer (�����������).");
            Console.WriteLine("                                      ������: start 55.123 37.456 plough");
            Console.WriteLine("  stop                               - ���������� ���������.");
            Console.WriteLine("  implement attach <���>             - ���������� ������������ (plough, seeder, sprayer).");
            Console.WriteLine("                                      ������: implement attach plough");
            Console.WriteLine("  implement <���> depth <��������>   - ���������� ������� ����� (���� ���� ���������). ������: implement plough depth 0.2");
            Console.WriteLine("  implement <���> rate <��������>    - ���������� ����� ������ ������. ������: implement seeder rate 50");
            Console.WriteLine("  implement <���> intensity <����>   - ���������� ������������� �������������. ������: implement sprayer intensity 75");
            Console.WriteLine("  implement activate                 - ������������ �������� ������������� ������������.");
            Console.WriteLine("  implement deactivate               - �������������� �������� ������������.");
            Console.WriteLine("  status                             - (����������� �������������) �������� ������� ������.");
            Console.WriteLine("  help                               - �������� ��� �������.");
            Console.WriteLine("  exit                               - ����� �� ����������.");
            Console.WriteLine();
        }
    }
}