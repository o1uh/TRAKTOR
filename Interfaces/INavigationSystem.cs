// Interfaces/INavigationSystem.cs
namespace TractorAutopilot.Interfaces
{
    public interface INavigationSystem
    {
        /// <summary>
        /// ���������� ������� �������������� ��������.
        /// </summary>
        /// <returns>������� �������������� ��������.</returns>
        Location GetPosition();

        /// <summary>
        /// ��������� ����������� ������� �������� ��������.
        /// </summary>
        /// <param name="destination">�������� ����� ��������.</param>
        /// <returns>������� ��������.</returns>
        Route CalculateRoute(Location destination);

        /// <summary>
        /// ������������ ������� ������� ��������.
        /// </summary>
        /// <returns>����������������� ������� ��������.</returns>
        Route AdjustRoute();
    }

    // ��������������� ��������� ��� ������������� ������
    public struct Location
    {
        public double Latitude;
        public double Longitude;
    }

    public struct Route
    {
        public Location[] Waypoints;
    }
}