using System.Drawing; 
using Traktor.DataModels;
using Traktor.Implements;

namespace Traktor.Core
{
    /// <summary>
    /// Представляет консольный пользовательский интерфейс для взаимодействия с системой автопилота.
    /// Отображает состояние системы и позволяет пользователю отправлять базовые команды.
    /// </summary>
    public class UserInterface
    {
        private readonly ControlUnit _controlUnit;
        private const string SourceFilePath = "Core/UserInterface.cs";
        private bool _keepRunning = true;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserInterface"/>.
        /// </summary>
        /// <param name="controlUnit">Центральный управляющий блок, с которым будет взаимодействовать интерфейс.</param>
        /// <exception cref="ArgumentNullException">Если controlUnit равен null.</exception>
        public UserInterface(ControlUnit controlUnit)
        {
            _controlUnit = controlUnit ?? throw new ArgumentNullException(nameof(controlUnit));
            Logger.Instance.Info(SourceFilePath, "UserInterface инициализирован.");
        }

        /// <summary>
        /// Запускает основной цикл работы пользовательского интерфейса.
        /// Отображает статус и ожидает команды пользователя.
        /// </summary>
        public void Run()
        {
            Logger.Instance.Info(SourceFilePath, "Запуск основного цикла UserInterface.");
            Console.Clear();
            DisplayHelp();
            Console.WriteLine("\nНажмите любую клавишу для первого отображения статуса...");
            Console.ReadKey(true);

            while (_keepRunning)
            {
                DisplayStatus(); // Всегда отображаем актуальный статус в начале итерации

                Console.Write("Введите команду (help для справки): ");
                string input = Console.ReadLine()?.Trim().ToLower();

                bool wasHelpCommand = (input == "help");
                bool commandProcessedRequiresPause = false; // Флаг, что после ProcessInput нужна пауза

                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (wasHelpCommand) // Для help всегда очищаем перед выводом справки
                    {
                        Console.Clear();
                    }
                    
                    else if (!_controlUnit.IsOperating)
                    {
                        Console.Clear();
                    }

                    ProcessInput(input);

                    if (wasHelpCommand || (!_controlUnit.IsOperating && !string.IsNullOrWhiteSpace(input) && input != "exit"))
                    {
                        commandProcessedRequiresPause = true;
                    }
                }
                else if (!_controlUnit.IsOperating)
                {
                    System.Threading.Thread.Sleep(100);
                    continue; 
                }


                if (_controlUnit.IsOperating && _keepRunning)
                {
                    _controlUnit.SimulateOneStep();
                    System.Threading.Thread.Sleep(1000);
                }

                if (commandProcessedRequiresPause && _keepRunning)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey(true);
                }
            }
            Logger.Instance.Info(SourceFilePath, "UserInterface завершает работу.");
        }


        /// <summary>
        /// Отображает текущее состояние системы.
        /// </summary>
        private void DisplayStatus()
        {
            Console.Clear();

            Console.WriteLine("--- Статус системы автопилота трактора ---");
            Console.WriteLine($"Состояние автопилота: {(_controlUnit.IsOperating ? "РАБОТАЕТ" : "ОСТАНОВЛЕН")}");

            Coordinates currentPosition = _controlUnit.GetCurrentPosition();
            Console.WriteLine($"Текущая позиция: {currentPosition}");

            if (_controlUnit.IsOperating)
            {
                Console.WriteLine($"Текущая цель на маршруте: {_controlUnit.GetCurrentTargetPoint()}");
                List<Coordinates> route = _controlUnit.GetCurrentRoute();
                if (route != null && route.Any())
                {
                    Console.WriteLine($"Точек в маршруте: {route.Count}. Маршрут: [{string.Join(" -> ", route.Take(5).Select(c => $"({c.Latitude:F3},{c.Longitude:F3})"))}{(route.Count > 5 ? "..." : "")}]");
                }
                else
                {
                    Console.WriteLine("Маршрут: не задан или пуст.");
                }
            }

            Console.WriteLine("\n--- Данные сенсоров ---");
            try
            {
                Console.WriteLine($"Датчик расстояния (передний): {_controlUnit.GetCurrentDistanceValue():F2} м");
                Console.WriteLine($"Датчик почвы: {_controlUnit.GetCurrentSoilData()}");

                using (Bitmap frame = _controlUnit.GetCurrentCameraFrame())
                {
                    if (frame != null)
                    {
                        Console.WriteLine($"Камера: получен кадр {frame.Width}x{frame.Height} (в консоли не отображается)");
                    }
                    else
                    {
                        Console.WriteLine("Камера: не удалось получить кадр.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Ошибка при получении данных сенсоров для отображения: {ex.Message}", ex);
                Console.WriteLine("Ошибка при получении данных с одного из сенсоров.");
            }


            Console.WriteLine("\n--- Навесное оборудование ---");
            ImplementControlSystem implementCtrl = _controlUnit.ImplementControl;
            Console.WriteLine($"Подключено: {implementCtrl.GetAttachedImplementType()}");
            Console.WriteLine($"Состояние операции: {(implementCtrl.IsOperationActive() ? "АКТИВНА" : "НЕ АКТИВНА")}");
            if (implementCtrl.GetAttachedImplementType() == ImplementType.Plough)
                Console.WriteLine($"  Глубина вспашки: {implementCtrl.GetCurrentPloughDepth():F2} м");
            else if (implementCtrl.GetAttachedImplementType() == ImplementType.Seeder)
                Console.WriteLine($"  Норма высева: {implementCtrl.GetCurrentSeedingRate():F2}");
            else if (implementCtrl.GetAttachedImplementType() == ImplementType.Sprayer)
                Console.WriteLine($"  Интенсивность опрыскивания: {implementCtrl.GetCurrentSprayerIntensity():F1}%");

            Console.WriteLine("\n--------------------------------------------");
        }

        /// <summary>
        /// Обрабатывает ввод пользователя.
        /// </summary>
        /// <param name="input">Команда, введенная пользователем.</param>
        private void ProcessInput(string input)
        {
            Logger.Instance.Debug(SourceFilePath, $"Получена команда от пользователя: '{input}'");
            if (string.IsNullOrWhiteSpace(input)) return;

            string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0];

            try
            {
                switch (command)
                {
                    case "start":
                        HandleStartCommand(parts);
                        break;
                    case "stop":
                        _controlUnit.StopOperation();
                        Logger.Instance.Info(SourceFilePath, "Команда 'stop' выполнена.");
                        Console.WriteLine("Автопилот остановлен.");
                        break;
                    case "status":
                        Logger.Instance.Info(SourceFilePath, "Команда 'status' (обновление произойдет на следующей итерации цикла).");
                        Console.WriteLine("Статус будет обновлен на следующей итерации...");
                        break;
                    case "implement":
                        HandleImplementCommand(parts);
                        break;
                    case "exit":
                        _keepRunning = false;
                        Logger.Instance.Info(SourceFilePath, "Команда 'exit'. Завершение работы интерфейса.");
                        Console.WriteLine("Завершение работы...");
                        break;
                    case "help":
                        DisplayHelp();
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                        Logger.Instance.Warning(SourceFilePath, $"Неизвестная команда: '{input}'");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении команды: {ex.Message}");
                Logger.Instance.Error(SourceFilePath, $"Ошибка при выполнении команды '{input}': {ex.Message}", ex);
            }
        }

        private void HandleStartCommand(string[] parts)
        {
            if (_controlUnit.IsOperating)
            {
                Console.WriteLine("Автопилот уже работает. Сначала остановите ('stop').");
                return;
            }
            if (parts.Length < 3)
            {
                Console.WriteLine("Использование: start <целевая_широта> <целевая_долгота> [тип_оборудования (plough/seeder/sprayer)]");
                return;
            }
            if (!double.TryParse(parts[1].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lat) ||
                !double.TryParse(parts[2].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lon))
            {
                Console.WriteLine("Ошибка: неверный формат координат.");
                return;
            }
            Coordinates target = new Coordinates(lat, lon);
            ImplementType implement = ImplementType.None;
            if (parts.Length > 3)
            {
                if (!Enum.TryParse<ImplementType>(parts[3], true, out implement))
                {
                    Console.WriteLine($"Ошибка: неизвестный тип оборудования '{parts[3]}'. Доступно: Plough, Seeder, Sprayer.");
                    return;
                }
            }

            Console.WriteLine($"Запуск автопилота к цели {target} с оборудованием {implement}...");
            _controlUnit.StartOperation(target, null, implement);
            if (_controlUnit.IsOperating) Console.WriteLine("Автопилот запущен.");
            else Console.WriteLine("Не удалось запустить автопилот (см. логи).");
        }

        private void HandleImplementCommand(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Использование: implement <тип> <параметр> <значение> | implement attach <тип> | implement <activate|deactivate>");
                return;
            }

            ImplementControlSystem implementCtrl = _controlUnit.ImplementControl;
            string actionOrType = parts[1].ToLower();

            switch (actionOrType)
            {
                case "attach":
                    if (parts.Length < 3 || !Enum.TryParse<ImplementType>(parts[2], true, out ImplementType typeToAttach))
                    { Console.WriteLine("Использование: implement attach <plough|seeder|sprayer>"); return; }
                    implementCtrl.AttachImplement(typeToAttach);
                    Console.WriteLine($"Оборудование '{typeToAttach}' подключено.");
                    break;
                case "activate":
                    implementCtrl.ActivateOperation();
                    Console.WriteLine($"Попытка активации операции с оборудованием '{implementCtrl.GetAttachedImplementType()}'.");
                    break;
                case "deactivate":
                    implementCtrl.DeactivateOperation();
                    Console.WriteLine($"Попытка деактивации операции с оборудованием '{implementCtrl.GetAttachedImplementType()}'.");
                    break;
                default:
                    if (parts.Length < 4) { Console.WriteLine("Использование для установки параметра: implement <тип_оборудования> <параметр> <значение>"); return; }

                    if (!Enum.TryParse<ImplementType>(actionOrType, true, out ImplementType targetImplement))
                    { Console.WriteLine($"Неизвестный тип оборудования: {actionOrType}"); return; }

                    string paramName = parts[2].ToLower();
                    if (!double.TryParse(parts[3].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double paramValue))
                    { Console.WriteLine($"Неверное значение параметра: {parts[3]}"); return; }

                    if (implementCtrl.GetAttachedImplementType() != targetImplement)
                    {
                        Console.WriteLine($"Сначала подключите оборудование '{targetImplement}' командой 'implement attach {targetImplement.ToString().ToLower()}'");
                        return;
                    }
                    bool paramSetSuccess = false;
                    switch (targetImplement)
                    {
                        case ImplementType.Plough:
                            if (paramName == "depth") { implementCtrl.SetPloughDepth(paramValue); paramSetSuccess = true; }
                            else Console.WriteLine($"Неизвестный параметр '{paramName}' для плуга.");
                            break;
                        case ImplementType.Seeder:
                            if (paramName == "rate") { implementCtrl.SetSeederRate(paramValue); paramSetSuccess = true; }
                            else Console.WriteLine($"Неизвестный параметр '{paramName}' для сеялки.");
                            break;
                        case ImplementType.Sprayer:
                            if (paramName == "intensity") { implementCtrl.SetSprayerIntensity(paramValue); paramSetSuccess = true; }
                            else Console.WriteLine($"Неизвестный параметр '{paramName}' для опрыскивателя.");
                            break;
                        default:
                            Console.WriteLine($"Установка параметров для '{targetImplement}' не поддерживается.");
                            break;
                    }
                    if (paramSetSuccess) Console.WriteLine($"Параметр '{paramName}' для '{targetImplement}' установлен на '{paramValue}'.");
                    break;
            }
        }

        /// <summary>
        /// Отображает справочную информацию по доступным командам.
        /// </summary>
        private void DisplayHelp()
        {
            Console.WriteLine("\nДоступные команды:");
            Console.WriteLine("  start <широта> <долгота> [тип_оборуд] - Запустить автопилот к цели. Оборудование: plough, seeder, sprayer (опционально).");
            Console.WriteLine("                                      Пример: start 55.123 37.456 plough");
            Console.WriteLine("  stop                               - Остановить автопилот.");
            Console.WriteLine("  implement attach <тип>             - Подключить оборудование (plough, seeder, sprayer).");
            Console.WriteLine("                                      Пример: implement attach plough");
            Console.WriteLine("  implement <тип> <параметр> <знач>  - Установить параметр для подключенного оборудования типа <тип>.");
            Console.WriteLine("                                      Параметры: plough depth <знач>, seeder rate <знач>, sprayer intensity <знач>");
            Console.WriteLine("                                      Пример: implement plough depth 0.2");
            Console.WriteLine("  implement activate                 - Активировать операцию подключенного оборудования.");
            Console.WriteLine("  implement deactivate               - Деактивировать операцию оборудования.");
            Console.WriteLine("  status                             - (Обновляется автоматически) Показать текущий статус.");
            Console.WriteLine("  help                               - Показать эту справку.");
            Console.WriteLine("  exit                               - Выйти из приложения.");
            Console.WriteLine();
        }
    }
}