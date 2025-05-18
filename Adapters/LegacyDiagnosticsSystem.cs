using Traktor.Core; // Для Logger

namespace Traktor.Adapters // или Traktor.LegacySystems
{
    public class LegacyDiagnosticsSystem
    {
        private const string SourceFilePath = "Adapters/LegacyDiagnosticsSystem.cs"; // или LegacySystems/LegacyDiagnosticsSystem.cs

        public LegacyDiagnosticsSystem()
        {
            Logger.Instance.Info(SourceFilePath, "LegacyDiagnosticsSystem инициализирована (симуляция старой системы).");
        }

        public string GetSystemStatusXml()
        {
            Logger.Instance.Debug(SourceFilePath, "LegacyDiagnosticsSystem: GetSystemStatusXml() вызван.");
            string xmlStatus = "<diagnosticsReport><engineStatus>OK</engineStatus><oilLevel>NORMAL</oilLevel><fuelLevel>75%</fuelLevel></diagnosticsReport>";
            Logger.Instance.Debug(SourceFilePath, $"LegacyDiagnosticsSystem: Возвращается XML: {xmlStatus}");
            return xmlStatus;
        }
    }
}