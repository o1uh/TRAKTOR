using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.Operations
{
    /// <summary>
    /// ���������� ����������: �������� �� ���������� �����.
    /// </summary>
    public class SoilPreparationOperation : TractorOperationBase
    {
        private const string SourceFilePath = "Operations/SoilPreparationOperation.cs";

        public SoilPreparationOperation(IOperationExecutor executor)
            : base("���������� �����", executor)
        {
        }

        protected override string GetSourceFilePath() => SourceFilePath;

        public override void ExecuteOperation()
        {
            Logger.Instance.Info(SourceFilePath, $"������ �������� '{_operationName}' � ������������ '{_executor.GetType().Name}'.");

            _executor.StartStep(_operationName, "�������");
            _executor.ProcessStep(_operationName);
            string ploughResult = _executor.FinishStep(_operationName, "�������");
            Logger.Instance.Debug(SourceFilePath, $"��������� �������: {ploughResult}");

            _executor.StartStep(_operationName, "�����������");
            _executor.ProcessStep(_operationName);
            string harrowingResult = _executor.FinishStep(_operationName, "�����������");
            Logger.Instance.Debug(SourceFilePath, $"��������� �����������: {harrowingResult}");

            Logger.Instance.Info(SourceFilePath, $"�������� '{_operationName}' ������� ���������.");
        }
    }
}