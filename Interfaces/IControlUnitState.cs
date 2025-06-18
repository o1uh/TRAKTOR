using Traktor.DataModels;
using Traktor.Implements;

namespace Traktor.Interfaces
{
    public interface IControlUnitState
    {
        void HandleStartRequest(IControlUnitStateContext context, Coordinates targetPosition, ImplementType implementType, FieldBoundaries boundaries);
        void HandleStopRequest(IControlUnitStateContext context);
        void HandleSimulationStep(IControlUnitStateContext context);
        string GetStateName();
    }
}