// Interfaces/IComputerVisionSystem.cs
namespace TractorAutopilot.Interfaces
{
    public interface IComputerVisionSystem
    {
        /// <summary>
        /// ������������ ����������� � ���� ������.
        /// </summary>
        /// <returns>������ ��������� �����������.</returns>
        Obstacle[] DetectObstacles();

        /// <summary>
        /// ����������� ��������� ���� (��������, ����� � �.�.).
        /// </summary>
        /// <returns>���������� � ��������� ����.</returns>
        FieldAnalysisData AnalyzeField();
    }

    // ��������������� ��������� ��� ������������� ������
    public struct Obstacle
    {
        public double X;
        public double Y;
        public double Z;
    }

    public struct FieldAnalysisData
    {
        public double AveragePlantHeight;
        public double SoilMoisture;
        // ������ ��������� ������� ����
    }
}