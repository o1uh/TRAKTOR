using Traktor.DataModels;

namespace Traktor.Interfaces
{
    public interface IComputerVisionSystem
    {
        /// <summary>
        /// ������������ ���������� ����������� �� ����.
        /// ������� ���� �������� �� ��������� ������ �� ����� ��������.
        /// </summary>
        /// <param name="currentTractorPosition">������� ��������� ��������.</param>
        /// <returns>������ ������������ ����������� <see cref="ObstacleData"/>.</returns>
        List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition);

        /// <summary>
        /// ����������� ������� ���� ���� ��� ����������� ������������� ������������ (��������, ������� ��������).
        /// ������� ���� �������� �� ��������� ������ �� ����� ��������.
        /// </summary>
        /// <param name="currentTractorPosition">������� ��������� �������� (��� �������� ��������� ������������).</param>
        /// <returns>������ ������������ ������������ ���� <see cref="FieldFeatureData"/>.</returns>
        List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition); // ����� areaOfInterest
    }
}
