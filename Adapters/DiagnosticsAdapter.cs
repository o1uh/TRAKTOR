using System.Xml.Linq;
using Traktor.Core;
using Traktor.Interfaces; 

namespace Traktor.Adapters
{
    public class DiagnosticsAdapter : ITractorDiagnostics // Реализуем интерфейс из Traktor.Interfaces
    {
        private readonly LegacyDiagnosticsSystem _legacySystem; // LegacySystem может быть в Traktor.Adapters или Traktor.LegacySystems
        private const string SourceFilePath = "Adapters/DiagnosticsAdapter.cs";

        public DiagnosticsAdapter(LegacyDiagnosticsSystem legacySystem)
        {
            _legacySystem = legacySystem ?? throw new ArgumentNullException(nameof(legacySystem));
            Logger.Instance.Info(SourceFilePath, $"DiagnosticsAdapter создан. Адаптирует систему типа: '{_legacySystem.GetType().FullName}'.");
        }

        public Dictionary<string, string> GetDiagnosticInfo()
        {
            Logger.Instance.Debug(SourceFilePath, "DiagnosticsAdapter: GetDiagnosticInfo() вызван. Запрашиваю XML-статус у LegacyDiagnosticsSystem.");
            string xmlStatus = _legacySystem.GetSystemStatusXml();
            Logger.Instance.Debug(SourceFilePath, $"DiagnosticsAdapter: Получен XML от LegacyDiagnosticsSystem: {xmlStatus}");

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
                Logger.Instance.Info(SourceFilePath, $"DiagnosticsAdapter: XML успешно преобразован в Dictionary. Обнаружено параметров: {diagnosticData.Count}.");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"DiagnosticsAdapter: Ошибка при парсинге XML: {ex.Message}. Возвращается пустой словарь.", ex);
                return new Dictionary<string, string>();
            }

            return diagnosticData;
        }
    }
}