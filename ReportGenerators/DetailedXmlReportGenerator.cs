using Traktor.Interfaces;     
using Traktor.ReportProducts;
using Traktor.Core;        
namespace Traktor.ReportGenerators
{
    /// <summary>
    /// ���������� ��������� (ConcreteCreator), ������� ������� DetailedXmlReport.
    /// </summary>
    public class DetailedXmlReportGenerator : ReportGenerator
    {
        private const string SourceFilePath = "ReportGenerators/DetailedXmlReportGenerator.cs";

        public DetailedXmlReportGenerator()
        {
            Logger.Instance.Info(SourceFilePath, "DetailedXmlReportGenerator ������.");
        }

        protected override string GetGeneratorSourceFilePath() => SourceFilePath;

        /// <summary>
        /// ���������� ���������� ������: ������� ��������� DetailedXmlReport.
        /// </summary>
        /// <returns>����� ��������� DetailedXmlReport.</returns>
        protected override IFieldReportProduct CreateReport()
        {
            Logger.Instance.Debug(SourceFilePath, "��������� ����� CreateReport(): �������� DetailedXmlReport...");
            return new DetailedXmlReport();
        }
    }
}