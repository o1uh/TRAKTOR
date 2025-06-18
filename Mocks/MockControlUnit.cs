using System;
using System.Collections.Generic;
using Traktor.Core;
using Traktor.DataModels;
using Traktor.Implements;
using Traktor.Interfaces;
using Traktor.States;

namespace Traktor.Mocks
{
    public class MockControlUnit : IControlUnitCommands, IControlUnitStateContext, ISubject
    {
        private const string SourceFilePath = "Mocks/MockControlUnit.cs";
        private IControlUnitState _currentState;
        private string _lastOperationPerformed;

        // --- Observer ---
        private readonly List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            if (observer != null && !_observers.Contains(observer))
            {
                _observers.Add(observer);
                Logger.Instance.Info(SourceFilePath, $"MockControlUnit (Subject): ����������� '{observer.GetType().Name}' �������� (��������). ����� ������������: {_observers.Count}.");
            }
        }

        public void Detach(IObserver observer)
        {
            if (observer != null && _observers.Remove(observer))
            {
                Logger.Instance.Info(SourceFilePath, $"MockControlUnit (Subject): ����������� '{observer.GetType().Name}' ������ (�������). ����� ������������: {_observers.Count}.");
            }
        }

        public void Notify(string message)
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit (Subject): ����������� ������������ ����������: '{message}'. ���������� ������������: {_observers.Count}.");
            List<IObserver> observersSnapshot = new List<IObserver>(_observers);
            foreach (var observer in observersSnapshot)
            {
                try
                {
                    observer.Update(this, message);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(SourceFilePath, $"MockControlUnit (Subject): ������ ��� ����������� ����������� '{observer.GetType().Name}': {ex.Message}", ex);
                }
            }
        }

        public string LastOperationPerformed
        {
            get => _lastOperationPerformed;
            private set
            {
                if (_lastOperationPerformed != value)
                {
                    _lastOperationPerformed = value;
                    Notify($"�������� ��������� ��������: '{_lastOperationPerformed}'");
                }
            }
        }

        public MockControlUnit()
        {
            _currentState = new Traktor.States.StoppedControlUnitState();
            this.LastOperationPerformed = "None (�������������)";
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit ������. ��������� ���������: '{_currentState.GetStateName()}', LastOperation: '{_lastOperationPerformed}'.");
        }


        // --- Memento---
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
        /// <returns>������ Memento (������������ ��� object).</returns>
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
                IControlUnitState previousStateBeforeRestore = _currentState;
                string previousLastOpBeforeRestore = LastOperationPerformed;

                _currentState = memento.State;
                this.LastOperationPerformed = memento.LastOperation;

                Logger.Instance.Info(SourceFilePath, $"MockControlUnit: ��������� ������������� �� Memento.");
                Logger.Instance.Info(SourceFilePath, $"  ����: ���������='{previousStateBeforeRestore?.GetStateName()}', LastOp='{previousLastOpBeforeRestore}'");
                Logger.Instance.Info(SourceFilePath, $"  �����: ���������='{_currentState.GetStateName()}', LastOp='{LastOperationPerformed}'");

                if (previousStateBeforeRestore?.GetStateName() != _currentState.GetStateName())
                {
                    Notify($"��������� ������������� � �������� ��: '{_currentState.GetStateName()}' (����: '{previousStateBeforeRestore?.GetStateName()}')");
                }
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
                string oldStateName = _currentState?.GetStateName() ?? "null";
                if (oldStateName != newState.GetStateName()) 
                {
                    Logger.Instance.Info(SourceFilePath, $"MockControlUnit: ����� ��������� � '{oldStateName}' �� '{newState.GetStateName()}'.");
                    _currentState = newState; 
                    Notify($"��������� �������� ��: '{_currentState.GetStateName()}' (����: '{oldStateName}')"); 
                }
                else
                {
                    _currentState = newState; 
                }
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "MockControlUnit: ������� ���������� null � �������� ���������.");
            }
        }

        public void PerformStartOperation(Coordinates targetPosition, FieldBoundaries fieldBoundaries, ImplementType implementType)
        {
            this.LastOperationPerformed = $"StartOp_Target:{targetPosition}_Impl:{implementType}";
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit (PerformStartOperation): �������� ��������� ������� ��������. LastOperation: '{this.LastOperationPerformed}'.");
        }

        public void PerformStopOperation()
        {
            this.LastOperationPerformed = "StopOp";
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit (PerformStopOperation): �������� �������� ��������� ��������. LastOperation: '{this.LastOperationPerformed}'.");
        }

        public void PerformSimulationStep()
        {
            this.LastOperationPerformed = $"SimStep_{DateTime.Now.Millisecond}";
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit (PerformSimulationStep): �������� ���������� ������ ���� ���������. LastOperation: '{this.LastOperationPerformed}'.");
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

        // --- ���������� IControlUnitCommands ---
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