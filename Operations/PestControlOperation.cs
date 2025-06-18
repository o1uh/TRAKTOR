using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.Operations
{
    /// <summary>
    /// ���������� ����������: �������� �� ������ � ����������� (������������).
    /// </summary>
    public class PestControlOperation : TractorOperationBase
    {
        private const string SourceFilePath = "Operations/PestControlOperation.cs";

        public PestControlOperation(IOperationExecutor executor)
            : base("������ � ����������� (������������)", executor)
        {
        }

        protected override string GetSourceFilePath() => SourceFilePath;

        public override void ExecuteOperation()
        {
            Logger.Instance.Info(SourceFilePath, $"������ �������� '{_operationName}' � ������������ '{_executor.GetType().Name}'.");

            _executor.StartStep(_operationName, "���������� ��������");
            _executor.ProcessStep(_operationName);
            string mixResult = _executor.FinishStep(_operationName, "���������� ��������");
            Logger.Instance.Debug(SourceFilePath, $"��������� ���������� ��������: {mixResult}");

            _executor.StartStep(_operationName, "������������ ������� A");
            _executor.ProcessStep(_operationName);
            string sprayAResult = _executor.FinishStep(_operationName, "������������ ������� A");
            Logger.Instance.Debug(SourceFilePath, $"��������� ������������ ������� A: {sprayAResult}");

            _executor.StartStep(_operationName, "������������ ������� B");
            _executor.ProcessStep(_operationName);
            string sprayBResult = _executor.FinishStep(_operationName, "������������ ������� B");
            Logger.Instance.Debug(SourceFilePath, $"��������� ������������ ������� B: {sprayBResult}");

            Logger.Instance.Info(SourceFilePath, $"�������� '{_operationName}' ������� ���������.");
        }
    }
}