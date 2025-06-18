using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.Operations
{
    /// <summary>
    /// Уточненная Абстракция: Операция по подготовке почвы.
    /// </summary>
    public class SoilPreparationOperation : TractorOperationBase
    {
        private const string SourceFilePath = "Operations/SoilPreparationOperation.cs";

        public SoilPreparationOperation(IOperationExecutor executor)
            : base("Подготовка почвы", executor)
        {
        }

        protected override string GetSourceFilePath() => SourceFilePath;

        public override void ExecuteOperation()
        {
            Logger.Instance.Info(SourceFilePath, $"Начало операции '{_operationName}' с исполнителем '{_executor.GetType().Name}'.");

            _executor.StartStep(_operationName, "Вспашка");
            _executor.ProcessStep(_operationName);
            string ploughResult = _executor.FinishStep(_operationName, "Вспашка");
            Logger.Instance.Debug(SourceFilePath, $"Результат вспашки: {ploughResult}");

            _executor.StartStep(_operationName, "Боронование");
            _executor.ProcessStep(_operationName);
            string harrowingResult = _executor.FinishStep(_operationName, "Боронование");
            Logger.Instance.Debug(SourceFilePath, $"Результат боронования: {harrowingResult}");

            Logger.Instance.Info(SourceFilePath, $"Операция '{_operationName}' успешно завершена.");
        }
    }
}