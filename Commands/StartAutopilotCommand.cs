using System; 
using Traktor.Core;       
using Traktor.Interfaces; 
using Traktor.DataModels; 
using Traktor.Implements; 

namespace Traktor.Commands
{
    /// <summary>
    /// Команда для запуска операции автопилота.
    /// </summary>
    public class StartAutopilotCommand : ICommand
    {
        private const string SourceFilePath = "Commands/StartAutopilotCommand.cs";
        private readonly IControlUnitCommands _receiver; // Получатель (Receiver)
        private readonly Coordinates _targetPosition;
        private readonly FieldBoundaries _fieldBoundaries; // Может быть null
        private readonly ImplementType _implementType;

        /// <summary>
        /// Инициализирует новый экземпляр команды запуска автопилота.
        /// </summary>
        /// <param name="controlUnit">Экземпляр ControlUnit, который будет выполнять действие.</param>
        /// <param name="targetPosition">Целевая позиция.</param>
        /// <param name="implementType">Тип используемого оборудования.</param>
        /// <param name="fieldBoundaries">Границы поля (опционально).</param>
        public StartAutopilotCommand(IControlUnitCommands receiver, Coordinates targetPosition, ImplementType implementType, FieldBoundaries fieldBoundaries = null)
        {
            _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
            _targetPosition = targetPosition; // Предполагаем, что Coordinates - структура, не может быть null
            _implementType = implementType;
            _fieldBoundaries = fieldBoundaries; // Может быть null
            Logger.Instance.Debug(SourceFilePath, $"StartAutopilotCommand создана. Цель: {_targetPosition}, Оборудование: {_implementType}, Границы: {(_fieldBoundaries == null ? "не заданы" : "заданы")}.");
        }

        /// <summary>
        /// Выполняет команду запуска автопилота.
        /// </summary>
        public void Execute()
        {
            Logger.Instance.Info(SourceFilePath, $"Выполнение StartAutopilotCommand: Запуск автопилота к цели {_targetPosition} с оборудованием {_implementType}.");
            try
            {
                _receiver.StartOperation(_targetPosition, _fieldBoundaries, _implementType);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Ошибка при выполнении StartAutopilotCommand: {ex.Message}", ex);
            }
        }
    }
}