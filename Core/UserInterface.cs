using Traktor.DataModels;       // Для Coordinates и других моделей, если будем их отображать

namespace Traktor.Core
{
    public class UserInterface
    {
        public UserInterface()
        {
            Console.WriteLine("[UserInterface]: Консольный интерфейс пользователя инициализирован.");
            Console.Title = "Система Управления Трактором (Макет)";
        }

        /// <summary>
        /// Отображает общее сообщение для пользователя.
        /// </summary>
        public void DisplayMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.White; // Стандартный цвет
            Console.WriteLine($"[INFO] {message}");
        }

        /// <summary>
        /// Отображает сообщение о состоянии или статусе системы.
        /// </summary>
        public void DisplayStatus(string statusMessage)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[STATUS] {statusMessage}");
            Console.ResetColor();
        }

        /// <summary>
        /// Отображает предупреждение.
        /// </summary>
        public void ShowWarning(string warningMessage)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[ПРЕДУПРЕЖДЕНИЕ] {warningMessage}");
            Console.ResetColor();
        }

        /// <summary>
        /// Отображает сообщение об ошибке.
        /// </summary>
        public void ShowError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ОШИБКА] {errorMessage}");
            Console.ResetColor();
        }

        /// <summary>
        /// Запрашивает команду от оператора.
        /// </summary>
        /// <returns>Введенная пользователем строка команды.</returns>
        public string GetOperatorCommand(string prompt = "Введите команду > ")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(prompt);
            string command = Console.ReadLine();
            Console.ResetColor();
            return command?.Trim().ToLower() ?? string.Empty; // Приводим к нижнему регистру и убираем пробелы
        }

        /// <summary>
        /// Отображает текущие координаты трактора.
        /// </summary>
        public void DisplayTractorPosition(Coordinates position)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[ПОЗИЦИЯ ТРАКТОРА]: {position}");
            Console.ResetColor();
        }

        /// <summary>
        /// Отображает информацию об обнаруженных препятствиях.
        /// </summary>
        public void DisplayObstacles(List<ObstacleData> obstacles)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            if (obstacles == null || !obstacles.Any())
            {
                // Console.WriteLine("[ПРЕПЯТСТВИЯ]: Не обнаружено."); // Можно не выводить, если нет
                return;
            }
            Console.WriteLine("[ОБНАРУЖЕНЫ ПРЕПЯТСТВИЯ]:");
            foreach (var obstacle in obstacles)
            {
                Console.WriteLine($"  - {obstacle}");
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Отображает информацию об особенностях поля (например, сорняках).
        /// </summary>
        public void DisplayFieldFeatures(List<FieldFeatureData> features)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (features == null || !features.Any())
            {
                // Console.WriteLine("[ОСОБЕННОСТИ ПОЛЯ]: Не обнаружено."); // Можно не выводить, если нет
                return;
            }
            Console.WriteLine("[ОБНАРУЖЕНЫ ОСОБЕННОСТИ ПОЛЯ]:");
            foreach (var feature in features)
            {
                Console.WriteLine($"  - {feature}");
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Отображает текущий маршрут.
        /// </summary>
        public void DisplayRoute(List<Coordinates> route, int currentTargetIndex = -1)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (route == null || !route.Any())
            {
                Console.WriteLine("[МАРШРУТ]: Не задан или пуст.");
                Console.ResetColor();
                return;
            }
            Console.WriteLine("[ТЕКУЩИЙ МАРШРУТ]:");
            for (int i = 0; i < route.Count; i++)
            {
                string prefix = (i == currentTargetIndex) ? "  -> ЦЕЛЬ: " : "     Точка: ";
                Console.WriteLine($"{prefix}{route[i]}");
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Запрашивает у пользователя подтверждение (Да/Нет).
        /// </summary>
        public bool RequestConfirmation(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{message} (да/нет): ");
            string input = Console.ReadLine()?.Trim().ToLower();
            Console.ResetColor();
            return input == "да" || input == "д" || input == "y" || input == "yes";
        }

        /// <summary>
        /// Отображает меню доступных команд (пример).
        /// </summary>
        public void ShowAvailableCommands()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nДоступные команды (пример):");
            Console.WriteLine("  start gps <lat_цели> <lon_цели> - Запустить GPS навигацию и рассчитать маршрут");
            Console.WriteLine("  start ins <lat_цели> <lon_цели> - Запустить ИНС навигацию и рассчитать маршрут");
            Console.WriteLine("  stop nav                             - Остановить текущую навигационную систему");
            Console.WriteLine("  status                               - Показать текущий статус");
            Console.WriteLine("  move                                 - Сделать шаг имитации движения (если есть маршрут)");
            Console.WriteLine("  scan obstacles                       - Сканировать на наличие препятствий");
            Console.WriteLine("  scan features                        - Сканировать на наличие особенностей поля");
            Console.WriteLine("  attach <плуг|сеялка|опрыскиватель>   - Подключить оборудование");
            Console.WriteLine("  activate imp                         - Активировать оборудование");
            Console.WriteLine("  deactivate imp                       - Деактивировать оборудование");
            Console.WriteLine("  set depth <значение>                 - Установить глубину плуга");
            Console.WriteLine("  set rate <значение>                  - Установить норму высева");
            Console.WriteLine("  set intensity <значение>             - Установить интенсивность опрыскивания");
            Console.WriteLine("  exit                                 - Выход");
            Console.ResetColor();
        }
    }
}