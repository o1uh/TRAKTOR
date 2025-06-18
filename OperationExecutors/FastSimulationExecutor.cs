using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.OperationExecutors
{
    /// <summary>
    /// ���������� ����������: ��������� �������� ����� "������� ���������".
    /// </summary>
    public class FastSimulationExecutor : IOperationExecutor
    {
        private const string SourceFilePath = "OperationExecutors/FastSimulationExecutor.cs";

        public FastSimulationExecutor()
        {
            Logger.Instance.Info(SourceFilePath, "FastSimulationExecutor ������.");
        }

        public void StartStep(string operationName, string stepDetails)
        {
            Logger.Instance.Info(SourceFilePath, $"[������� ���������] �������� '{operationName}': ������ ���� '{stepDetails}'. ���������� �������������.");
        }

        public void ProcessStep(string operationName)
        {
            Logger.Instance.Info(SourceFilePath, $"[������� ���������] �������� '{operationName}': ������� ��������� ����, �������� ��������.");
        }

        public string FinishStep(string operationName, string stepDetails)
        {
            Logger.Instance.Info(SourceFilePath, $"[������� ���������] �������� '{operationName}': ���������� ���� '{stepDetails}'. ������� ������ ����������.");
            return $"��������� ������� ��������� ��� '{stepDetails}' � �������� '{operationName}': ���������.";
        }
    }
}