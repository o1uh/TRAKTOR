using Traktor.Core; 

namespace Traktor.Prototypes
{
    /// <summary>
    /// ����������� ����� ��������� (Prototype).
    /// ��������� ��������� ��� ������������ ������ ����.
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
        /// ����������� ����� ������������.
        /// </summary>
        /// <returns>���� �������� �������.</returns>
        public abstract WorkOrderPrototype Clone();

        /// <summary>
        /// ��������������� ����� ��� ����������� (�����������) ���������� � ������.
        /// </summary>
        public virtual void DisplayOrderDetails(string sourcePathForLog)
        {
            Logger.Instance.Info(sourcePathForLog,
                $"������ ������ (ID: {OrderId}): ����='{FieldName}', ����='{ScheduledDate:yyyy-MM-dd}'. ��� ������: {this.GetType().Name}");
        }
    }
}