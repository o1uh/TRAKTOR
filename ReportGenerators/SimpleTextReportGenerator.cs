using Traktor.Interfaces;     
using Traktor.ReportProducts; 
using Traktor.Core;           

namespace Traktor.ReportGenerators
{
    /// <summary>
    /// ���������� ��������� (ConcreteCreator), ������� ������� SimpleTextReport.
    /// </summary>
    public class SimpleTextReportGenerator : ReportGenerator
    {
        private const string SourceFilePath = "ReportGenerators/SimpleTextReportGenerator.cs";

        public SimpleTextReportGenerator()
        {
            Logger.Instance.Info(SourceFilePath, "SimpleTextReportGenerator ������.");
        }

        protected override string GetGeneratorSourceFilePath() => SourceFilePath;

        /// <summary>
        /// ���������� ���������� ������: ������� ��������� SimpleTextReport.
        /// </summary>
        /// <returns>����� ��������� SimpleTextReport.</returns>
        protected override IFieldReportProduct CreateReport()
        {
            Logger.Instance.Debug(SourceFilePath, "��������� ����� CreateReport(): �������� SimpleTextReport...");
            return new SimpleTextReport();
        }
    }
}