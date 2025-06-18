using System; 
using Traktor.Core; 

namespace Traktor.Prototypes
{
    /// <summary>
    /// Конкретный Прототип: Заказ на вспашку поля.
    /// </summary>
    public class FieldPloughingOrder : WorkOrderPrototype
    {
        private const string SourceFilePath = "Prototypes/FieldPloughingOrder.cs";

        /// <summary>
        /// Специфичное свойство для этого типа заказа.
        /// </summary>
        public double PloughingDepth { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр заказа на вспашку.
        /// </summary>
        public FieldPloughingOrder(string orderId, string fieldName, DateTime scheduledDate, double ploughingDepth)
            : base(orderId, fieldName, scheduledDate)
        {
            PloughingDepth = ploughingDepth;
            Logger.Instance.Info(SourceFilePath, $"Создан FieldPloughingOrder (ID: {OrderId}), Поле: '{FieldName}', Глубина: {PloughingDepth}м.");
        }

        /// <summary>
        /// Создает поверхностную копию (shallow copy) текущего объекта.
        /// Для глубокого копирования ссылочных типов потребовалась бы дополнительная логика.
        /// </summary>
        /// <returns>Клон объекта FieldPloughingOrder.</returns>
        public override WorkOrderPrototype Clone()
        {
            Logger.Instance.Info(SourceFilePath, $"Клонирование FieldPloughingOrder (ID: {this.OrderId})...");

            var clone = (FieldPloughingOrder)this.MemberwiseClone();

            Logger.Instance.Info(SourceFilePath, $"FieldPloughingOrder (ID: {this.OrderId}) успешно склонирован. Новый объект (или тот же, если ID не менялся): {clone.OrderId}.");
            return clone;
        }

        /// <summary>
        /// Переопределенный метод для отображения деталей, включая специфичные для вспашки.
        /// </summary>
        public override void DisplayOrderDetails(string sourcePathForLog)
        {
            Logger.Instance.Info(sourcePathForLog, 
                $"Детали Заказа на ВСПАШКУ (ID: {OrderId}): Поле='{FieldName}', Дата='{ScheduledDate:yyyy-MM-dd}', Глубина вспашки={PloughingDepth}м.");
        }
    }
}