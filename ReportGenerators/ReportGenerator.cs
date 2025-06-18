using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.ReportGenerators
{
    /// <summary>
    /// Абстрактный Создатель (Creator) в паттерне Фабричный метод.
    /// Объявляет фабричный метод для создания Продуктов (отчетов).
    /// </summary>
    public abstract class ReportGenerator
    {
        /// <summary>
        /// Фабричный метод, который должен быть реализован подклассами
        /// для создания конкретного экземпляра отчета.
        /// </summary>
        /// <returns>Созданный экземпляр IFieldReportProduct.</returns>
        protected abstract IFieldReportProduct CreateReport();
        
        /// <summary>
        /// Основная операция Создателя, которая использует Продукт, 
        /// созданный фабричным методом.
        /// </summary>
        public void GenerateAndDisplayReport()
        {
            string sourcePath = GetGeneratorSourceFilePath();
            Logger.Instance.Info(sourcePath, $"Начало генерации и отображения отчета с помощью {this.GetType().Name}.");

            IFieldReportProduct report = CreateReport();
            Logger.Instance.Info(sourcePath, $"Фабричный метод CreateReport() в {this.GetType().Name} создал отчет типа '{report.GetReportType()}'.");

            report.DisplayFormat();

            Logger.Instance.Info(sourcePath, $"Генерация и отображение отчета с помощью {this.GetType().Name} завершены.");
        }

        /// <summary>
        /// Вспомогательный абстрактный метод, чтобы наследники предоставили свой SourceFilePath.
        /// </summary>
        protected abstract string GetGeneratorSourceFilePath();
    }
}