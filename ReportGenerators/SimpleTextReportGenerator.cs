using Traktor.Interfaces;     
using Traktor.ReportProducts; 
using Traktor.Core;           

namespace Traktor.ReportGenerators
{
    /// <summary>
    /// Конкретный Создатель (ConcreteCreator), который создает SimpleTextReport.
    /// </summary>
    public class SimpleTextReportGenerator : ReportGenerator
    {
        private const string SourceFilePath = "ReportGenerators/SimpleTextReportGenerator.cs";

        public SimpleTextReportGenerator()
        {
            Logger.Instance.Info(SourceFilePath, "SimpleTextReportGenerator создан.");
        }

        protected override string GetGeneratorSourceFilePath() => SourceFilePath;

        /// <summary>
        /// Реализация фабричного метода: создает экземпляр SimpleTextReport.
        /// </summary>
        /// <returns>Новый экземпляр SimpleTextReport.</returns>
        protected override IFieldReportProduct CreateReport()
        {
            Logger.Instance.Debug(SourceFilePath, "Фабричный метод CreateReport(): Создание SimpleTextReport...");
            return new SimpleTextReport();
        }
    }
}