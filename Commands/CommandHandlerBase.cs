using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.Commands
{
    /// <summary>
    /// Абстрактный базовый класс для обработчиков команд в цепочке обязанностей.
    /// Предоставляет реализацию для установки следующего обработчика и делегирования запроса.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        protected ICommandHandler _nextHandler;

        public void SetNext(ICommandHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        /// <summary>
        /// Обрабатывает запрос или передает его следующему обработчику в цепочке.
        /// </summary>
        /// <param name="userInput">Строка, введенная пользователем.</param>
        /// <param name="receiver">Получатель команд.</param>
        /// <returns>Созданная команда ICommand, если ввод распознан, иначе null.</returns>
        public abstract ICommand HandleRequest(string userInput, IControlUnitCommands receiver);

        /// <summary>
        /// Вспомогательный метод для передачи запроса следующему обработчику, если текущий не смог его обработать.
        /// </summary>
        protected ICommand PassToNext(string userInput, IControlUnitCommands receiver, string currentHandlerName)
        {
            if (_nextHandler != null)
            {
                Logger.Instance.Debug(currentHandlerName, $"Не смог обработать '{userInput}'. Передача следующему: {_nextHandler.GetType().Name}.");
                return _nextHandler.HandleRequest(userInput, receiver);
            }
            else
            {
                Logger.Instance.Debug(currentHandlerName, $"Не смог обработать '{userInput}'. Следующего обработчика нет.");
                return null; // Конец цепочки, команда не распознана
            }
        }
    }
}