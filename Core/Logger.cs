using System.Text; // ��� StringBuilder

namespace Traktor.Core
{
    /// <summary>
    /// ������ �������� ���-���������.
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
    /// ������������� ���������������� �������� ����������� ��������� � ����.
    /// ���������� ��� Singleton.
    /// </summary>
    public sealed class Logger
    {
        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());
        private readonly string _logFilePath;
        private static readonly object _lock = new object(); // ������ ��� ���������� ��� ������ � ����

        // �����������: ����������� ������� ��� ������ � ���
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Info; // �� ��������� ����� Info � ����

        /// <summary>
        /// �������� ������������ ��������� �������.
        /// </summary>
        public static Logger Instance => _instance.Value;

        /// <summary>
        /// ��������� ����������� ��� ���������� Singleton.
        /// �������������� ���� � ����� ����.
        /// </summary>
        private Logger()
        {
            // ���������� ���� � ����� ����. ��������, � ����� � ����������� ������.
            // ��� ����� ����� �������� ���� ��� ������� �����, �� ��� �������� ���� ���� ���.
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(logDirectory); // ��������, ��� ���������� ����������
            _logFilePath = Path.Combine(logDirectory, $"TraktorApp_{DateTime.Now:yyyyMMdd_HHmmss_fff}.log");

            // ������� ��������� � ������ �������
            Log(LogLevel.Info, "Core/Logger.cs", "������ ���������������. ������ ������ �����������.");
        }

        /// <summary>
        /// ���������� ��������� � ���-����.
        /// </summary>
        /// <param name="level">������� �������� ���������.</param>
        /// <param name="sourceFilePath">������ ���� � ����� ��������� ��������� (��� ��� �������������).</param>
        /// <param name="message">����� ���������.</param>
        /// <param name="exception">������������ ����������, ��������� � ����������.</param>
        public void Log(LogLevel level, string sourceFilePath, string message, Exception exception = null)
        {
            if (level < MinimumLogLevel)
            {
                return; // �� �������� ��������� ���� �������������� ������
            }

            try
            {
                // ��������� ������ ����
                // ������: [�������]-[�����������������]-[����-��-�� ��:��:��.fff]: ���������
                //          (���� ���� ����������, ��������� ��� ������)
                StringBuilder logEntry = new StringBuilder();
                logEntry.Append($"[{level.ToString().ToUpper()}]-");
                logEntry.Append($"[{Path.GetFileName(sourceFilePath)}]-"); // ���������� ������ ��� ����� ��� ���������
                logEntry.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ");
                logEntry.Append(message);

                if (exception != null)
                {
                    logEntry.AppendLine(); // ����� ������ ����� �������� ����������
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

                // ���������������� ������ � ����
                lock (_lock)
                {
                    File.AppendAllText(_logFilePath, logEntry.ToString() + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // ���� ����������� ���� �� ���� �������� ������, ������� � �������
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"����������� ������ �������: �� ������� �������� � ���� '{_logFilePath}'. ������: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine($"������������ ��������� ��� �����������: [{level}] [{sourceFilePath}] {message}");
                if (exception != null) Console.WriteLine($"������������ ����������: {exception}");
            }
        }

        // ��������������� ������ ��� ��������
        public void Debug(string sourceFilePath, string message) => Log(LogLevel.Debug, sourceFilePath, message);
        public void Info(string sourceFilePath, string message) => Log(LogLevel.Info, sourceFilePath, message);
        public void Warning(string sourceFilePath, string message, Exception ex = null) => Log(LogLevel.Warning, sourceFilePath, message, ex);
        public void Error(string sourceFilePath, string message, Exception ex = null) => Log(LogLevel.Error, sourceFilePath, message, ex);
        public void Fatal(string sourceFilePath, string message, Exception ex = null) => Log(LogLevel.Fatal, sourceFilePath, message, ex);
    }
}