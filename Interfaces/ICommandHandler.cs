using Traktor.Interfaces; 
using Traktor.Core;     

namespace Traktor.Interfaces
{
    /// <summary>
    /// Интерфейс для обработчика в цепочке обязанностей.
    /// Обрабатывает строковый ввод и потенциально создает команду.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Устанавливает следующего обработчика в цепочке.
        /// </summary>
        /// <param name="nextHandler">Следующий обработчик.</param>
        void SetNext(ICommandHandler nextHandler);

        /// <summary>
        /// Пытается обработать строку ввода.
        /// </summary>
        /// <param name="userInput">Строка, введенная пользователем.</param>
        /// <param name="receiver">Получатель команд (например, ControlUnit).</param>
        /// <returns>Созданная команда ICommand, если ввод распознан, иначе null.</returns>
        ICommand HandleRequest(string userInput, IControlUnitCommands receiver);
    }
}