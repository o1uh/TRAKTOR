using Traktor.Core; // ��� Logger

namespace Traktor.Adapters // ��� Traktor.LegacySystems
{
    public class LegacyDiagnosticsSystem
    {
        private const string SourceFilePath = "Adapters/LegacyDiagnosticsSystem.cs"; // ��� LegacySystems/LegacyDiagnosticsSystem.cs

        public LegacyDiagnosticsSystem()
        {
            Logger.Instance.Info(SourceFilePath, "LegacyDiagnosticsSystem ���������������� (��������� ������ �������).");
        }

        public string GetSystemStatusXml()
        {
            Logger.Instance.Debug(SourceFilePath, "LegacyDiagnosticsSystem: GetSystemStatusXml() ������.");
            string xmlStatus = "<diagnosticsReport><engineStatus>OK</engineStatus><oilLevel>NORMAL</oilLevel><fuelLevel>75%</fuelLevel></diagnosticsReport>";
            Logger.Instance.Debug(SourceFilePath, $"LegacyDiagnosticsSystem: ������������ XML: {xmlStatus}");
            return xmlStatus;
        }
    }
}