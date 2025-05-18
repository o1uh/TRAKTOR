using System.Xml.Linq;
using Traktor.Core;
using Traktor.Interfaces; 

namespace Traktor.Adapters
{
    public class DiagnosticsAdapter : ITractorDiagnostics // ��������� ��������� �� Traktor.Interfaces
    {
        private readonly LegacyDiagnosticsSystem _legacySystem; // LegacySystem ����� ���� � Traktor.Adapters ��� Traktor.LegacySystems
        private const string SourceFilePath = "Adapters/DiagnosticsAdapter.cs";

        public DiagnosticsAdapter(LegacyDiagnosticsSystem legacySystem)
        {
            _legacySystem = legacySystem ?? throw new ArgumentNullException(nameof(legacySystem));
            Logger.Instance.Info(SourceFilePath, $"DiagnosticsAdapter ������. ���������� ������� ����: '{_legacySystem.GetType().FullName}'.");
        }

        public Dictionary<string, string> GetDiagnosticInfo()
        {
            Logger.Instance.Debug(SourceFilePath, "DiagnosticsAdapter: GetDiagnosticInfo() ������. ���������� XML-������ � LegacyDiagnosticsSystem.");
            string xmlStatus = _legacySystem.GetSystemStatusXml();
            Logger.Instance.Debug(SourceFilePath, $"DiagnosticsAdapter: ������� XML �� LegacyDiagnosticsSystem: {xmlStatus}");

            var diagnosticData = new Dictionary<string, string>();
            try
            {
                XDocument doc = XDocument.Parse(xmlStatus);
                if (doc.Root != null)
                {
                    foreach (var element in doc.Root.Elements())
                    {
                        diagnosticData[element.Name.LocalName] = element.Value;
                    }
                }
                Logger.Instance.Info(SourceFilePath, $"DiagnosticsAdapter: XML ������� ������������ � Dictionary. ���������� ����������: {diagnosticData.Count}.");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"DiagnosticsAdapter: ������ ��� �������� XML: {ex.Message}. ������������ ������ �������.", ex);
                return new Dictionary<string, string>();
            }

            return diagnosticData;
        }
    }
}