using Traktor.Interfaces; 

namespace Traktor.Core
{
    /// <summary>
    /// Простой Инициатор (Invoker) команд.
    /// Хранит и выполняет переданную ему команду.
    /// В более сложной реализации мог бы иметь историю команд, очередь и т.д.
    /// </summary>
    public class CommandInvoker
    {
        private const string SourceFilePath = "Commands/CommandInvoker.cs"; // Обновите, если папка другая
        private ICommand _command;

        public CommandInvoker()
        {
            Logger.Instance.Debug(SourceFilePath, "CommandInvoker создан.");
        }

        /// <summary>
        /// Устанавливает команду, которая будет выполнена.
        /// </summary>
        /// <param name="command">Команда для выполнения.</param>
        public void SetCommand(ICommand command)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            Logger.Instance.Info(SourceFilePath, $"CommandInvoker: Установлена команда типа '{_command.GetType().Name}'.");
        }

        /// <summary>
        /// Выполняет установленную команду.
        /// </summary>
        public void ExecuteCommand()
        {
            if (_command != null)
            {
                Logger.Instance.Info(SourceFilePath, $"CommandInvoker: Выполнение команды '{_command.GetType().Name}'...");
                try
                {
                    _command.Execute();
                    Logger.Instance.Info(SourceFilePath, $"CommandInvoker: Команда '{_command.GetType().Name}' успешно выполнена.");
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(SourceFilePath, $"CommandInvoker: Ошибка при выполнении команды '{_command.GetType().Name}': {ex.Message}", ex);
                    // В зависимости от требований, можно решить, должен ли Invoker обрабатывать исключения или пробрасывать их
                }
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "CommandInvoker: Попытка выполнить команду, но команда не установлена.");
            }
        }
    }
}