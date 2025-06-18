using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.ReportProducts
{
    /// <summary>
    /// ���������� �������: ���������������� XML �����.
    /// </summary>
    public class DetailedXmlReport : IFieldReportProduct
    {
        private const string SourceFilePath = "ReportProducts/DetailedXmlReport.cs";
        private const string ReportType = "���������������� XML �����";

        public DetailedXmlReport()
        {
            Logger.Instance.Info(SourceFilePath, $"������ ��������� '{ReportType}'.");
        }

        public void DisplayFormat()
        {
            string content = $"<Report type=\"DetailedFieldStatus\">\n" +
                             $"  <Section name=\"Soil\">\n" +
                             $"    <Parameter name=\"Moisture\" value=\"45%\" />\n" +
                             $"    <Parameter name=\"Temperature\" value=\"15C\" />\n" +
                             $"  </Section>\n" +
                             $"  <Section name=\"Pests\">\n" +
                             $"    <Status>LowActivity</Status>\n" +
                             $"  </Section>\n" +
                             $"</Report>";
            Logger.Instance.Info(SourceFilePath, $"����������� ������ (XML ������):\n{content}");
        }

        public string GetReportType()
        {
            return ReportType;
        }
    }
}