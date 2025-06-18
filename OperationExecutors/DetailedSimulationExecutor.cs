using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.OperationExecutors
{
    /// <summary>
    ///  онкретный –еализатор: выполн€ет операции через "детальную симул€цию".
    /// </summary>
    public class DetailedSimulationExecutor : IOperationExecutor
    {
        private const string SourceFilePath = "OperationExecutors/DetailedSimulationExecutor.cs";

        public DetailedSimulationExecutor()
        {
            Logger.Instance.Info(SourceFilePath, "DetailedSimulationExecutor создан.");
        }

        public void StartStep(string operationName, string stepDetails)
        {
            Logger.Instance.Info(SourceFilePath, $"[ƒ≈“јЋ№Ќјя —»ћ”Ћя÷»я] ќпераци€ '{operationName}': Ќачало шага '{stepDetails}'. »нициализаци€ подробных параметров...");
            System.Threading.Thread.Sleep(50); 
        }

        public void ProcessStep(string operationName)
        {
            Logger.Instance.Info(SourceFilePath, $"[ƒ≈“јЋ№Ќјя —»ћ”Ћя÷»я] ќпераци€ '{operationName}': ќбработка шага с высокой точностью, расчет множества переменных...");
            System.Threading.Thread.Sleep(100);
        }

        public string FinishStep(string operationName, string stepDetails)
        {
            Logger.Instance.Info(SourceFilePath, $"[ƒ≈“јЋ№Ќјя —»ћ”Ћя÷»я] ќпераци€ '{operationName}': «авершение шага '{stepDetails}'. —бор детальных результатов, формирование отчета...");
            System.Threading.Thread.Sleep(50);
            return $"–езультат детальной симул€ции дл€ '{stepDetails}' в операции '{operationName}': ”спешно, все параметры в норме.";
        }
    }
}