using Traktor.Core;       
using Traktor.DataModels;
using Traktor.Implements;
using Traktor.Interfaces;

namespace Traktor.Mocks
{
    /// <summary>
    /// Простой макет ControlUnit для демонстрации паттерна Команда.
    /// Логирует вызовы методов, которые должны были бы выполнять реальные действия.
    /// </summary>
    public class MockControlUnit : IControlUnitCommands
    {
        private const string SourceFilePath = "Mocks/MockControlUnit.cs";

        public MockControlUnit()
        {
            Logger.Instance.Debug(SourceFilePath, "MockControlUnit создан (для демонстрации Command).");
        }

        public void StartOperation(Coordinates targetPosition, FieldBoundaries fieldBoundaries, ImplementType implementType)
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit: Вызван StartOperation. Цель: {targetPosition}, Оборудование: {implementType}, Границы: {(fieldBoundaries == null ? "не заданы" : "заданы")}.");
            // Имитация действия
            Logger.Instance.Info(SourceFilePath, "MockControlUnit: Имитация запуска операции... Успешно.");
        }

        public void StopOperation()
        {
            Logger.Instance.Info(SourceFilePath, "MockControlUnit: Вызван StopOperation.");
            // Имитация действия
            Logger.Instance.Info(SourceFilePath, "MockControlUnit: Имитация остановки операции... Успешно.");
        }
    }
}