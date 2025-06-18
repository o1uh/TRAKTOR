namespace Traktor.Interfaces
{
    /// <summary>
    /// Интерфейс Продукта (Product) для паттерна Фабричный метод.
    /// Представляет некоторый отчет о состоянии поля.
    /// </summary>
    public interface IFieldReportProduct
    {
        /// <summary>
        /// "Формирует" или "отображает" содержимое отчета.
        /// </summary>
        void DisplayFormat();

        /// <summary>
        /// Получает тип отчета.
        /// </summary>
        string GetReportType();
    }
}