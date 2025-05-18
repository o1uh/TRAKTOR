using Traktor.DataModels;      // ��� Coordinates, FieldBoundaries, ObstacleData

namespace Traktor.Interfaces
{
    /// <summary>
    /// ���������� �������� ��� ������� ��������� ��������.
    /// </summary>
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
        /// <param name="boundaries">������������ ������� ����, � �������� ������� ������ ���� �������� �������.</param>
        /// <param name="precisionPoints">
        /// ���������� ������������� ����� ��� ��������� �� ������������� ��������� ��������.
        /// ����������� ����������� ��������.
        /// </param>
        /// <returns>
        /// ������ <see cref="Coordinates"/>, �������������� ������������ �������.
        /// ����� ������� ������ ������ ��� null, ���� ������� �� ����� ���� ��������.
        /// </returns>
        List<Coordinates> CalculateRoute(Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3);

        /// <summary>
        /// ������������ ������������ ������� �� ������ ���������� �� ������������ ������������.
        /// </summary>
        /// <param name="currentRoute">������� ������� (������ <see cref="Coordinates"/>) ��� �������������.</param>
        /// <param name="detectedObstacles">������ ������������ ����������� <see cref="ObstacleData"/>.</param>
        /// <returns>
        /// ����� (�����������������) ������ <see cref="Coordinates"/>.
        /// ���� ������������� ����������, ����� ������� null ��� ������ ������.
        /// ���� ������������� �� ���������, ����� ������� ����� <paramref name="currentRoute"/> ��� ��� <paramref name="currentRoute"/>.
        /// </returns>
        List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles);

        /// <summary>
        /// ���������� ������������� �������.
        /// ����� ������ ����� ������ ������� ������ � ������, �� ������� ��� �� ��������� (���� �� �������������� ����������).
        /// </summary>
        void StartNavigation();

        /// <summary>
        /// ���������� ������������� ������� � ���������� �������������� ������ ��������
        /// �� �������� ��������� ������� �� ��������� ����.
        /// </summary>
        /// <param name="initialTargetPosition">�������� ����� ��� ��������������� ������� ��������.</param>
        /// <param name="initialBoundaries">������������ ������� ���� ��� ��������������� �������.</param>
        /// <param name="initialPrecisionPoints">�������� ����������� ��� ��������������� ������� ��������.</param>
        /// <returns>
        /// ������������� ������������ ������� (������ <see cref="Coordinates"/>).
        /// ����� ������� ������ ������ ��� null, ���� ������/������ �� ������.
        /// </returns>
        List<Coordinates> StartNavigation(Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3);

        /// <summary>
        /// ������������ ������������� ������� (��������, ��������� ������ � "������ �����" ��� ��������� ���).
        /// </summary>
        void StopNavigation();

        /// <summary>
        /// ��������������� ����� ��� ��������� � ������������:
        /// ��������� ����� ���������� ��� �������� ������� �������������� ������� ������������� �������.
        /// </summary>
        /// <param name="newPosition">����� �������������� ���������� <see cref="Coordinates"/>.</param>
        void UpdateSimulatedPosition(Coordinates newPosition);
    }
}