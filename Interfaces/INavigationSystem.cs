using Traktor.DataModels;

namespace Traktor.Interfaces
{
    public interface INavigationSystem
    {
        /// <summary>
        /// �������� ������� �������������� �������� �� ������������� �������.
        /// </summary>
        /// <returns>������� ���������� <see cref="Coordinates"/>.</returns>
        Coordinates GetPosition();

        /// <summary>
        /// ������������ �������. ���� ����� ����� ���� ������ ����� StartNavigation
        /// ��� ��� ����������� ������������� ��������.
        /// </summary>
        /// <param name="startPosition">��������� ����� ��������.</param>
        /// <param name="targetPosition">�������� ����� ��������.</param>
        /// <param name="boundaries">������������ ������� ����.</param>
        /// <param name="precisionPoints">���������� ������������� ����� ��� ��������� �� ��������� ��������.</param>
        /// <returns>������ <see cref="Coordinates"/>, �������������� �������. ����� ������� ������ ������ ��� null, ���� ������� �� ��������.</returns>
        List<Coordinates> CalculateRoute(Coordinates startPosition, Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3);

        /// <summary>
        /// ������������ ������������ ������� �� ������ ���������� �� ������������ ������������.
        /// </summary>
        /// <param name="currentRoute">������� ������� ��� �������������.</param>
        /// <param name="detectedObstacles">������ ������������ �����������.</param>
        /// <returns>
        /// ����� (�����������������) ������ <see cref="Coordinates"/>.
        /// ���� ������������� ����������, ���������� null.
        /// ���� ������������� �� ���������, ����� ������� ����� currentRoute.
        /// </returns>
        List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles);

        /// <summary>
        /// ������������ ������������� ������� (��������� � "������ �����" ��� ��������� ������).
        /// </summary>
        void StopNavigation();

        /// <summary>
        /// ���������� ������������� ������� � ����� ������������ �������������� ������ ��������.
        /// </summary>
        /// <param name="initialStartPosition">��������� ����� ��� ������� ������� ��������.</param>
        /// <param name="initialTargetPosition">�������� ����� ��� ������� ������� ��������.</param>
        /// <param name="initialBoundaries">������������ ������� ���� ��� ������� �������.</param>
        /// <param name="initialPrecisionPoints">�������� ��� ������� �������.</param>
        /// <returns>������������� ������������ ������� ��� null/������ ������, ���� ������ �� ������.</returns>
        List<Coordinates> StartNavigation(Coordinates initialStartPosition, Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3);

        /// <summary>
        /// ��������������� ����� ��� ������: ��������� ����� ���������� ������� �������������� �������.
        /// </summary>
        void UpdateSimulatedPosition(Coordinates newPosition);
    }
}