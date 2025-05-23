using Traktor.DataModels;      // ��� Coordinates, ObstacleData, FieldFeatureData

namespace Traktor.Interfaces
{
    /// <summary>
    /// ���������� �������� ��� ������� ������������� ������.
    /// ������� �������� �� ������ ���������� ���������� � ������ � LiDAR.
    /// </summary>
    public interface IComputerVisionSystem
    {
        /// <summary>
        /// ������������ ���������� ����������� � ���� ��������� ��� �������� �������� �������.
        /// ������� �������������� �������� ������ �� ����� ����� �/��� LiDAR.
        /// </summary>
        /// <param name="currentTractorPosition">
        /// ������� ��������� ��������. ������������ ��� ����������� ������� ������������
        /// � ���������� �������� ��������� ������������ �����������.
        /// </param>
        /// <returns>
        /// ������ ������������ ����������� <see cref="ObstacleData"/>.
        /// ���� ����������� �� ����������, ���������� ������ ������.
        /// </returns>
        List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition);

        /// <summary>
        /// ����������� ������� ���� ���� ��� ������������� ������������� �������������� ������������,
        /// ����� ��� �������, ������� ��������, ��������� ����� �� ����� � �.�.
        /// ���� ����� ������ ����������� ��������� �� ������ �����.
        /// </summary>
        /// <param name="currentTractorPosition">
        /// ������� ��������� ��������. ������������ ��� ����������� ������� �������
        /// � ���������� �������� ��������� ������������ ������������.
        /// </param>
        /// <returns>
        /// ������ ������������ ������������ ���� <see cref="FieldFeatureData"/>.
        /// ���� ������������ �� ����������, ���������� ������ ������.
        /// </returns>
        List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition);
    }
}