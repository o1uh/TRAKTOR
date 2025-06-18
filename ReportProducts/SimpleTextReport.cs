using Traktor.Interfaces; 
using Traktor.Core;       
namespace Traktor.ReportProducts
{
    /// <summary>
    /// ���������� �������: ������� ��������� �����.
    /// </summary>
    public class SimpleTextReport : IFieldReportProduct
    {
        private const string SourceFilePath = "ReportProducts/SimpleTextReport.cs";
        private const string ReportType = "������� ��������� �����";

        public SimpleTextReport()
        {
            Logger.Instance.Info(SourceFilePath, $"������ ��������� '{ReportType}'.");
        }

        public void DisplayFormat()
        {
            string content = $"--- {ReportType} ---\n��������� ����: ������������������.\n������������: ���������� ����������.\n--------------------";
            Logger.Instance.Info(SourceFilePath, $"����������� ������:\n{content}");
        }

        public string GetReportType()
        {
            return ReportType;
        }
    }
}