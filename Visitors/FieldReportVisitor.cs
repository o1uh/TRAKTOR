using Traktor.Interfaces; 
using Traktor.FieldElements; 
using Traktor.Core;          
using System.Text;          

namespace Traktor.Visitors
{
    /// <summary>
    /// Конкретный Посетитель, который "собирает" информацию для отчета о состоянии поля,
    /// проходя по его элементам. В данном макете он просто логирует информацию.
    /// </summary>
    public class FieldReportVisitor : IVisitor
    {
        private const string SourceFilePath = "Visitors/FieldReportVisitor.cs";
        private StringBuilder _reportContent; // Для накопления отчета, если нужно больше чем просто логи

        public FieldReportVisitor()
        {
            _reportContent = new StringBuilder();
            Logger.Instance.Info(SourceFilePath, "FieldReportVisitor создан. Готов к сбору информации для отчета.");
        }

        /// <summary>
        /// Посещает элемент типа ObstacleElement и добавляет информацию о нем в отчет.
        /// </summary>
        /// <param name="obstacleElement">Посещаемый элемент препятствия.</param>
        public void VisitObstacleElement(ObstacleElement obstacleElement)
        {
            string info = $"[ОТЧЕТ] Обнаружено препятствие: Тип='{obstacleElement.ObstacleType}', Позиция={obstacleElement.Position}. ({obstacleElement.GetDescription()})";
            Logger.Instance.Info(SourceFilePath, info);
            _reportContent.AppendLine(info);
        }

        /// <summary>
        /// Посещает элемент типа SoilPatchElement и добавляет информацию о нем в отчет.
        /// </summary>
        /// <param name="soilPatchElement">Посещаемый элемент участка почвы.</param>
        public void VisitSoilPatchElement(SoilPatchElement soilPatchElement)
        {
            string info = $"[ОТЧЕТ] Информация об участке почвы: Тип='{soilPatchElement.SoilType}', Влажность={soilPatchElement.Moisture}%. ({soilPatchElement.GetDescription()})";
            Logger.Instance.Info(SourceFilePath, info);
            _reportContent.AppendLine(info);
        }

        /// <summary>
        /// Возвращает собранный отчет (в данном макете это просто для примера).
        /// Основная информация логируется.
        /// </summary>
        /// <returns>Строка с отчетом.</returns>
        public string GetGeneratedReport()
        {
            Logger.Instance.Info(SourceFilePath, "Запрошен сгенерированный отчет FieldReportVisitor.");
            return _reportContent.ToString();
        }
    }
}