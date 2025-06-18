using System; 
using Traktor.Interfaces; 
using Traktor.Core;     

namespace Traktor.Operations
{
    /// <summary>
    /// Абстракция в паттерне Мост.
    /// Определяет высокоуровневый интерфейс для операций трактора
    /// и хранит ссылку на объект Реализатора (IOperationExecutor).
    /// </summary>
    public abstract class TractorOperationBase
    {
        protected readonly IOperationExecutor _executor; // Ссылка на Реализатора
        protected readonly string _operationName;

        /// <summary>
        /// Инициализирует новый экземпляр базовой операции.
        /// </summary>
        /// <param name="operationName">Название операции.</param>
        /// <param name="executor">Реализатор, который будет выполнять низкоуровневые шаги.</param>
        protected TractorOperationBase(string operationName, IOperationExecutor executor)
        {
            _operationName = operationName ?? "Неименованная операция";
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            Logger.Instance.Info(GetSourceFilePath(), $"Создана операция '{_operationName}' с исполнителем типа '{_executor.GetType().Name}'.");
        }

        /// <summary>
        /// Метод, который должен быть реализован наследниками для определения
        /// специфической логики выполнения операции через вызовы _executor.
        /// </summary>
        public abstract void ExecuteOperation();

        /// <summary>
        /// Вспомогательный метод, чтобы наследники могли указать свой SourceFilePath для логгера.
        /// </summary>
        protected abstract string GetSourceFilePath();
    }
}