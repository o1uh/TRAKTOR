// Файл: Core/UserInterface.cs
using System;
using System.Collections.Generic;
using System.Drawing; // Для Bitmap
using System.Linq;    // Для Any()
using Traktor.DataModels;
using Traktor.Implements;
using Traktor.Core; // Для Logger и ControlUnit (если он будет здесь)

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
            DisplayHelp();

            while (_keepRunning)
            {
                DisplayStatus(); // Отображаем текущий статус
                Console.Write("Введите команду: ");
                string input = Console.ReadLine()?.Trim().ToLower();
                ProcessInput(input);

                if (_controlUnit.IsOperating && _keepRunning) // Если автопилот работает, делаем шаг симуляции
                {
                    _controlUnit.SimulateOneStep();
                    System.Threading.Thread.Sleep(1000); // Небольшая задержка для наглядности симуляции
                }
                else if (!_controlUnit.IsOperating && _keepRunning)
                {
                    System.Threading.Thread.Sleep(200); // Меньшая задержка в режиме ожидания команд
                }
            }
            Logger.Instance.Info(SourceFilePath, "UserInterface завершает работу.");
        }

        /// <summary>
        /// Отображает текущее состояние системы.
        /// </summary>
        private void DisplayStatus()
        {
            Console.Clear(); // Очищаем консоль для обновления статуса
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
            Console.WriteLine($"Датчик расстояния (передний): {_controlUnit.GetCurrentDistanceValue():F2} м");
            Console.WriteLine($"Датчик почвы: {_controlUnit.GetCurrentSoilData()}");

            // Отображение кадра с камеры может быть сложным в консоли.
            // Пока просто выведем информацию о том, что кадр получен.
            using (Bitmap frame = _controlUnit.GetCurrentCameraFrame()) // Важно утилизировать Bitmap
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
                    case "start": // Пример: start <lat> <lon> [plough|seeder|sprayer]
                        HandleStartCommand(parts);
                        break;
                    case "stop":
                        _controlUnit.StopOperation();
                        Logger.Instance.Info(SourceFilePath, "Команда 'stop' выполнена.");
                        break;
                    case "status": // Принудительное обновление статуса (хотя он и так обновляется)
                        // DisplayStatus() вызывается в цикле, так что отдельная команда не сильно нужна
                        Logger.Instance.Info(SourceFilePath, "Команда 'status' (обновление не требуется, происходит автоматически).");
                        break;
                    case "implement": // Пример: implement plough depth 0.2 | implement seeder rate 50 | implement sprayer intensity 75
                        HandleImplementCommand(parts);
                        break;
                    case "exit":
                        _keepRunning = false;
                        Logger.Instance.Info(SourceFilePath, "Команда 'exit'. Завершение работы интерфейса.");
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
            // Для примера, выставим начальную позицию трактора (можно сделать конфигурируемым)
            _controlUnit.GetCurrentPosition(); // Чтобы "прогреть" навигацию, если она ленивая.
            _controlUnit.StartOperation(target, null, implement); // Boundaries пока null
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
                Console.WriteLine("Использование: implement <тип> <параметр> <значение> | implement attach <тип> | implement <activate|deactivate>");
                return;
            }

            ImplementControlSystem implementCtrl = _controlUnit.ImplementControl;

            switch (parts[1].ToLower())
            {
                case "attach":
                    if (parts.Length < 3 || !Enum.TryParse<ImplementType>(parts[2], true, out ImplementType typeToAttach))
                    { Console.WriteLine("Использование: implement attach <plough|seeder|sprayer>"); return; }
                    implementCtrl.AttachImplement(typeToAttach);
                    break;
                case "activate":
                    implementCtrl.ActivateOperation();
                    break;
                case "deactivate":
                    implementCtrl.DeactivateOperation();
                    break;
                default: // Предполагаем, что это установка параметра
                    if (parts.Length < 4) { Console.WriteLine("Использование: implement <тип_оборудования> <параметр> <значение>"); return; }
                    if (!Enum.TryParse<ImplementType>(parts[1], true, out ImplementType targetImplement))
                    { Console.WriteLine($"Неизвестный тип оборудования: {parts[1]}"); return; }

                    string paramName = parts[2].ToLower();
                    if (!double.TryParse(parts[3].Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double paramValue))
                    { Console.WriteLine($"Неверное значение параметра: {parts[3]}"); return; }

                    if (implementCtrl.GetAttachedImplementType() != targetImplement)
                    {
                        Console.WriteLine($"Сначала подключите оборудование '{targetImplement}' командой 'implement attach {targetImplement.ToString().ToLower()}'");
                        return;
                    }

                    switch (targetImplement)
                    {
                        case ImplementType.Plough:
                            if (paramName == "depth") implementCtrl.SetPloughDepth(paramValue);
                            else Console.WriteLine($"Неизвестный параметр '{paramName}' для плуга.");
                            break;
                        case ImplementType.Seeder:
                            if (paramName == "rate") implementCtrl.SetSeederRate(paramValue);
                            else Console.WriteLine($"Неизвестный параметр '{paramName}' для сеялки.");
                            break;
                        case ImplementType.Sprayer:
                            if (paramName == "intensity") implementCtrl.SetSprayerIntensity(paramValue);
                            else Console.WriteLine($"Неизвестный параметр '{paramName}' для опрыскивателя.");
                            break;
                        default:
                            Console.WriteLine($"Установка параметров для '{targetImplement}' не поддерживается или оборудование не подключено.");
                            break;
                    }
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
            Console.WriteLine("  implement <тип> depth <значение>   - Установить глубину плуга (если плуг подключен). Пример: implement plough depth 0.2");
            Console.WriteLine("  implement <тип> rate <значение>    - Установить норму высева сеялки. Пример: implement seeder rate 50");
            Console.WriteLine("  implement <тип> intensity <знач>   - Установить интенсивность опрыскивателя. Пример: implement sprayer intensity 75");
            Console.WriteLine("  implement activate                 - Активировать операцию подключенного оборудования.");
            Console.WriteLine("  implement deactivate               - Деактивировать операцию оборудования.");
            Console.WriteLine("  status                             - (Обновляется автоматически) Показать текущий статус.");
            Console.WriteLine("  help                               - Показать эту справку.");
            Console.WriteLine("  exit                               - Выйти из приложения.");
            Console.WriteLine();
        }
    }
}