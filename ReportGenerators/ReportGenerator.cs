using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.ReportGenerators
{
    /// <summary>
    /// ����������� ��������� (Creator) � �������� ��������� �����.
    /// ��������� ��������� ����� ��� �������� ��������� (�������).
    /// </summary>
    public abstract class ReportGenerator
    {
        /// <summary>
        /// ��������� �����, ������� ������ ���� ���������� �����������
        /// ��� �������� ����������� ���������� ������.
        /// </summary>
        /// <returns>��������� ��������� IFieldReportProduct.</returns>
        protected abstract IFieldReportProduct CreateReport();
        
        /// <summary>
        /// �������� �������� ���������, ������� ���������� �������, 
        /// ��������� ��������� �������.
        /// </summary>
        public void GenerateAndDisplayReport()
        {
            string sourcePath = GetGeneratorSourceFilePath();
            Logger.Instance.Info(sourcePath, $"������ ��������� � ����������� ������ � ������� {this.GetType().Name}.");

            IFieldReportProduct report = CreateReport();
            Logger.Instance.Info(sourcePath, $"��������� ����� CreateReport() � {this.GetType().Name} ������ ����� ���� '{report.GetReportType()}'.");

            report.DisplayFormat();

            Logger.Instance.Info(sourcePath, $"��������� � ����������� ������ � ������� {this.GetType().Name} ���������.");
        }

        /// <summary>
        /// ��������������� ����������� �����, ����� ���������� ������������ ���� SourceFilePath.
        /// </summary>
        protected abstract string GetGeneratorSourceFilePath();
    }
}