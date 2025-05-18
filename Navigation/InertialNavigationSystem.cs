using Traktor.Interfaces;
using Traktor.DataModels;

namespace Traktor.Navigation
{
    /// <summary>
    /// ���������� ������� ��������� �� ������ ������������ �������� (���).
    /// ��������� ������ ���, ������� ���������� ������ (�����) �� ��������.
    /// ������������ ��� ��������������� ��� ��������� ������� ���������.
    /// </summary>
    public class InertialNavigationSystem : INavigationSystem
    {
        private Coordinates _currentEstimatedPosition; // ������� ������ ������� � ������ ������
        private DateTime _lastCalibrationTime;      // ����� ��������� ������ ����������/�������� �������
        private bool _isInitializedAndActive = false; // ����, ��� ������� ������� � ���� ���������������� ��������� ��������
        private static readonly Random _random = new Random();

        private const double DRIFT_RATE_PER_SECOND = 0.000001; // �������� �������� ������ (������� � �������)

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="InertialNavigationSystem"/>.
        /// ������� ��������� � ������� ������������� ��������� ��������.
        /// </summary>
        public InertialNavigationSystem()
        {
            _currentEstimatedPosition = new Coordinates(0, 0); // ��������� �������� �� �������������
            _lastCalibrationTime = DateTime.MinValue;           // �� ���� ����������
            _isInitializedAndActive = false;
            Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ������������ ������� ��������� (���) ����������������. ������� ��������� � �������� ��������� �������.");
        }

        /// <inheritdoc/>
        public Coordinates GetPosition()
        {
            if (!_isInitializedAndActive)
            {
                Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: GetPosition: ��� �� ������� ��� �� ����������������. ���������� ��������� ��������� (��������, ����������) �������: {_currentEstimatedPosition}.");
                return _currentEstimatedPosition;
            }

            TimeSpan timeSinceLastCalibration = DateTime.Now - _lastCalibrationTime;
            double elapsedSeconds = timeSinceLastCalibration.TotalSeconds;

            if (elapsedSeconds > 0.1)
            {
                double driftLatitude = (_random.NextDouble() - 0.5) * 2 * DRIFT_RATE_PER_SECOND * elapsedSeconds;
                double driftLongitude = (_random.NextDouble() - 0.5) * 2 * DRIFT_RATE_PER_SECOND * elapsedSeconds;

                _currentEstimatedPosition = new Coordinates(
                    _currentEstimatedPosition.Latitude + driftLatitude,
                    _currentEstimatedPosition.Longitude + driftLongitude
                );
                
                Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: GetPosition: �������� ����� ({elapsedSeconds:F2}�). ����� ������ ���: {_currentEstimatedPosition}");
            }
            return _currentEstimatedPosition;
        }

        /// <inheritdoc/>
        public List<Coordinates> CalculateRoute(Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3)
        {
            if (!_isInitializedAndActive)
            {
                Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: CalculateRoute: ��� �� ������� ��� �� ����������������, ������ �������� ����������.");
                return null;
            }

            Coordinates startPosition = this.GetPosition();
            Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: CalculateRoute: ������ �������� �� ������� ������ ��� ({startPosition}) �� {targetPosition}. ������������� ����� �� �������: {precisionPoints}. �������: {(boundaries == null ? "�� ������" : "������")}.");

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

            Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: CalculateRoute: ������� ���������, {route.Count} �����.");
            return route;
        }

        /// <inheritdoc/>
        public List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles)
        {
            if (!_isInitializedAndActive)
            {
                Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ��� �� ������� ��� �� ����������������, ������������� ����������.");
                return null;
            }

            int currentRouteCount = currentRoute?.Count ?? 0;
            int obstaclesCount = detectedObstacles?.Count ?? 0;
            Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ������ �� ������������� �������� ({currentRouteCount} �����) ��-�� {obstaclesCount} �����������.");

            if (currentRoute == null || !currentRoute.Any())
            {
                Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ��� ������������� �������� ��� �������������.");
                return null;
            }

            if (detectedObstacles == null || !detectedObstacles.Any())
            {
                Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ������������� �� ��������� (����������� ���). ���������� ����� �������� ��������.");
                return new List<Coordinates>(currentRoute);
            }

            if (_random.Next(0, 3) == 0)
            {
                Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ��������: ������������� �������� � ������� ��� ����������.");
                return null;
            }

            List<Coordinates> adjustedRoute = new List<Coordinates>(currentRoute);
            Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: �������� ����������� ������������� �������� ��� ��� (��������� �����).");
            for (int i = 0; i < adjustedRoute.Count; i++)
            {
                adjustedRoute[i] = new Coordinates(
                    adjustedRoute[i].Latitude + (_random.NextDouble() - 0.5) * 0.00002,
                    adjustedRoute[i].Longitude + (_random.NextDouble() - 0.5) * 0.00002
                );
            }
            Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: ����������������� ������� ��� ����� ({adjustedRoute.Count} �����).");
            return adjustedRoute;
        }

        /// <inheritdoc/>
        public void StartNavigation()
        {
            if (_isInitializedAndActive)
            {
                Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation: ��� ��� ������� � ����������������.");
                return;
            }
            _isInitializedAndActive = false;
            Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation: ��� ������ '�������', �� ������� ���������� ��������� ������� ��� ���������� ������.");
        }

        /// <inheritdoc/>
        public List<Coordinates> StartNavigation(Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3)
        {
            Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (� �����������): ������� ��������� ��� � ������� �������� � {initialTargetPosition}.");

            if (!_isInitializedAndActive)
            {
                Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (� �����������): ��� �� ���������������� (�� ������������� ��������� ������� ����� UpdateSimulatedPosition). ������ �������� ����������.");
                return null;
            }

            Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (� �����������): ��� ������� � ����������������. ������� ������� ��� �������: {_currentEstimatedPosition}. ����������� �������������� ������ ��������...");

            List<Coordinates> calculatedRoute = CalculateRoute(initialTargetPosition, initialBoundaries, initialPrecisionPoints);

            if (calculatedRoute == null || !calculatedRoute.Any())
            {
                Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (� �����������): �������������� ������ �������� �� ������ ��� ������ ������ �������.");
            }
            else
            {
                Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (� �����������): �������������� ������� ������� ��������� ({calculatedRoute.Count} �����).");
            }
            return calculatedRoute;
        }

        /// <inheritdoc/>
        public void StopNavigation()
        {
            if (!_isInitializedAndActive)
            {
                Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StopNavigation: ��� ��� ���������.");
                return;
            }
            _isInitializedAndActive = false;
            Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StopNavigation: ������� ��� ��������������.");
        }

        /// <inheritdoc/>
        public void UpdateSimulatedPosition(Coordinates newPosition)
        {
            _currentEstimatedPosition = newPosition;
            _lastCalibrationTime = DateTime.Now;
            _isInitializedAndActive = true;
            Console.WriteLine($"[Navigation/InertialNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: UpdateSimulatedPosition (���������� ���): ������� ��� ���������/������������� ��: {newPosition}. ����� ������� (������� ������� ������ �������). ������� ������� � ����������������.");
        }
    }
}