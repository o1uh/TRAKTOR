using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.OperationExecutors
{
    /// <summary>
    ///  онкретный –еализатор: выполн€ет операции через "быструю симул€цию".
    /// </summary>
    public class FastSimulationExecutor : IOperationExecutor
    {
        private const string SourceFilePath = "OperationExecutors/FastSimulationExecutor.cs";

        public FastSimulationExecutor()
        {
            Logger.Instance.Info(SourceFilePath, "FastSimulationExecutor создан.");
        }

        public void StartStep(string operationName, string stepDetails)
        {
            Logger.Instance.Info(SourceFilePath, $"[Ѕџ—“–јя —»ћ”Ћя÷»я] ќпераци€ '{operationName}': Ќачало шага '{stepDetails}'. ”прощенна€ инициализаци€.");
        }

        public void ProcessStep(string operationName)
        {
            Logger.Instance.Info(SourceFilePath, $"[Ѕџ—“–јя —»ћ”Ћя÷»я] ќпераци€ '{operationName}': Ѕыстра€ обработка шага, основные проверки.");
        }

        public string FinishStep(string operationName, string stepDetails)
        {
            Logger.Instance.Info(SourceFilePath, $"[Ѕџ—“–јя —»ћ”Ћя÷»я] ќпераци€ '{operationName}': «авершение шага '{stepDetails}'. ¬озврат общего результата.");
            return $"–езультат быстрой симул€ции дл€ '{stepDetails}' в операции '{operationName}': «авершено.";
        }
    }
}