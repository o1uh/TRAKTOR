using Traktor.Interfaces;
using Traktor.Core;
using Traktor.DataModels;
using Traktor.Implements;

namespace Traktor.States
{
    public class StoppedControlUnitState : IControlUnitState
    {
        private const string SourceFilePath = "States/StoppedControlUnitState.cs";
        private const string StateName = "����������";
        public string GetStateName() => StateName;

        public void HandleStartRequest(IControlUnitStateContext context, Coordinates targetPosition, ImplementType implementType, FieldBoundaries boundaries)
        {
            Logger.Instance.Info(SourceFilePath, $"��������� '{StateName}': ������� ������ �� �����. ��������� ��������...");
            context.PerformStartOperation(targetPosition, boundaries, implementType);

            Logger.Instance.Info(SourceFilePath, $"��������� '{StateName}': �������� ��������. ������� � ��������� '��������'.");
            context.SetState(new OperatingControlUnitState());
        }

        public void HandleStopRequest(IControlUnitStateContext context)
        {
            Logger.Instance.Info(SourceFilePath, $"��������� '{StateName}': ������� ������ �� ���������. ������� ��� �����������. �������� �� ���������.");
        }

        public void HandleSimulationStep(IControlUnitStateContext context)
        {
            Logger.Instance.Debug(SourceFilePath, $"��������� '{StateName}': ������� ������ �� ��� ���������. ������� �����������. ��� �� �����������.");
        }
    }
}