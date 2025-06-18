using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.OperationExecutors
{
    /// <summary>
    /// ���������� ����������: ��������� �������� ����� "��������� ���������".
    /// </summary>
    public class DetailedSimulationExecutor : IOperationExecutor
    {
        private const string SourceFilePath = "OperationExecutors/DetailedSimulationExecutor.cs";

        public DetailedSimulationExecutor()
        {
            Logger.Instance.Info(SourceFilePath, "DetailedSimulationExecutor ������.");
        }

        public void StartStep(string operationName, string stepDetails)
        {
            Logger.Instance.Info(SourceFilePath, $"[��������� ���������] �������� '{operationName}': ������ ���� '{stepDetails}'. ������������� ��������� ����������...");
            System.Threading.Thread.Sleep(50); 
        }

        public void ProcessStep(string operationName)
        {
            Logger.Instance.Info(SourceFilePath, $"[��������� ���������] �������� '{operationName}': ��������� ���� � ������� ���������, ������ ��������� ����������...");
            System.Threading.Thread.Sleep(100);
        }

        public string FinishStep(string operationName, string stepDetails)
        {
            Logger.Instance.Info(SourceFilePath, $"[��������� ���������] �������� '{operationName}': ���������� ���� '{stepDetails}'. ���� ��������� �����������, ������������ ������...");
            System.Threading.Thread.Sleep(50);
            return $"��������� ��������� ��������� ��� '{stepDetails}' � �������� '{operationName}': �������, ��� ��������� � �����.";
        }
    }
}