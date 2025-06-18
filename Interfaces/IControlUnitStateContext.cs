using Traktor.DataModels;
using Traktor.Implements;

namespace Traktor.Interfaces
{
    /// <summary>
    /// ���������� �������� ��� �������� ��������� ControlUnit.
    /// ������������� ������ ��� ����� ��������� � ���������� �������� ��������.
    /// </summary>
    public interface IControlUnitStateContext
    {
        void SetState(IControlUnitState newState);
        void PerformStartOperation(Coordinates targetPosition, FieldBoundaries fieldBoundaries, ImplementType implementType);
        void PerformStopOperation();
        void PerformSimulationStep();
    }
}