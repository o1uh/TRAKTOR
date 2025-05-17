using Traktor.DataModels;           // ��� Coordinates � FieldBoundaries

namespace Traktor.Interfaces
{
    public interface INavigationSystem
    {
        /// <summary>
        /// �������� ������� ���������� ��������.
        /// </summary>
        /// <returns>��������� <see cref="Coordinates"/> � �������� ������������.</returns>
        Coordinates GetPosition();

        /// <summary>
        /// ������������ ������� ��� ������������������ ����� (���������).
        /// </summary>
        /// <param name="startPosition">��������� ����� �������� (������� ���������� ��������).</param>
        /// <param name="targetPosition">�������� ����� ��������.</param>
        /// <param name="boundaries">�����������, ������� ����, � �������� ������� ������ ���� �������� �������.</param>
        /// <returns>������ <see cref="Coordinates"/>, �������������� �������, ��� null/������ ������, ���� ������� �� ����� ���� ��������.</returns>
        List<Coordinates> CalculateRoute(Coordinates startPosition, Coordinates targetPosition, FieldBoundaries boundaries = null);

        /// <summary>
        /// ������������ ������������ ������� �� ������ ���������� �� ������������ ������������.
        /// ���������� ����� ����������������� �������.
        /// </summary>
        /// <param name="currentRoute">������� �������, ������� ����� ���������������.</param>
        /// <param name="detectedObstacles">������ ������������ �����������.</param>
        /// <returns>����� ������ <see cref="Coordinates"/>, �������������� ����������������� �������, ��� null/������ ������, ���� ������������� �� ������� ��� �� ���������.</returns>
        List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles);

        /// <summary>
        /// ������������� ������� ���������.
        /// </summary>
        void StopNavigation();
    }
}