using Traktor.Core;
using Traktor.DataModels;
using Traktor.Implements;
using Traktor.Interfaces; 
using Traktor.States;

namespace Traktor.Mocks
{
    public class MockControlUnit : IControlUnitCommands, IControlUnitStateContext 
    {
        private const string SourceFilePath = "Mocks/MockControlUnit.cs";
        private IControlUnitState _currentState;
        public string LastOperationPerformed { get; private set; }

        public MockControlUnit()
        {
            _currentState = new StoppedControlUnitState();
            LastOperationPerformed = "None";
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit ������. ��������� ���������: '{_currentState.GetStateName()}', LastOperation: '{LastOperationPerformed}'.");
        }
        // --- ���������� Memento ---
        /// <summary>
        /// ���������� ����� Memento, �������� ��������� MockControlUnit.
        /// </summary>
        private class Memento
        {
            public IControlUnitState State { get; }
            public string LastOperation { get; }

            public Memento(IControlUnitState state, string lastOperation)
            {
                State = state;
                LastOperation = lastOperation;
            }
        }

        /// <summary>
        /// ������� ������ �������� ���������.
        /// </summary>
        /// <returns>������ Memento.</returns>
        public object CreateMemento()
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit: �������� Memento. ������� ���������: '{_currentState.GetStateName()}', LastOperation: '{LastOperationPerformed}'.");
            return new Memento(_currentState, LastOperationPerformed);
        }

        /// <summary>
        /// ��������������� ��������� �� ������.
        /// </summary>
        /// <param name="mementoObject">������ Memento, ����� ��������� CreateMemento().</param>
        public void RestoreMemento(object mementoObject)
        {
            if (mementoObject is Memento memento)
            {
                _currentState = memento.State;
                LastOperationPerformed = memento.LastOperation;
                Logger.Instance.Info(SourceFilePath, $"MockControlUnit: ��������� ������������� �� Memento. ����� ���������: '{_currentState.GetStateName()}', LastOperation: '{LastOperationPerformed}'.");
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "MockControlUnit: RestoreMemento ������� �������� ��� ������� Memento.");
            }
        }


        // --- ���������� IControlUnitStateContext ---
        public void SetState(IControlUnitState newState)
        {
            if (newState != null)
            {
                Logger.Instance.Info(SourceFilePath, $"MockControlUnit: ����� ��������� � '{_currentState?.GetStateName() ?? "null"}' �� '{newState.GetStateName()}'.");
                _currentState = newState;
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "MockControlUnit: ������� ���������� null � �������� ���������.");
            }
        }

        public void PerformStartOperation(Coordinates targetPosition, FieldBoundaries fieldBoundaries, ImplementType implementType)
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit (PerformStartOperation): �������� ��������� ������� ��������. ����: {targetPosition}, ������������: {implementType}.");
            Logger.Instance.Info(SourceFilePath, "MockControlUnit (PerformStartOperation): �������� '��������'.");
        }

        public void PerformStopOperation()
        {
            Logger.Instance.Info(SourceFilePath, "MockControlUnit (PerformStopOperation): �������� �������� ��������� ��������.");
            Logger.Instance.Info(SourceFilePath, "MockControlUnit (PerformStopOperation): �������� '�����������'.");
        }

        public void PerformSimulationStep()
        {
            Logger.Instance.Info(SourceFilePath, "MockControlUnit (PerformSimulationStep): �������� ���������� ������ ���� ���������.");
            Logger.Instance.Info(SourceFilePath, "MockControlUnit (PerformSimulationStep): ��� ��������� '��������'.");
        }


        // --- ������, ���������� ����� ---
        public void RequestStart(Coordinates targetPosition, ImplementType implementType, FieldBoundaries boundaries = null)
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit: ������� ������� ������ �� ����� (RequestStart). ������������� ��������� '{_currentState.GetStateName()}'.");
            _currentState.HandleStartRequest(this, targetPosition, implementType, boundaries);
        }

        public void RequestStop()
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit: ������� ������� ������ �� ��������� (RequestStop). ������������� ��������� '{_currentState.GetStateName()}'.");
            _currentState.HandleStopRequest(this);
        }

        public void RequestSimulationStep()
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit: ������� ������� ������ �� ��� ��������� (RequestSimulationStep). ������������� ��������� '{_currentState.GetStateName()}'.");
            _currentState.HandleSimulationStep(this);
        }

        // --- ���������� IControlUnitCommands ��� ������������� ---
        void IControlUnitCommands.StartOperation(Coordinates targetPosition, FieldBoundaries fieldBoundaries, ImplementType implementType)
        {
            this.RequestStart(targetPosition, implementType, fieldBoundaries);
        }

        void IControlUnitCommands.StopOperation()
        {
            this.RequestStop();
        }
    }
}