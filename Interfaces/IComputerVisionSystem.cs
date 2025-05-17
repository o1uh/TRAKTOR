using Traktor.DataModels;

namespace Traktor.Interfaces
{
    public interface IComputerVisionSystem
    {
        /// <summary>
        /// ������������ ����������� �� ����.
        /// </summary>
        /// <param name="sensorInput">������������ "�����" ������ � �������.</param>
        /// <returns>
        /// ������ <see cref="ObstacleData"/> ������������ �����������.
        /// ������ ������, ���� ����������� ���.
        /// </returns>
        List<ObstacleData> DetectObstacles(object sensorInput = null);

        /// <summary>
        /// ����������� ��������� ��������� �����/�������� ����.
        /// </summary>
        /// <param name="targetCoordinates">������ ��������� �����/�������� ��� �������. ���� null ��� ����, ����� ������������� ������� ���� ���������.</param>
        /// <param name="sensorInput">������������ "�����" ������ � �������.</param>
        /// <returns>
        /// ������ <see cref="FieldPointAnalysis"/> � ������������ ������� ��� ������ ����������� �����/�������.
        /// ����� ������� ������ ������, ���� ������ �� ��� �����������.
        /// </returns>
        List<FieldPointAnalysis> AnalyzeFieldPoints(List<Coordinates> targetCoordinates = null, object sensorInput = null);
    }
}
