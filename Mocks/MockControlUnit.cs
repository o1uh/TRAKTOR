using Traktor.Core;       
using Traktor.DataModels;
using Traktor.Implements;
using Traktor.Interfaces;

namespace Traktor.Mocks
{
    /// <summary>
    /// ������� ����� ControlUnit ��� ������������ �������� �������.
    /// �������� ������ �������, ������� ������ ���� �� ��������� �������� ��������.
    /// </summary>
    public class MockControlUnit : IControlUnitCommands
    {
        private const string SourceFilePath = "Mocks/MockControlUnit.cs";

        public MockControlUnit()
        {
            Logger.Instance.Debug(SourceFilePath, "MockControlUnit ������ (��� ������������ Command).");
        }

        public void StartOperation(Coordinates targetPosition, FieldBoundaries fieldBoundaries, ImplementType implementType)
        {
            Logger.Instance.Info(SourceFilePath, $"MockControlUnit: ������ StartOperation. ����: {targetPosition}, ������������: {implementType}, �������: {(fieldBoundaries == null ? "�� ������" : "������")}.");
            // �������� ��������
            Logger.Instance.Info(SourceFilePath, "MockControlUnit: �������� ������� ��������... �������.");
        }

        public void StopOperation()
        {
            Logger.Instance.Info(SourceFilePath, "MockControlUnit: ������ StopOperation.");
            // �������� ��������
            Logger.Instance.Info(SourceFilePath, "MockControlUnit: �������� ��������� ��������... �������.");
        }
    }
}