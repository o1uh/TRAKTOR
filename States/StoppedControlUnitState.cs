using Traktor.Interfaces;
using Traktor.Core;
using Traktor.DataModels;
using Traktor.Implements;

namespace Traktor.States
{
    public class StoppedControlUnitState : IControlUnitState
    {
        private const string SourceFilePath = "States/StoppedControlUnitState.cs";
        private const string StateName = "Остановлен";
        public string GetStateName() => StateName;

        public void HandleStartRequest(IControlUnitStateContext context, Coordinates targetPosition, ImplementType implementType, FieldBoundaries boundaries)
        {
            Logger.Instance.Info(SourceFilePath, $"Состояние '{StateName}': Получен запрос на старт. Запускаем операции...");
            context.PerformStartOperation(targetPosition, boundaries, implementType);

            Logger.Instance.Info(SourceFilePath, $"Состояние '{StateName}': Операция запущена. Переход в состояние 'Работает'.");
            context.SetState(new OperatingControlUnitState());
        }

        public void HandleStopRequest(IControlUnitStateContext context)
        {
            Logger.Instance.Info(SourceFilePath, $"Состояние '{StateName}': Получен запрос на остановку. Система уже остановлена. Действий не требуется.");
        }

        public void HandleSimulationStep(IControlUnitStateContext context)
        {
            Logger.Instance.Debug(SourceFilePath, $"Состояние '{StateName}': Получен запрос на шаг симуляции. Система остановлена. Шаг не выполняется.");
        }
    }
}