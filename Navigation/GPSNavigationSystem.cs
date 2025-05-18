using Traktor.Interfaces;
using Traktor.DataModels;
using Traktor.Core;   // ��������� ��� Logger

namespace Traktor.Navigation
{
    /// <summary>
    /// ���������� ������� ��������� �� ������ GPS.
    /// ��������� ������ GPS-������, ������� ��������� ���������, ������ � ������������� ��������.
    /// �������� �������� �������� ���������.
    /// </summary>
    public class GPSNavigationSystem : INavigationSystem
    {
        private Coordinates _currentSimulatedPosition;
        private bool _isActive = false;
        private static readonly Random _random = new Random(); // ���� ��������� Random ��� ����� ������

        private const double GPS_NOISE_MAGNITUDE = 0.000005; // ��������� ��� ��� �������� ���������� GPS
        private const string SourceFilePath = "Navigation/GPSNavigationSystem.cs"; // ���������� ���������

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="GPSNavigationSystem"/>.
        /// ��������� ������� ��������������� � (0,0), ������� ���������.
        /// </summary>
        public GPSNavigationSystem()
        {
            _currentSimulatedPosition = new Coordinates(0, 0); // �������� �� ���������
            _isActive = false;
            Logger.Instance.Info(SourceFilePath, $"������� GPS ��������� ����������������. ������� �� ���������: {_currentSimulatedPosition}. ������� �� �������.");
        }

        /// <inheritdoc/>
        public Coordinates GetPosition()
        {
            string activeStatus = _isActive ? "�������" : "�� �������";
            // �������� ������ ���� ���-�� ������ (��������, ������� �� �������, �� �������� �������� �������)
            // ��� ��� ��������� �������. ���� ������� �������� ���.
            // Logger.Instance.Debug(SourceFilePath, $"GetPosition ������. ������� {activeStatus}."); // ����������������, ����� �� �������

            // �������� ���������� ����/����������� GPS
            Coordinates reportedPosition = new Coordinates(
                _currentSimulatedPosition.Latitude + (_random.NextDouble() - 0.5) * GPS_NOISE_MAGNITUDE,
                _currentSimulatedPosition.Longitude + (_random.NextDouble() - 0.5) * GPS_NOISE_MAGNITUDE
            );
            // ��������� ��� ����� �������, ���� GetPosition ���������� �����. ������� ��� ������������������ ��� ����������.
            // Logger.Instance.Debug(SourceFilePath, $"GetPosition. ��������������: {_currentSimulatedPosition}, ������������ � �����: {reportedPosition}"); // ����������������, ����� �� �������
            return reportedPosition;
        }

        /// <inheritdoc/>
        public List<Coordinates> CalculateRoute(Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3)
        {
            if (!_isActive)
            {
                Logger.Instance.Warning(SourceFilePath, $"CalculateRoute: ������� �� �������, ������ �������� ����������.");
                return null; // ��� ������ ������: new List<Coordinates>();
            }

            Coordinates startPosition = this.GetPosition(); // �������� "��������" ������� � �����
            Logger.Instance.Info(SourceFilePath, $"CalculateRoute: ������ �������� �� {startPosition} �� {targetPosition}. ������������� ����� �� �������: {precisionPoints}. ������� ����: {(boundaries == null ? "�� ������" : "������")}.");

            List<Coordinates> route = new List<Coordinates>();
            route.Add(startPosition); // ��������� ����� - ��� "������� �������"

            // ������� �������� ������������ ��� ��������
            // precisionPoints - ��� ���������� �������������� ����� ����� ������� � ������ ��������.
            // ���� precisionPoints = 0, �� ������ start � target.
            // ���� precisionPoints = 1, �� start, middle, target (����� 1+1 = 2 ��������)
            // ����� ���������� ��������� = precisionPoints + 1
            int totalSegments = Math.Max(1, precisionPoints + 1); // ���� �� ���� �������

            for (int i = 1; i < totalSegments; i++) // ��������� precisionPoints �����
            {
                double fraction = (double)i / totalSegments;
                route.Add(new Coordinates(
                   startPosition.Latitude + (targetPosition.Latitude - startPosition.Latitude) * fraction,
                   startPosition.Longitude + (targetPosition.Longitude - startPosition.Longitude) * fraction
               ));
            }
            route.Add(targetPosition); // �������� �����

            // ����� ����� �� ���� �������� �� ����� �� boundaries, ���� ��� ������.
            // ��� ������ ���� ��������.

            Logger.Instance.Info(SourceFilePath, $"CalculateRoute: ������� ���������, {route.Count} �����.");
            return route; // ���������� ��� ������, ��� ����� � new List<Coordinates>(route) �����
        }

        /// <inheritdoc/>
        public List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles)
        {
            if (!_isActive)
            {
                Logger.Instance.Warning(SourceFilePath, $"AdjustRoute: ������� �� �������, ������������� ����������.");
                return null;
            }

            int currentRouteCount = currentRoute?.Count ?? 0;
            int obstaclesCount = detectedObstacles?.Count ?? 0;
            Logger.Instance.Info(SourceFilePath, $"AdjustRoute: ������ �� ������������� �������� ({currentRouteCount} �����) ��-�� {obstaclesCount} �����������.");

            if (currentRoute == null || !currentRoute.Any())
            {
                Logger.Instance.Warning(SourceFilePath, $"AdjustRoute: ��� ������������� �������� ��� �������������.");
                return null;
            }

            if (detectedObstacles == null || !detectedObstacles.Any())
            {
                Logger.Instance.Info(SourceFilePath, $"AdjustRoute: ������������� �� ��������� (����������� ���). ���������� ����� �������� ��������.");
                return new List<Coordinates>(currentRoute); // ���������� �����, ����� �������� ����������� ��������� ���������
            }

            // �������� ������� ������ �������������
            if (_random.Next(0, 5) == 0) // �������� ���� �������� �������������
            {
                Logger.Instance.Warning(SourceFilePath, $"AdjustRoute: ��������: ������������� �������� ����������.");
                return null;
            }

            List<Coordinates> adjustedRoute = new List<Coordinates>(currentRoute); // �������� � ������
            bool containsRock = detectedObstacles.Any(o => o.Description != null && o.Description.ToLower().Contains("������"));

            if (containsRock)
            {
                Logger.Instance.Info(SourceFilePath, $"AdjustRoute: ��������� '������', ��������� ������������� ������������� (��������� �����).");
                for (int i = 0; i < adjustedRoute.Count; i++)
                {
                    // ��������� "������"
                    adjustedRoute[i] = new Coordinates(
                        adjustedRoute[i].Latitude + (_random.NextDouble() - 0.3) * 0.00003, // ������� �����
                        adjustedRoute[i].Longitude + (_random.NextDouble() - 0.3) * 0.00003
                    );
                }
            }
            else
            {
                Logger.Instance.Info(SourceFilePath, $"AdjustRoute: ���������� ������ �����������, ��������� ����������� ������������� (��������� �����).");
                for (int i = 0; i < adjustedRoute.Count; i++)
                {
                    // ��� �������, ����� �����
                    adjustedRoute[i] = new Coordinates(
                        adjustedRoute[i].Latitude + (_random.NextDouble() - 0.5) * 0.00001,
                        adjustedRoute[i].Longitude + (_random.NextDouble() - 0.5) * 0.00001
                    );
                }
            }
            Logger.Instance.Info(SourceFilePath, $"AdjustRoute: ����������������� ������� ����� ({adjustedRoute.Count} �����).");
            return adjustedRoute;
        }

        /// <inheritdoc/>
        public void StartNavigation()
        {
            if (_isActive)
            {
                Logger.Instance.Info(SourceFilePath, $"StartNavigation: ������� GPS ��� �������.");
                return;
            }

            // �������� ����������� ���� ��� ��������� ������
            if (_random.Next(0, 10) == 0) // 10% ���� ����
            {
                _isActive = false;
                Logger.Instance.Error(SourceFilePath, $"StartNavigation: ������� ���������. GPS ������ �� ���� ����������� ���������.");
            }
            else
            {
                _isActive = true;
                Logger.Instance.Info(SourceFilePath, $"StartNavigation: ������� GPS ������� ������������. ������� �������������� �������: {_currentSimulatedPosition}.");
            }
        }

        /// <inheritdoc/>
        public List<Coordinates> StartNavigation(Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3)
        {
            Logger.Instance.Info(SourceFilePath, $"StartNavigation (� �����������): ������� ��������� ������� GPS � ������� �������� � {initialTargetPosition}.");

            this.StartNavigation(); // ������� �������� ������������ ������� (��� ���������, ��� ��� ��� �������)

            if (!_isActive)
            {
                Logger.Instance.Warning(SourceFilePath, $"StartNavigation (� �����������): ������� GPS �� ������� ����� ������� �������, �������������� ������ �������� ����������.");
                return null;
            }

            // ���� ������� �������, ������� �������������� ������� ��� ������ ���� ����������� ����� UpdateSimulatedPosition.
            // ���� ���, �� _currentSimulatedPosition ����� (0,0) ��� ��������� ��������� ��������.
            Logger.Instance.Info(SourceFilePath, $"StartNavigation (� �����������): ������� �������. ������� ������� ��� �������: {_currentSimulatedPosition}. ����������� �������������� ������ ��������...");

            List<Coordinates> calculatedRoute = CalculateRoute(initialTargetPosition, initialBoundaries, initialPrecisionPoints);

            if (calculatedRoute == null || !calculatedRoute.Any())
            {
                Logger.Instance.Warning(SourceFilePath, $"StartNavigation (� �����������): �������������� ������ �������� �� ������ ��� ������ ������ ������� (�������� �� �������� GPS).");
            }
            else
            {
                Logger.Instance.Info(SourceFilePath, $"StartNavigation (� �����������): �������������� ������� ������� ��������� ({calculatedRoute.Count} �����).");
            }
            return calculatedRoute;
        }


        /// <inheritdoc/>
        public void StopNavigation()
        {
            if (!_isActive)
            {
                Logger.Instance.Info(SourceFilePath, $"StopNavigation: ������� GPS ��� ���������.");
                return;
            }
            _isActive = false;
            Logger.Instance.Info(SourceFilePath, $"StopNavigation: ������� GPS ��������������.");
        }

        /// <inheritdoc/>
        public void UpdateSimulatedPosition(Coordinates newPosition)
        {
            _currentSimulatedPosition = newPosition;
            Logger.Instance.Info(SourceFilePath, $"UpdateSimulatedPosition: �������������� ������� GPS ��������� ��: {newPosition}.");
        }
    }
}