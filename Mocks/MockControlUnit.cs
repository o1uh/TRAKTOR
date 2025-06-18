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
                Logger.Instance.Info(SourceFilePath, $"MockControlUnit (Subject): Наблюдатель '{observer.GetType().Name}' добавлен (подписан). Всего наблюдателей: {_observers.Count}.");
            }
        }

        public void Detach(IObserver observer)
        {
            if (observer != null && _observers.Remove(observer))
            {
                Logger.Instance.Info(SourceFilePath, $"MockControlUnit (Subject): Наблюдатель '{observer.GetType().Name}' удален (отписан). Всего наблюдателей: {_observers.Count}.");
            }
        }

        public void Notify(string message)
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit (Subject): Уведомление наблюдателей сообщением: '{message}'. Количество наблюдателей: {_observers.Count}.");
            List<IObserver> observersSnapshot = new List<IObserver>(_observers);
            foreach (var observer in observersSnapshot)
            {
                try
                {
                    observer.Update(this, message);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(SourceFilePath, $"MockControlUnit (Subject): Ошибка при уведомлении наблюдателя '{observer.GetType().Name}': {ex.Message}", ex);
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
                    Notify($"Изменена последняя операция: '{_lastOperationPerformed}'");
                }
            }
        }

        public MockControlUnit()
        {
            _currentState = new Traktor.States.StoppedControlUnitState();
            this.LastOperationPerformed = "None (Инициализация)";
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit создан. Начальное состояние: '{_currentState.GetStateName()}', LastOperation: '{_lastOperationPerformed}'.");
        }


        // --- Memento---
        /// <summary>
        /// Внутренний класс Memento, хранящий состояние MockControlUnit.
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
        /// Создает снимок текущего состояния.
        /// </summary>
        /// <returns>Объект Memento (возвращается как object).</returns>
        public object CreateMemento()
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit: Создание Memento. Текущее состояние: '{_currentState.GetStateName()}', LastOperation: '{LastOperationPerformed}'.");
            return new Memento(_currentState, LastOperationPerformed);
        }

        /// <summary>
        /// Восстанавливает состояние из снимка.
        /// </summary>
        /// <param name="mementoObject">Объект Memento, ранее созданный CreateMemento().</param>
        public void RestoreMemento(object mementoObject)
        {
            if (mementoObject is Memento memento)
            {
                IControlUnitState previousStateBeforeRestore = _currentState;
                string previousLastOpBeforeRestore = LastOperationPerformed;

                _currentState = memento.State;
                this.LastOperationPerformed = memento.LastOperation;

                Logger.Instance.Info(SourceFilePath, $"MockControlUnit: Состояние восстановлено из Memento.");
                Logger.Instance.Info(SourceFilePath, $"  Было: Состояние='{previousStateBeforeRestore?.GetStateName()}', LastOp='{previousLastOpBeforeRestore}'");
                Logger.Instance.Info(SourceFilePath, $"  Стало: Состояние='{_currentState.GetStateName()}', LastOp='{LastOperationPerformed}'");

                if (previousStateBeforeRestore?.GetStateName() != _currentState.GetStateName())
                {
                    Notify($"Состояние восстановлено и изменено на: '{_currentState.GetStateName()}' (было: '{previousStateBeforeRestore?.GetStateName()}')");
                }
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "MockControlUnit: RestoreMemento получил неверный тип объекта Memento.");
            }
        }


        // --- Реализация IControlUnitStateContext ---
        public void SetState(IControlUnitState newState)
        {
            if (newState != null)
            {
                string oldStateName = _currentState?.GetStateName() ?? "null";
                if (oldStateName != newState.GetStateName()) 
                {
                    Logger.Instance.Info(SourceFilePath, $"MockControlUnit: Смена состояния с '{oldStateName}' на '{newState.GetStateName()}'.");
                    _currentState = newState; 
                    Notify($"Состояние изменено на: '{_currentState.GetStateName()}' (было: '{oldStateName}')"); 
                }
                else
                {
                    _currentState = newState; 
                }
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "MockControlUnit: Попытка установить null в качестве состояния.");
            }
        }

        public void PerformStartOperation(Coordinates targetPosition, FieldBoundaries fieldBoundaries, ImplementType implementType)
        {
            this.LastOperationPerformed = $"StartOp_Target:{targetPosition}_Impl:{implementType}";
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit (PerformStartOperation): Имитация реального запуска операций. LastOperation: '{this.LastOperationPerformed}'.");
        }

        public void PerformStopOperation()
        {
            this.LastOperationPerformed = "StopOp";
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit (PerformStopOperation): Имитация реальной остановки операций. LastOperation: '{this.LastOperationPerformed}'.");
        }

        public void PerformSimulationStep()
        {
            this.LastOperationPerformed = $"SimStep_{DateTime.Now.Millisecond}";
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit (PerformSimulationStep): Имитация выполнения одного шага симуляции. LastOperation: '{this.LastOperationPerformed}'.");
        }

        // --- Методы, вызываемые извне ---
        public void RequestStart(Coordinates targetPosition, ImplementType implementType, FieldBoundaries boundaries = null)
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit: Получен внешний запрос на старт (RequestStart). Делегирование состоянию '{_currentState.GetStateName()}'.");
            _currentState.HandleStartRequest(this, targetPosition, implementType, boundaries);
        }

        public void RequestStop()
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit: Получен внешний запрос на остановку (RequestStop). Делегирование состоянию '{_currentState.GetStateName()}'.");
            _currentState.HandleStopRequest(this);
        }

        public void RequestSimulationStep()
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit: Получен внешний запрос на шаг симуляции (RequestSimulationStep). Делегирование состоянию '{_currentState.GetStateName()}'.");
            _currentState.HandleSimulationStep(this);
        }

        // --- Реализация IControlUnitCommands ---
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