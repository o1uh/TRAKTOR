using Traktor.Interfaces; 
using Traktor.Core;       
namespace Traktor.ReportProducts
{
    /// <summary>
    /// Конкретный Продукт: Простой текстовый отчет.
    /// </summary>
    public class SimpleTextReport : IFieldReportProduct
    {
        private const string SourceFilePath = "ReportProducts/SimpleTextReport.cs";
        private const string ReportType = "Простой Текстовый Отчет";

        public SimpleTextReport()
        {
            Logger.Instance.Info(SourceFilePath, $"Создан экземпляр '{ReportType}'.");
        }

        public void DisplayFormat()
        {
            string content = $"--- {ReportType} ---\nСостояние поля: Удовлетворительное.\nРекомендации: Продолжать мониторинг.\n--------------------";
            Logger.Instance.Info(SourceFilePath, $"Отображение отчета:\n{content}");
        }

        public string GetReportType()
        {
            return ReportType;
        }
    }
}