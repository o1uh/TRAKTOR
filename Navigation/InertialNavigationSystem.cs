using System.Collections.Generic;
using Traktor.Interfaces;
using Traktor.DataModels;
using System;
using System.Linq;

namespace Traktor.Navigation
{
    public class InertialNavigationSystem : INavigationSystem
    {
        private Coordinates _currentEstimatedPosition; // Позиция, вычисленная ИНС
        private DateTime _lastUpdateTime;              // Время последнего обновления/выставления
        private bool _isActiveAndInitialized = false;  // Флаг активности и инициализации
        private readonly Random _random = new Random();

        // Конструктор может быть простым. Начальная позиция задается при StartNavigation.
        public InertialNavigationSystem(Coordinates approximateInitialLocation)
        {
            // approximateInitialLocation может использоваться для какой-то предварительной настройки,
            // но реальная выставка - в StartNavigation.
            _currentEstimatedPosition = approximateInitialLocation; // Просто для хранения до инициализации
            Console.WriteLine($"[InertialNavSystem]: Объект создан. Позиция по умолчанию: {_currentEstimatedPosition}. Система НЕ АКТИВНА и НЕ ИНИЦИАЛИЗИРОВАНА.");
        }

        public List<Coordinates> StartNavigation(Coordinates initialStartPosition, Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3)
        {
            Console.WriteLine($"[InertialNavSystem]: Попытка активации и инициализации с позиции {initialStartPosition}.");

            // Имитация возможной неудачи инициализации ИНС (например, сбой датчиков при калибровке)
            if (_random.Next(0, 10) == 0) // 10% шанс неудачи инициализации
            {
                _isActiveAndInitialized = false;
                Console.WriteLine("[InertialNavSystem]: НЕУДАЧА ИНИЦИАЛИЗАЦИИ. Не удалось откалибровать или выставить систему.");
                return null; // Сигнал о неудаче запуска
            }

            _currentEstimatedPosition = initialStartPosition;
            _lastUpdateTime = DateTime.Now;
            // Здесь могли бы сбрасываться накопленные скорости/ускорения, если бы они были
            _isActiveAndInitialized = true;
            Console.WriteLine($"[InertialNavSystem]: Система ИНС успешно активирована и инициализирована. Позиция выставлена: {_currentEstimatedPosition}.");

            Console.WriteLine("[InertialNavSystem]: Выполняется первоначальный расчет маршрута при активации...");
            // Расчет маршрута на основе только что выставленной позиции
            return CalculateRoute(initialStartPosition, initialTargetPosition, initialBoundaries, initialPrecisionPoints);
        }

        public void StopNavigation()
        {
            _isActiveAndInitialized = false; // Деактивируем и сбрасываем флаг инициализации
            Console.WriteLine("[InertialNavSystem]: Система ИНС деактивирована (StopNavigation). Требуется повторная инициализация через StartNavigation.");
        }

        public Coordinates GetPosition()
        {
            if (!_isActiveAndInitialized)
            {
                Console.WriteLine("[InertialNavSystem]: GetPosition - система НЕ АКТИВНА или НЕ ИНИЦИАЛИЗИРОВАНА.");
                return new Coordinates(double.NaN, double.NaN); // Невалидные координаты
            }

            // Имитация дрейфа ИНС с течением времени
            TimeSpan timeSinceLastUpdate = DateTime.Now - _lastUpdateTime;
            double driftFactor = timeSinceLastUpdate.TotalSeconds * 0.000002; // Увеличивающийся дрейф

            _currentEstimatedPosition = new Coordinates(
                _currentEstimatedPosition.Latitude + (_random.NextDouble() - 0.5) * driftFactor,
                _currentEstimatedPosition.Longitude + (_random.NextDouble() - 0.5) * driftFactor
            );
            // Важно: в реальной ИНС _currentEstimatedPosition обновлялась бы на основе данных IMU,
            // а не просто дрейфовала бы от времени. Этот дрейф - очень грубая имитация накопления ошибки.
            // _lastUpdateTime здесь не обновляем, чтобы дрейф продолжал накапливаться от точки последней выставки.

            // Console.WriteLine($"[InertialNavSystem]: GetPosition возвращает (с дрейфом): {_currentEstimatedPosition}");
            return _currentEstimatedPosition;
        }

        public List<Coordinates> CalculateRoute(Coordinates startPosition, Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3)
        {
            if (!_isActiveAndInitialized)
            {
                Console.WriteLine("[InertialNavSystem]: CalculateRoute - система НЕ АКТИВНА или НЕ ИНИЦИАЛИЗИРОВАНА.");
                return null;
            }

            Console.WriteLine($"[InertialNavSystem]: Расчет маршрута от {startPosition} (ИНС считает текущей {_currentEstimatedPosition}) до {targetPosition}.");
            // Для ИНС, startPosition для расчета маршрута обычно берется из ее собственного GetPosition()
            // или из той startPosition, которую передал ControlUnit (которая должна быть актуальной).
            // Используем переданную startPosition, предполагая, что ControlUnit передает актуальные данные.

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

            Console.WriteLine($"[InertialNavSystem]: Маршрут рассчитан, {route.Count} точек.");
            return new List<Coordinates>(route);
        }

        public List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles)
        {
            if (!_isActiveAndInitialized)
            {
                Console.WriteLine("[InertialNavSystem]: AdjustRoute - система НЕ АКТИВНА или НЕ ИНИЦИАЛИЗИРОВАНА.");
                return null;
            }
            // ... (логика аналогична GPSNavigationSystem, включая шанс невозможности) ...
            Console.WriteLine($"[InertialNavSystem]: Запрос на корректировку маршрута ({currentRoute?.Count ?? 0} точек) из-за {detectedObstacles?.Count ?? 0} препятствий.");
            if (currentRoute == null || !currentRoute.Any()) { return null; }
            if (detectedObstacles == null || !detectedObstacles.Any()) { return new List<Coordinates>(currentRoute); }

            if (_random.Next(0, 2) == 0) // 50% шанс "невозможности" для ИНС
            {
                Console.WriteLine("[InertialNavSystem]: Корректировка маршрута НЕВОЗМОЖНА (имитация для ИНС).");
                return null;
            }

            List<Coordinates> adjustedRoute = new List<Coordinates>(currentRoute);
            // ... (простая логика смещения, как в GPSNavigationSystem) ...
            Console.WriteLine($"[InertialNavSystem]: Маршрут был изменен (имитация для ИНС).");
            return new List<Coordinates>(adjustedRoute);
        }

        public void UpdateSimulatedPosition(Coordinates newPosition)
        {
            // Для "чистой" ИНС, которая сама вычисляет свою позицию на основе инерциальных данных,
            // этот метод не должен напрямую "телепортировать" ее.
            // Выставление ИНС должно происходить через StartNavigation.
            // Если ControlUnit получил точную позицию от GPS и хочет "скорректировать" ИНС,
            // он должен вызвать StopNavigation() для ИНС, а затем StartNavigation() с новыми координатами.
            Console.WriteLine($"[InertialNavSystem]: UpdateSimulatedPosition вызван. Для ИНС используйте StopNavigation() и StartNavigation() с новыми координатами для переинициализации/выставки.");
            // Можно, конечно, добавить "жесткую" перевыставку, если это нужно для симуляции:
            // if (_isActiveAndInitialized)
            // {
            //     _currentEstimatedPosition = newPosition;
            //     _lastUpdateTime = DateTime.Now; // Сбросить время для дрейфа
            //     Console.WriteLine($"[InertialNavSystem]: ВНИМАНИЕ! Позиция ИНС принудительно обновлена (жесткая перевыставка): {_currentEstimatedPosition}");
            // }
        }
    }
}