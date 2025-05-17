using Traktor.Interfaces;
using Traktor.DataModels;

namespace Traktor.Navigation
{
    public class GPSNavigationSystem : INavigationSystem
    {
        private Coordinates _currentSimulatedPosition;
        private bool _isActive = false; // ������� �� ��������� �� ������� (� "������ ������")
        private readonly Random _random = new Random();

        public GPSNavigationSystem(Coordinates initialPosition)
        {
            // ��� ������������� ������� GPS-�������, ��� ����� ���� ��� �� "��������"
            // _currentSimulatedPosition ������ �������, ��� ������� "��������" ��� ��� � ��������� ���.
            _currentSimulatedPosition = initialPosition;
            Console.WriteLine($"[GPSNavigationSystem]: ������ ������. ��������� �������������� �������: {_currentSimulatedPosition}. ������� �� �������.");
        }

        public Coordinates GetPosition()
        {
            if (!_isActive)
            {
                Console.WriteLine("[GPSNavigationSystem]: GetPosition - ������� �� �������. ���������� ��������� ��������� ������� (��� ����� ������� ����������).");
                // ��� ������ ������ ������, �� � �������� ������� ��� ����� ���� �������.
            }
            else
            {
                Console.WriteLine("[GPSNavigationSystem]: GetPosition - ������� �������.");
            }

            Coordinates reportedPosition = new Coordinates(
                _currentSimulatedPosition.Latitude + (_random.NextDouble() - 0.5) * 0.000001, // ������ ���
                _currentSimulatedPosition.Longitude + (_random.NextDouble() - 0.5) * 0.000001
            );
            Console.WriteLine($"[GPSNavigationSystem]: GetPosition ����������: {reportedPosition}");
            return reportedPosition;
        }

        public List<Coordinates> CalculateRoute(Coordinates startPosition, Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3)
        {
            if (!_isActive)
            {
                Console.WriteLine("[GPSNavigationSystem]: CalculateRoute - ������� �� �������, ������ �������� ����������.");
                return null;
            }

            Console.WriteLine($"[GPSNavigationSystem]: ������ �������� �� {startPosition} �� {targetPosition} � {precisionPoints} �������������� �������.");

            List<Coordinates> route = new List<Coordinates>();
            route.Add(startPosition);

            int totalSegments = Math.Max(1, precisionPoints + 1); // ������� 1 ������� (������)
            for (int i = 1; i < totalSegments; i++)
            {
                double fraction = (double)i / totalSegments;
                route.Add(new Coordinates(
                   startPosition.Latitude + (targetPosition.Latitude - startPosition.Latitude) * fraction,
                   startPosition.Longitude + (targetPosition.Longitude - startPosition.Longitude) * fraction
               ));
            }
            route.Add(targetPosition);

            Console.WriteLine($"[GPSNavigationSystem]: ������� ���������, {route.Count} �����.");
            // ���� ����� �� ������ _currentSimulatedPosition, �.�. ��� ������ ������.
            // �������� � ���������� ������� - ��������������� ControlUnit ����� UpdateSimulatedPosition.
            return new List<Coordinates>(route);
        }

        public List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles)
        {
            if (!_isActive)
            {
                Console.WriteLine("[GPSNavigationSystem]: AdjustRoute - ������� �� �������, ������������� ����������.");
                return null;
            }

            Console.WriteLine($"[GPSNavigationSystem]: ������ �� ������������� �������� ({currentRoute?.Count ?? 0} �����) ��-�� {detectedObstacles?.Count ?? 0} �����������.");

            if (currentRoute == null || !currentRoute.Any())
            {
                Console.WriteLine("[GPSNavigationSystem]: ��� ������������� �������� ��� �������������.");
                return null;
            }

            if (detectedObstacles == null || !detectedObstacles.Any())
            {
                Console.WriteLine("[GPSNavigationSystem]: ������������� �� ��������� (����������� ���).");
                return new List<Coordinates>(currentRoute);
            }

            if (_random.Next(0, 3) == 0)
            {
                Console.WriteLine("[GPSNavigationSystem]: ������������� �������� ���������� (��������).");
                return null;
            }

            List<Coordinates> adjustedRoute = new List<Coordinates>(currentRoute);
            bool containsRock = detectedObstacles.Any(o => o.Description.ToLower().Contains("������"));

            if (containsRock)
            {
                Console.WriteLine("[GPSNavigationSystem]: ��������� '������', ��������� ������������� �������������.");
                for (int i = 0; i < adjustedRoute.Count; i++)
                {
                    adjustedRoute[i] = new Coordinates(
                        adjustedRoute[i].Latitude + (_random.NextDouble() - 0.2) * 0.0003,
                        adjustedRoute[i].Longitude + (_random.NextDouble() - 0.2) * 0.0003
                    );
                }
            }
            else
            {
                Console.WriteLine("[GPSNavigationSystem]: ���������� ������ �����������, ��������� ����������� �������������.");
                for (int i = 0; i < adjustedRoute.Count; i++)
                {
                    adjustedRoute[i] = new Coordinates(
                        adjustedRoute[i].Latitude + (_random.NextDouble() - 0.5) * 0.0001,
                        adjustedRoute[i].Longitude + (_random.NextDouble() - 0.5) * 0.0001
                    );
                }
            }
            Console.WriteLine($"[GPSNavigationSystem]: ������� ��� �������.");
            Console.WriteLine($"[GPSNavigationSystem]: ����������������� ������� ����� ({adjustedRoute.Count} �����).");
            return new List<Coordinates>(adjustedRoute);
        }

        public void StopNavigation()
        {
            _isActive = false;
            Console.WriteLine("[GPSNavigationSystem]: ������� GPS �������������� (StopNavigation).");
        }

        public List<Coordinates> StartNavigation(Coordinates initialStartPosition, Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3)
        {
            Console.WriteLine("[GPSNavigationSystem]: ������� ��������� ������� GPS (StartNavigation)...");

            // �������� ��������� ������� ��� ������� GPS ������ (��������, �� ������ ��������)
            if (_random.Next(0, 5) == 0) // 20% ���� ������� �������
            {
                _isActive = false; // ��������, ��� ������� �� �������
                Console.WriteLine("[GPSNavigationSystem]: ������� ���������. GPS ������ �� ���� ����������� ���������.");
                return null; // ���������� null, ����� ControlUnit �����, ��� ������ �� ������
            }

            _isActive = true;
            UpdateSimulatedPosition(initialStartPosition); // ������������� ��������� �������, ��� � ������
            Console.WriteLine($"[GPSNavigationSystem]: ������� GPS ������� ������������. �������������� ������� ����������� �� {initialStartPosition}.");

            // ���� ��������� �������, ����� �� ������������ ������ �������
            Console.WriteLine("[GPSNavigationSystem]: ����������� �������������� ������ �������� ��� ���������...");
            List<Coordinates> calculatedRoute = CalculateRoute(initialStartPosition, initialTargetPosition, initialBoundaries, initialPrecisionPoints);

            if (calculatedRoute == null)
            {
                Console.WriteLine("[GPSNavigationSystem]: �������������� ������ �������� �� ������ (�������� �� �������� GPS). ������� �������� ��������, �� ��� ��������.");
                // _isActive ����� �������� true, �.�. ��� GPS ������ "�������", �� ������� �� ����������.
                // ControlUnit ������ ����� ���������� null �� CalculateRoute.
            }

            return calculatedRoute;
        }

        public void UpdateSimulatedPosition(Coordinates newPosition)
        {
            // ���� ����� ����� ControlUnit'�, ����� �������� GPS-������� (� ������ ���������),
            // ��� ������� "��������" ����� �������� ���� ��������.
            // � �������� ������� GPS ��� �� ��������� ����� �������.
            _currentSimulatedPosition = newPosition;
            Console.WriteLine($"[GPSNavigationSystem]: �������������� ������� ��������� ControlUnit'��: {_currentSimulatedPosition}");
        }
    }
}