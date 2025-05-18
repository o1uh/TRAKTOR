using System.Text; // Для StringBuilder

namespace Traktor.Core
{
    /// <summary>
    /// Уровни важности лог-сообщений.
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    /// <summary>
    /// Предоставляет централизованный механизм логирования сообщений в файл.
    /// Реализован как Singleton.
    /// </summary>
    public sealed class Logger
    {
        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
        private readonly string _logFilePath;
        private static readonly object _lock = new object(); // Объект для блокировки при записи в файл

        // Опционально: Минимальный уровень для записи в лог
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Info; // По умолчанию пишем Info и выше

        /// <summary>
        /// Получает единственный экземпляр логгера.
        /// </summary>
        public static Logger Instance => _instance.Value;

        /// <summary>
        /// Приватный конструктор для реализации Singleton.
        /// Инициализирует путь к файлу лога.
        /// </summary>
        private Logger()
        {
            // Определяем путь к файлу лога. Например, в папке с исполняемым файлом.
            // Имя файла может включать дату для ротации логов, но для простоты пока одно имя.
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(logDirectory); // Убедимся, что директория существует
            _logFilePath = Path.Combine(logDirectory, $"TraktorApp_{DateTime.Now:yyyyMMdd_HHmmss_fff}.log");

            // Запишем сообщение о старте логгера
            Log(LogLevel.Info, "Core/Logger.cs", "Логгер инициализирован. Начало сессии логирования.");
        }

        /// <summary>
        /// Записывает сообщение в лог-файл.
        /// </summary>
        /// <param name="level">Уровень важности сообщения.</param>
        /// <param name="sourceFilePath">Полный путь к файлу источника сообщения (или его идентификатор).</param>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="exception">Опциональное исключение, связанное с сообщением.</param>
        public void Log(LogLevel level, string sourceFilePath, string message, Exception exception = null)
        {
            if (level < MinimumLogLevel)
            {
                return; // Не логируем сообщения ниже установленного уровня
            }

            try
            {
                // Формируем строку лога
                // Формат: [УРОВЕНЬ]-[ИмяФайлаИсточника]-[ГГГГ-ММ-ДД ЧЧ:мм:сс.fff]: Сообщение
                //          (Если есть исключение, добавляем его детали)
                StringBuilder logEntry = new StringBuilder();
                logEntry.Append($"[{level.ToString().ToUpper()}]-");
                logEntry.Append($"[{Path.GetFileName(sourceFilePath)}]-"); // Используем только имя файла для краткости
                logEntry.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ");
                logEntry.Append(message);

                if (exception != null)
                {
                    logEntry.AppendLine(); // Новая строка перед деталями исключения
                    logEntry.AppendLine("--- Exception Details ---");
                    logEntry.AppendLine($"Type: {exception.GetType().FullName}");
                    logEntry.AppendLine($"Message: {exception.Message}");
                    logEntry.AppendLine($"StackTrace: {exception.StackTrace}");
                    if (exception.InnerException != null)
                    {
                        logEntry.AppendLine("--- Inner Exception ---");
                        logEntry.AppendLine($"Type: {exception.InnerException.GetType().FullName}");
                        logEntry.AppendLine($"Message: {exception.InnerException.Message}");
                        logEntry.AppendLine($"StackTrace: {exception.InnerException.StackTrace}");
                    }
                    logEntry.AppendLine("-------------------------");
                }

                // Потокобезопасная запись в файл
                lock (_lock)
                {
                    File.AppendAllText(_logFilePath, logEntry.ToString() + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // Если логирование само по себе вызывает ошибку, выводим в консоль
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"КРИТИЧЕСКАЯ ОШИБКА ЛОГГЕРА: Не удалось записать в файл '{_logFilePath}'. Ошибка: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine($"Оригинальное сообщение для логирования: [{level}] [{sourceFilePath}] {message}");
                if (exception != null) Console.WriteLine($"Оригинальное исключение: {exception}");
            }
        }

        // Вспомогательные методы для удобства
        public void Debug(string sourceFilePath, string message) => Log(LogLevel.Debug, sourceFilePath, message);
        public void Info(string sourceFilePath, string message) => Log(LogLevel.Info, sourceFilePath, message);
        public void Warning(string sourceFilePath, string message, Exception ex = null) => Log(LogLevel.Warning, sourceFilePath, message, ex);
        public void Error(string sourceFilePath, string message, Exception ex = null) => Log(LogLevel.Error, sourceFilePath, message, ex);
        public void Fatal(string sourceFilePath, string message, Exception ex = null) => Log(LogLevel.Fatal, sourceFilePath, message, ex);
    }
}