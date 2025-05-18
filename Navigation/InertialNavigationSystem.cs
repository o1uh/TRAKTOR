using Traktor.Interfaces;
using Traktor.DataModels;


namespace Traktor.Navigation
{
    public class InertialNavigationSystem : INavigationSystem
    {
        private Coordinates _currentEstimatedPosition;
        private DateTime _lastInitializationTime;
        private bool _isInitializedAndActive = false;
        private readonly Random _random = new Random();

        public InertialNavigationSystem()
        {
            _currentEstimatedPosition = new Coordinates(0, 0);
            _lastInitializationTime = DateTime.MinValue;
            Console.WriteLine($"[InertialNavSystem]: ������ ������. ������� ������������� ����� StartAndCalculateInitialRoute.");
        }

        public List<Coordinates> StartNavigation(Coordinates systemInitial��������Position, Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3)
        {
            Console.WriteLine($"[InertialNavSystem]: ������� ��������� � �������������. ��������� ������� ��� ��������: {systemInitial��������Position}.");
            // �������� ��������� ������� ������������� ��� (��������, ���� �������� ��� ����������)
            if (_random.Next(0, 10) == 0) // 10% ���� ������� �������������
            {
                _isInitializedAndActive = false;
                Console.WriteLine("[InertialNavSystem]: ������� �������������/���������.");
                return null;
            }

            _currentEstimatedPosition = systemInitial��������Position;
            _lastInitializationTime = DateTime.Now;
            _isInitializedAndActive = true;
            Console.WriteLine($"[InertialNavSystem]: ������� ��� ������� ������������ � ����������������. ������� ����������: {_currentEstimatedPosition}.");

            Console.WriteLine("[InertialNavSystem]: ����������� �������������� ������ ��������...");
            return CalculateRoute(initialTargetPosition, initialBoundaries, initialPrecisionPoints);
        }

        public void StopNavigation()
        {
            _isInitializedAndActive = false;
            Console.WriteLine("[InertialNavSystem]: ������� ��� ��������������.");
        }

        public Coordinates GetPosition()
        {
            if (!_isInitializedAndActive)
            {
                Console.WriteLine("[InertialNavSystem]: GetPosition - ������� �� ������� ��� �� ����������������. ���������� ��������� �������.");
                return _currentEstimatedPosition;
            }

            TimeSpan timeSinceLastInitialization = DateTime.Now - _lastInitializationTime;

            if (timeSinceLastInitialization.TotalSeconds > 0.2)
            {
                double driftMagnitude = timeSinceLastInitialization.TotalSeconds * 0.000003;
                _currentEstimatedPosition = new Coordinates(
                    _currentEstimatedPosition.Latitude + (_random.NextDouble() - 0.5) * driftMagnitude,
                    _currentEstimatedPosition.Longitude + (_random.NextDouble() - 0.5) * driftMagnitude
                );
                // _lastInitializationTime �� ��������� �����, ����� ������������� �� ��������� ��������.
                // ���������� _currentEstimatedPosition ����� ��������� ����������� ������ ��� � �������.
                Console.WriteLine($"[InertialNavSystem]: GetPosition - �������� �����. ����� �������: {_currentEstimatedPosition}");
            }

            return _currentEstimatedPosition;
        }

        public List<Coordinates> CalculateRoute(Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3)
        {
            if (!_isInitializedAndActive)
            {
                Console.WriteLine("[InertialNavSystem]: CalculateRoute - ������� �� ������� ��� �� ����������������.");
                return null;
            }

            Coordinates startPosition = this.GetPosition(); // ���������� ������� ������ ���
            Console.WriteLine($"[InertialNavSystem]: ������ �������� �� ������� ������ ��� ({startPosition}) �� {targetPosition}.");

            List<Coordinates> route = new List<Coordinates>();
            route.Add(startPosition);

            int totalSegments = Math.Max(1, precisionPoints + 1);
            for (int i = 1; i < totalSegments; i++)
            {
                double fraction = (double)i / totalSegments;
                route.Add(new Coordinates(
                   startPosition.Latitude + (targetPosition.Latitude - startPosition.Latitude) * fraction,
                   startPosition.Longitude + (targetPosition.Longitude - startPosition.Longitude) * fraction
               ));
            }
            route.Add(targetPosition);

            Console.WriteLine($"[InertialNavSystem]: ������� ���������, {route.Count} �����.");
            return route;
        }

        public List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles)
        {
            if (!_isInitializedAndActive)
            {
                Console.WriteLine("[InertialNavSystem]: AdjustRoute - ������� �� ������� ��� �� ����������������.");
                return null;
            }

            Console.WriteLine($"[InertialNavSystem]: ������ �� ������������� �������� ({currentRoute?.Count ?? 0} �����) ��-�� {detectedObstacles?.Count ?? 0} �����������.");
            if (currentRoute == null || !currentRoute.Any()) { Console.WriteLine("[InertialNavSystem]: ��� ������������� ��������."); return null; }
            if (detectedObstacles == null || !detectedObstacles.Any()) { Console.WriteLine("[InertialNavSystem]: ��� ����������� ��� �������������."); return new List<Coordinates>(currentRoute); }

            if (_random.Next(0, 2) == 0)
            {
                Console.WriteLine("[InertialNavSystem]: ������������� �������� ���������� (�������� ��� ���).");
                return null;
            }
            List<Coordinates> adjustedRoute = new List<Coordinates>(currentRoute);
            // �������� �������������
            Console.WriteLine($"[InertialNavSystem]: ������� ��� ������� (��������).");
            for (int i = 0; i < adjustedRoute.Count; i++)
            {
                adjustedRoute[i] = new Coordinates(
                    adjustedRoute[i].Latitude + (_random.NextDouble() - 0.5) * 0.00015,
                    adjustedRoute[i].Longitude + (_random.NextDouble() - 0.5) * 0.00015
                );
            }
            return adjustedRoute;
        }

        public void UpdateSimulatedPosition(Coordinates newPosition)
        {
            Console.WriteLine($"[InertialNavSystem]: UpdateSimulatedPosition ������ � {newPosition}. ��� ��� ��� ����������� ������������.");
            _currentEstimatedPosition = newPosition;
            if (_isInitializedAndActive)
            {
                _lastInitializationTime = DateTime.Now;
                Console.WriteLine($"[InertialNavSystem]: ������� ��� ������������� ���������, ����� ������ ��������: {_currentEstimatedPosition}");
            }
            else
            {
                Console.WriteLine($"[InertialNavSystem]: ������� ��� ���������, �� ������� �� ���� �������. �������� StartAndCalculateInitialRoute ��� ������ �������������.");
            }
        }
    }
}