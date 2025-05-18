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
        /// ������������ ������� �� �������� ����������� ��������� ������� �� ������� �����.
        /// </summary>
        /// <param name="targetPosition">�������� ����� ��������.</param>
        /// <param name="boundaries">������������ ������� ����.</param>
        /// <param name="precisionPoints">���������� ������������� ����� ��� ��������� �� ��������� ��������.</param>
        /// <returns>������ <see cref="Coordinates"/>, �������������� �������. ����� ������� ������ ������ ��� null, ���� ������� �� ��������.</returns>
        List<Coordinates> CalculateRoute(Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3);

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
        /// ���������� ������������� �������, ���������� �� � ��������� ��������� �������
        /// � ���������� �������������� ������ ��������.
        /// </summary>
        /// <param name="systemInitial��������Position">��������� ������� ��� �������� ������� � ������� ������� ��������.</param>
        /// <param name="initialTargetPosition">�������� ����� ��� ������� ������� ��������.</param>
        /// <param name="initialBoundaries">������������ ������� ���� ��� ������� �������.</param>
        /// <param name="initialPrecisionPoints">�������� ��� ������� �������.</param>
        /// <returns>������������� ������������ ������� ��� null/������ ������, ���� ������/������ �� ������.</returns>
        List<Coordinates> StartNavigation(Coordinates systemInitial��������Position, Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3);

        /// <summary>
        /// ������������ ������������� ������� (��������� � "������ �����" ��� ��������� ������).
        /// </summary>
        void StopNavigation();

        /// <summary>
        /// ��������������� ����� ��� ������: ��������� ����� ���������� ������� �������������� �������.
        /// </summary>
        void UpdateSimulatedPosition(Coordinates newPosition);
    }
}