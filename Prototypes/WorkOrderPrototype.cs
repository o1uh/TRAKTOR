using Traktor.Core; 

namespace Traktor.Prototypes
{
    /// <summary>
    /// Абстрактный класс Прототипа (Prototype).
    /// Объявляет интерфейс для клонирования самого себя.
    /// </summary>
    public abstract class WorkOrderPrototype
    {
        public string OrderId { get; set; }
        public string FieldName { get; set; }
        public DateTime ScheduledDate { get; set; }

        protected WorkOrderPrototype(string orderId, string fieldName, DateTime scheduledDate)
        {
            OrderId = orderId;
            FieldName = fieldName;
            ScheduledDate = scheduledDate;
        }

        /// <summary>
        /// Абстрактный метод клонирования.
        /// </summary>
        /// <returns>Клон текущего объекта.</returns>
        public abstract WorkOrderPrototype Clone();

        /// <summary>
        /// Вспомогательный метод для отображения (логирования) информации о заказе.
        /// </summary>
        public virtual void DisplayOrderDetails(string sourcePathForLog)
        {
            Logger.Instance.Info(sourcePathForLog,
                $"Детали Заказа (ID: {OrderId}): Поле='{FieldName}', Дата='{ScheduledDate:yyyy-MM-dd}'. Тип заказа: {this.GetType().Name}");
        }
    }
}