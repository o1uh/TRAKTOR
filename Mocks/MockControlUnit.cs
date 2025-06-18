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
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit создан. Начальное состояние: '{_currentState.GetStateName()}', LastOperation: '{LastOperationPerformed}'.");
        }
        // --- Реализация Memento ---
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
        /// <returns>Объект Memento.</returns>
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
                _currentState = memento.State;
                LastOperationPerformed = memento.LastOperation;
                Logger.Instance.Info(SourceFilePath, $"MockControlUnit: Состояние восстановлено из Memento. Новое состояние: '{_currentState.GetStateName()}', LastOperation: '{LastOperationPerformed}'.");
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
                Logger.Instance.Info(SourceFilePath, $"MockControlUnit: Смена состояния с '{_currentState?.GetStateName() ?? "null"}' на '{newState.GetStateName()}'.");
                _currentState = newState;
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "MockControlUnit: Попытка установить null в качестве состояния.");
            }
        }

        public void PerformStartOperation(Coordinates targetPosition, FieldBoundaries fieldBoundaries, ImplementType implementType)
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit (PerformStartOperation): Имитация реального запуска операций. Цель: {targetPosition}, Оборудование: {implementType}.");
            Logger.Instance.Info(SourceFilePath, "MockControlUnit (PerformStartOperation): Операции 'запущены'.");
        }

        public void PerformStopOperation()
        {
            Logger.Instance.Info(SourceFilePath, "MockControlUnit (PerformStopOperation): Имитация реальной остановки операций.");
            Logger.Instance.Info(SourceFilePath, "MockControlUnit (PerformStopOperation): Операции 'остановлены'.");
        }

        public void PerformSimulationStep()
        {
            Logger.Instance.Info(SourceFilePath, "MockControlUnit (PerformSimulationStep): Имитация выполнения одного шага симуляции.");
            Logger.Instance.Info(SourceFilePath, "MockControlUnit (PerformSimulationStep): Шаг симуляции 'выполнен'.");
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

        // --- Реализация IControlUnitCommands для совместимости ---
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