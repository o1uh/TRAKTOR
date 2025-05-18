using Traktor.Interfaces;
using Traktor.DataModels;

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

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="GPSNavigationSystem"/>.
        /// ��������� ������� ��������������� � (0,0), ������� ���������.
        /// </summary>
        public GPSNavigationSystem()
        {
            _currentSimulatedPosition = new Coordinates(0, 0); // �������� �� ���������
            _isActive = false;
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ������� GPS ��������� ����������������. ������� �� ���������: {_currentSimulatedPosition}. ������� �� �������.");
        }

        /// <inheritdoc/>
        public Coordinates GetPosition()
        {
            string activeStatus = _isActive ? "�������" : "�� �������";
            // �������� ������ ���� ���-�� ������ (��������, ������� �� �������, �� �������� �������� �������)
            // ��� ��� ��������� �������. ���� ������� �������� ���.
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: GetPosition ������. ������� {activeStatus}.");

            // �������� ���������� ����/����������� GPS
            Coordinates reportedPosition = new Coordinates(
                _currentSimulatedPosition.Latitude + (_random.NextDouble() - 0.5) * GPS_NOISE_MAGNITUDE,
                _currentSimulatedPosition.Longitude + (_random.NextDouble() - 0.5) * GPS_NOISE_MAGNITUDE
            );
            // ��������� ��� ����� �������, ���� GetPosition ���������� �����. ������� ��� ������������������ ��� ����������.
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: GetPosition. ��������������: {_currentSimulatedPosition}, ������������ � �����: {reportedPosition}");
            return reportedPosition;
        }

        /// <inheritdoc/>
        public List<Coordinates> CalculateRoute(Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3)
        {
            if (!_isActive)
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: CalculateRoute: ������� �� �������, ������ �������� ����������.");
                return null; // ��� ������ ������: new List<Coordinates>();
            }

            Coordinates startPosition = this.GetPosition(); // �������� "��������" ������� � �����
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: CalculateRoute: ������ �������� �� {startPosition} �� {targetPosition}. ������������� ����� �� �������: {precisionPoints}. ������� ����: {(boundaries == null ? "�� ������" : "������")}.");

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

            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: CalculateRoute: ������� ���������, {route.Count} �����.");
            return route; // ���������� ��� ������, ��� ����� � new List<Coordinates>(route) �����
        }

        /// <inheritdoc/>
        public List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles)
        {
            if (!_isActive)
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ������� �� �������, ������������� ����������.");
                return null;
            }

            int currentRouteCount = currentRoute?.Count ?? 0;
            int obstaclesCount = detectedObstacles?.Count ?? 0;
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ������ �� ������������� �������� ({currentRouteCount} �����) ��-�� {obstaclesCount} �����������.");

            if (currentRoute == null || !currentRoute.Any())
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ��� ������������� �������� ��� �������������.");
                return null;
            }

            if (detectedObstacles == null || !detectedObstacles.Any())
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ������������� �� ��������� (����������� ���). ���������� ����� �������� ��������.");
                return new List<Coordinates>(currentRoute); // ���������� �����, ����� �������� ����������� ��������� ���������
            }

            // �������� ������� ������ �������������
            if (_random.Next(0, 5) == 0) // �������� ���� �������� �������������
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ��������: ������������� �������� ����������.");
                return null;
            }

            List<Coordinates> adjustedRoute = new List<Coordinates>(currentRoute); // �������� � ������
            bool containsRock = detectedObstacles.Any(o => o.Description != null && o.Description.ToLower().Contains("������"));

            if (containsRock)
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ��������� '������', ��������� ������������� ������������� (��������� �����).");
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
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ���������� ������ �����������, ��������� ����������� ������������� (��������� �����).");
                for (int i = 0; i < adjustedRoute.Count; i++)
                {
                    // ��� �������, ����� �����
                    adjustedRoute[i] = new Coordinates(
                        adjustedRoute[i].Latitude + (_random.NextDouble() - 0.5) * 0.00001,
                        adjustedRoute[i].Longitude + (_random.NextDouble() - 0.5) * 0.00001
                    );
                }
            }
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ����������������� ������� ����� ({adjustedRoute.Count} �����).");
            return adjustedRoute;
        }

        /// <inheritdoc/>
        public void StartNavigation()
        {
            if (_isActive)
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation: ������� GPS ��� �������.");
                return;
            }

            // �������� ����������� ���� ��� ��������� ������
            if (_random.Next(0, 10) == 0) // 10% ���� ����
            {
                _isActive = false;
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation: ������� ���������. GPS ������ �� ���� ����������� ���������.");
            }
            else
            {
                _isActive = true;
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation: ������� GPS ������� ������������. ������� �������������� �������: {_currentSimulatedPosition}.");
            }
        }

        /// <inheritdoc/>
        public List<Coordinates> StartNavigation(Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3)
        {
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (� �����������): ������� ��������� ������� GPS � ������� �������� � {initialTargetPosition}.");

            this.StartNavigation(); // ������� �������� ������������ ������� (��� ���������, ��� ��� ��� �������)

            if (!_isActive)
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (� �����������): ������� GPS �� ������� ����� ������� �������, �������������� ������ �������� ����������.");
                return null;
            }

            // ���� ������� �������, ������� �������������� ������� ��� ������ ���� ����������� ����� UpdateSimulatedPosition.
            // ���� ���, �� _currentSimulatedPosition ����� (0,0) ��� ��������� ��������� ��������.
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (� �����������): ������� �������. ������� ������� ��� �������: {_currentSimulatedPosition}. ����������� �������������� ������ ��������...");

            List<Coordinates> calculatedRoute = CalculateRoute(initialTargetPosition, initialBoundaries, initialPrecisionPoints);

            if (calculatedRoute == null || !calculatedRoute.Any())
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (� �����������): �������������� ������ �������� �� ������ ��� ������ ������ ������� (�������� �� �������� GPS).");
            }
            else
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (� �����������): �������������� ������� ������� ��������� ({calculatedRoute.Count} �����).");
            }
            return calculatedRoute;
        }


        /// <inheritdoc/>
        public void StopNavigation()
        {
            if (!_isActive)
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StopNavigation: ������� GPS ��� ���������.");
                return;
            }
            _isActive = false;
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StopNavigation: ������� GPS ��������������.");
        }

        /// <inheritdoc/>
        public void UpdateSimulatedPosition(Coordinates newPosition)
        {
            _currentSimulatedPosition = newPosition;
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: UpdateSimulatedPosition: �������������� ������� GPS ��������� ��: {newPosition}.");
        }
    }
}