// ����: States/OperatingControlUnitState.cs
using Traktor.Interfaces;
using Traktor.Core;
using Traktor.DataModels;
using Traktor.Implements;

namespace Traktor.States
{
    public class OperatingControlUnitState : IControlUnitState
    {
        private const string SourceFilePath = "States/OperatingControlUnitState.cs";
        private const string StateName = "��������";
        public string GetStateName() => StateName;

        public void HandleStartRequest(IControlUnitStateContext context, Coordinates targetPosition, ImplementType implementType, FieldBoundaries boundaries)
        {
            Logger.Instance.Info(SourceFilePath, $"��������� '{StateName}': ������� ������ �� �����. ������� ��� ��������. �������� �� ���������.");
        }

        public void HandleStopRequest(IControlUnitStateContext context)
        {
            Logger.Instance.Info(SourceFilePath, $"��������� '{StateName}': ������� ������ �� ���������. ������������� ��������...");
            context.PerformStopOperation();

            Logger.Instance.Info(SourceFilePath, $"��������� '{StateName}': �������� �����������. ������� � ��������� '����������'.");
            context.SetState(new StoppedControlUnitState());
        }

        public void HandleSimulationStep(IControlUnitStateContext context)
        {
            Logger.Instance.Debug(SourceFilePath, $"��������� '{StateName}': ������� ������ �� ��� ���������. ��������� ���...");
            context.PerformSimulationStep();
        }
    }
}