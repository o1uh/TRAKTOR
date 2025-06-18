using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.Operations
{
    /// <summary>
    /// Уточненная Абстракция: Операция по борьбе с вредителями (опрыскивание).
    /// </summary>
    public class PestControlOperation : TractorOperationBase
    {
        private const string SourceFilePath = "Operations/PestControlOperation.cs";

        public PestControlOperation(IOperationExecutor executor)
            : base("Борьба с вредителями (Опрыскивание)", executor)
        {
        }

        protected override string GetSourceFilePath() => SourceFilePath;

        public override void ExecuteOperation()
        {
            Logger.Instance.Info(SourceFilePath, $"Начало операции '{_operationName}' с исполнителем '{_executor.GetType().Name}'.");

            _executor.StartStep(_operationName, "Подготовка раствора");
            _executor.ProcessStep(_operationName);
            string mixResult = _executor.FinishStep(_operationName, "Подготовка раствора");
            Logger.Instance.Debug(SourceFilePath, $"Результат подготовки раствора: {mixResult}");

            _executor.StartStep(_operationName, "Опрыскивание участка A");
            _executor.ProcessStep(_operationName);
            string sprayAResult = _executor.FinishStep(_operationName, "Опрыскивание участка A");
            Logger.Instance.Debug(SourceFilePath, $"Результат опрыскивания участка A: {sprayAResult}");

            _executor.StartStep(_operationName, "Опрыскивание участка B");
            _executor.ProcessStep(_operationName);
            string sprayBResult = _executor.FinishStep(_operationName, "Опрыскивание участка B");
            Logger.Instance.Debug(SourceFilePath, $"Результат опрыскивания участка B: {sprayBResult}");

            Logger.Instance.Info(SourceFilePath, $"Операция '{_operationName}' успешно завершена.");
        }
    }
}