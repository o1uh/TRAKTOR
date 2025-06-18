using Traktor.DataModels;
using Traktor.Implements;

namespace Traktor.Interfaces
{
    /// <summary>
    /// Определяет контекст для объектов состояния ControlUnit.
    /// Предоставляет методы для смены состояния и выполнения реальных операций.
    /// </summary>
    public interface IControlUnitStateContext
    {
        void SetState(IControlUnitState newState);
        void PerformStartOperation(Coordinates targetPosition, FieldBoundaries fieldBoundaries, ImplementType implementType);
        void PerformStopOperation();
        void PerformSimulationStep();
    }
}