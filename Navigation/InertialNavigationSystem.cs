using System.Collections.Generic;
using Traktor.Interfaces;
using Traktor.DataModels;
using System;
using System.Linq;

namespace Traktor.Navigation
{
    public class InertialNavigationSystem : INavigationSystem
    {
        private Coordinates _currentEstimatedPosition; // �������, ����������� ���
        private DateTime _lastUpdateTime;              // ����� ���������� ����������/�����������
        private bool _isActiveAndInitialized = false;  // ���� ���������� � �������������
        private readonly Random _random = new Random();

        // ����������� ����� ���� �������. ��������� ������� �������� ��� StartNavigation.
        public InertialNavigationSystem(Coordinates approximateInitialLocation)
        {
            // approximateInitialLocation ����� �������������� ��� �����-�� ��������������� ���������,
            // �� �������� �������� - � StartNavigation.
            _currentEstimatedPosition = approximateInitialLocation; // ������ ��� �������� �� �������������
            Console.WriteLine($"[InertialNavSystem]: ������ ������. ������� �� ���������: {_currentEstimatedPosition}. ������� �� ������� � �� ����������������.");
        }

        public List<Coordinates> StartNavigation(Coordinates initialStartPosition, Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3)
        {
            Console.WriteLine($"[InertialNavSystem]: ������� ��������� � ������������� � ������� {initialStartPosition}.");

            // �������� ��������� ������� ������������� ��� (��������, ���� �������� ��� ����������)
            if (_random.Next(0, 10) == 0) // 10% ���� ������� �������������
            {
                _isActiveAndInitialized = false;
                Console.WriteLine("[InertialNavSystem]: ������� �������������. �� ������� ������������� ��� ��������� �������.");
                return null; // ������ � ������� �������
            }

            _currentEstimatedPosition = initialStartPosition;
            _lastUpdateTime = DateTime.Now;
            // ����� ����� �� ������������ ����������� ��������/���������, ���� �� ��� ����
            _isActiveAndInitialized = true;
            Console.WriteLine($"[InertialNavSystem]: ������� ��� ������� ������������ � ����������������. ������� ����������: {_currentEstimatedPosition}.");

            Console.WriteLine("[InertialNavSystem]: ����������� �������������� ������ �������� ��� ���������...");
            // ������ �������� �� ������ ������ ��� ������������ �������
            return CalculateRoute(initialStartPosition, initialTargetPosition, initialBoundaries, initialPrecisionPoints);
        }

        public void StopNavigation()
        {
            _isActiveAndInitialized = false; // ������������ � ���������� ���� �������������
            Console.WriteLine("[InertialNavSystem]: ������� ��� �������������� (StopNavigation). ��������� ��������� ������������� ����� StartNavigation.");
        }

        public Coordinates GetPosition()
        {
            if (!_isActiveAndInitialized)
            {
                Console.WriteLine("[InertialNavSystem]: GetPosition - ������� �� ������� ��� �� ����������������.");
                return new Coordinates(double.NaN, double.NaN); // ���������� ����������
            }

            // �������� ������ ��� � �������� �������
            TimeSpan timeSinceLastUpdate = DateTime.Now - _lastUpdateTime;
            double driftFactor = timeSinceLastUpdate.TotalSeconds * 0.000002; // ��������������� �����

            _currentEstimatedPosition = new Coordinates(
                _currentEstimatedPosition.Latitude + (_random.NextDouble() - 0.5) * driftFactor,
                _currentEstimatedPosition.Longitude + (_random.NextDouble() - 0.5) * driftFactor
            );
            // �����: � �������� ��� _currentEstimatedPosition ����������� �� �� ������ ������ IMU,
            // � �� ������ ���������� �� �� �������. ���� ����� - ����� ������ �������� ���������� ������.
            // _lastUpdateTime ����� �� ���������, ����� ����� ��������� ������������� �� ����� ��������� ��������.

            // Console.WriteLine($"[InertialNavSystem]: GetPosition ���������� (� �������): {_currentEstimatedPosition}");
            return _currentEstimatedPosition;
        }

        public List<Coordinates> CalculateRoute(Coordinates startPosition, Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3)
        {
            if (!_isActiveAndInitialized)
            {
                Console.WriteLine("[InertialNavSystem]: CalculateRoute - ������� �� ������� ��� �� ����������������.");
                return null;
            }

            Console.WriteLine($"[InertialNavSystem]: ������ �������� �� {startPosition} (��� ������� ������� {_currentEstimatedPosition}) �� {targetPosition}.");
            // ��� ���, startPosition ��� ������� �������� ������ ������� �� �� ������������ GetPosition()
            // ��� �� ��� startPosition, ������� ������� ControlUnit (������� ������ ���� ����������).
            // ���������� ���������� startPosition, �����������, ��� ControlUnit �������� ���������� ������.

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
            return new List<Coordinates>(route);
        }

        public List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles)
        {
            if (!_isActiveAndInitialized)
            {
                Console.WriteLine("[InertialNavSystem]: AdjustRoute - ������� �� ������� ��� �� ����������������.");
                return null;
            }
            // ... (������ ���������� GPSNavigationSystem, ������� ���� �������������) ...
            Console.WriteLine($"[InertialNavSystem]: ������ �� ������������� �������� ({currentRoute?.Count ?? 0} �����) ��-�� {detectedObstacles?.Count ?? 0} �����������.");
            if (currentRoute == null || !currentRoute.Any()) { return null; }
            if (detectedObstacles == null || !detectedObstacles.Any()) { return new List<Coordinates>(currentRoute); }

            if (_random.Next(0, 2) == 0) // 50% ���� "�������������" ��� ���
            {
                Console.WriteLine("[InertialNavSystem]: ������������� �������� ���������� (�������� ��� ���).");
                return null;
            }

            List<Coordinates> adjustedRoute = new List<Coordinates>(currentRoute);
            // ... (������� ������ ��������, ��� � GPSNavigationSystem) ...
            Console.WriteLine($"[InertialNavSystem]: ������� ��� ������� (�������� ��� ���).");
            return new List<Coordinates>(adjustedRoute);
        }

        public void UpdateSimulatedPosition(Coordinates newPosition)
        {
            // ��� "������" ���, ������� ���� ��������� ���� ������� �� ������ ������������ ������,
            // ���� ����� �� ������ �������� "���������������" ��.
            // ����������� ��� ������ ����������� ����� StartNavigation.
            // ���� ControlUnit ������� ������ ������� �� GPS � ����� "���������������" ���,
            // �� ������ ������� StopNavigation() ��� ���, � ����� StartNavigation() � ������ ������������.
            Console.WriteLine($"[InertialNavSystem]: UpdateSimulatedPosition ������. ��� ��� ����������� StopNavigation() � StartNavigation() � ������ ������������ ��� �����������������/��������.");
            // �����, �������, �������� "�������" ������������, ���� ��� ����� ��� ���������:
            // if (_isActiveAndInitialized)
            // {
            //     _currentEstimatedPosition = newPosition;
            //     _lastUpdateTime = DateTime.Now; // �������� ����� ��� ������
            //     Console.WriteLine($"[InertialNavSystem]: ��������! ������� ��� ������������� ��������� (������� ������������): {_currentEstimatedPosition}");
            // }
        }
    }
}