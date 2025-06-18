using System; 
using Traktor.Core; 

namespace Traktor.Prototypes
{
    /// <summary>
    /// ���������� ��������: ����� �� ������� ����.
    /// </summary>
    public class FieldPloughingOrder : WorkOrderPrototype
    {
        private const string SourceFilePath = "Prototypes/FieldPloughingOrder.cs";

        /// <summary>
        /// ����������� �������� ��� ����� ���� ������.
        /// </summary>
        public double PloughingDepth { get; set; }

        /// <summary>
        /// �������������� ����� ��������� ������ �� �������.
        /// </summary>
        public FieldPloughingOrder(string orderId, string fieldName, DateTime scheduledDate, double ploughingDepth)
            : base(orderId, fieldName, scheduledDate)
        {
            PloughingDepth = ploughingDepth;
            Logger.Instance.Info(SourceFilePath, $"������ FieldPloughingOrder (ID: {OrderId}), ����: '{FieldName}', �������: {PloughingDepth}�.");
        }

        /// <summary>
        /// ������� ������������� ����� (shallow copy) �������� �������.
        /// ��� ��������� ����������� ��������� ����� ������������� �� �������������� ������.
        /// </summary>
        /// <returns>���� ������� FieldPloughingOrder.</returns>
        public override WorkOrderPrototype Clone()
        {
            Logger.Instance.Info(SourceFilePath, $"������������ FieldPloughingOrder (ID: {this.OrderId})...");

            var clone = (FieldPloughingOrder)this.MemberwiseClone();

            Logger.Instance.Info(SourceFilePath, $"FieldPloughingOrder (ID: {this.OrderId}) ������� �����������. ����� ������ (��� ��� ��, ���� ID �� �������): {clone.OrderId}.");
            return clone;
        }

        /// <summary>
        /// ���������������� ����� ��� ����������� �������, ������� ����������� ��� �������.
        /// </summary>
        public override void DisplayOrderDetails(string sourcePathForLog)
        {
            Logger.Instance.Info(sourcePathForLog, 
                $"������ ������ �� ������� (ID: {OrderId}): ����='{FieldName}', ����='{ScheduledDate:yyyy-MM-dd}', ������� �������={PloughingDepth}�.");
        }
    }
}