using Traktor.Interfaces;     
using Traktor.ReportProducts;
using Traktor.Core;        
namespace Traktor.ReportGenerators
{
    /// <summary>
    /// Конкретный Создатель (ConcreteCreator), который создает DetailedXmlReport.
    /// </summary>
    public class DetailedXmlReportGenerator : ReportGenerator
    {
        private const string SourceFilePath = "ReportGenerators/DetailedXmlReportGenerator.cs";

        public DetailedXmlReportGenerator()
        {
            Logger.Instance.Info(SourceFilePath, "DetailedXmlReportGenerator создан.");
        }

        protected override string GetGeneratorSourceFilePath() => SourceFilePath;

        /// <summary>
        /// Реализация фабричного метода: создает экземпляр DetailedXmlReport.
        /// </summary>
        /// <returns>Новый экземпляр DetailedXmlReport.</returns>
        protected override IFieldReportProduct CreateReport()
        {
            Logger.Instance.Debug(SourceFilePath, "Фабричный метод CreateReport(): Создание DetailedXmlReport...");
            return new DetailedXmlReport();
        }
    }
}