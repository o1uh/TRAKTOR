// Файл: States/OperatingControlUnitState.cs
using Traktor.Interfaces;
using Traktor.Core;
using Traktor.DataModels;
using Traktor.Implements;

namespace Traktor.States
{
    public class OperatingControlUnitState : IControlUnitState
    {
        private const string SourceFilePath = "States/OperatingControlUnitState.cs";
        private const string StateName = "Работает";
        public string GetStateName() => StateName;

        public void HandleStartRequest(IControlUnitStateContext context, Coordinates targetPosition, ImplementType implementType, FieldBoundaries boundaries)
        {
            Logger.Instance.Info(SourceFilePath, $"Состояние '{StateName}': Получен запрос на старт. Система уже работает. Действий не требуется.");
        }

        public void HandleStopRequest(IControlUnitStateContext context)
        {
            Logger.Instance.Info(SourceFilePath, $"Состояние '{StateName}': Получен запрос на остановку. Останавливаем операции...");
            context.PerformStopOperation();

            Logger.Instance.Info(SourceFilePath, $"Состояние '{StateName}': Операция остановлена. Переход в состояние 'Остановлен'.");
            context.SetState(new StoppedControlUnitState());
        }

        public void HandleSimulationStep(IControlUnitStateContext context)
        {
            Logger.Instance.Debug(SourceFilePath, $"Состояние '{StateName}': Получен запрос на шаг симуляции. Выполняем шаг...");
            context.PerformSimulationStep();
        }
    }
}