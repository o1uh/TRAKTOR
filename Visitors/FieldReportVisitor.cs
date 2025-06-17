using Traktor.Interfaces; 
using Traktor.FieldElements; 
using Traktor.Core;          
using System.Text;          

namespace Traktor.Visitors
{
    /// <summary>
    /// ���������� ����������, ������� "��������" ���������� ��� ������ � ��������� ����,
    /// ������� �� ��� ���������. � ������ ������ �� ������ �������� ����������.
    /// </summary>
    public class FieldReportVisitor : IVisitor
    {
        private const string SourceFilePath = "Visitors/FieldReportVisitor.cs";
        private StringBuilder _reportContent; // ��� ���������� ������, ���� ����� ������ ��� ������ ����

        public FieldReportVisitor()
        {
            _reportContent = new StringBuilder();
            Logger.Instance.Info(SourceFilePath, "FieldReportVisitor ������. ����� � ����� ���������� ��� ������.");
        }

        /// <summary>
        /// �������� ������� ���� ObstacleElement � ��������� ���������� � ��� � �����.
        /// </summary>
        /// <param name="obstacleElement">���������� ������� �����������.</param>
        public void VisitObstacleElement(ObstacleElement obstacleElement)
        {
            string info = $"[�����] ���������� �����������: ���='{obstacleElement.ObstacleType}', �������={obstacleElement.Position}. ({obstacleElement.GetDescription()})";
            Logger.Instance.Info(SourceFilePath, info);
            _reportContent.AppendLine(info);
        }

        /// <summary>
        /// �������� ������� ���� SoilPatchElement � ��������� ���������� � ��� � �����.
        /// </summary>
        /// <param name="soilPatchElement">���������� ������� ������� �����.</param>
        public void VisitSoilPatchElement(SoilPatchElement soilPatchElement)
        {
            string info = $"[�����] ���������� �� ������� �����: ���='{soilPatchElement.SoilType}', ���������={soilPatchElement.Moisture}%. ({soilPatchElement.GetDescription()})";
            Logger.Instance.Info(SourceFilePath, info);
            _reportContent.AppendLine(info);
        }

        /// <summary>
        /// ���������� ��������� ����� (� ������ ������ ��� ������ ��� �������).
        /// �������� ���������� ����������.
        /// </summary>
        /// <returns>������ � �������.</returns>
        public string GetGeneratedReport()
        {
            Logger.Instance.Info(SourceFilePath, "�������� ��������������� ����� FieldReportVisitor.");
            return _reportContent.ToString();
        }
    }
}