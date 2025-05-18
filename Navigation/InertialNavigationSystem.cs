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
            Console.WriteLine($"[InertialNavSystem]: Объект создан. Ожидает инициализации через StartAndCalculateInitialRoute.");
        }

        public List<Coordinates> StartNavigation(Coordinates systemInitialВыставкаPosition, Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3)
        {
            Console.WriteLine($"[InertialNavSystem]: Попытка активации и инициализации. Начальная позиция для выставки: {systemInitialВыставкаPosition}.");
            // Имитация возможной неудачи инициализации ИНС (например, сбой датчиков при калибровке)
            if (_random.Next(0, 10) == 0) // 10% шанс неудачи инициализации
            {
                _isInitializedAndActive = false;
                Console.WriteLine("[InertialNavSystem]: НЕУДАЧА ИНИЦИАЛИЗАЦИИ/АКТИВАЦИИ.");
                return null;
            }

            _currentEstimatedPosition = systemInitialВыставкаPosition;
            _lastInitializationTime = DateTime.Now;
            _isInitializedAndActive = true;
            Console.WriteLine($"[InertialNavSystem]: Система ИНС успешно активирована и инициализирована. Позиция выставлена: {_currentEstimatedPosition}.");

            Console.WriteLine("[InertialNavSystem]: Выполняется первоначальный расчет маршрута...");
            return CalculateRoute(initialTargetPosition, initialBoundaries, initialPrecisionPoints);
        }

        public void StopNavigation()
        {
            _isInitializedAndActive = false;
            Console.WriteLine("[InertialNavSystem]: Система ИНС деактивирована.");
        }

        public Coordinates GetPosition()
        {
            if (!_isInitializedAndActive)
            {
                Console.WriteLine("[InertialNavSystem]: GetPosition - система НЕ АКТИВНА или НЕ ИНИЦИАЛИЗИРОВАНА. Возвращаем последнюю позицию.");
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
                // _lastInitializationTime НЕ обновляем здесь, дрейф накапливается от последней выставки.
                // Обновление _currentEstimatedPosition здесь имитирует непрерывную работу ИНС с дрейфом.
                Console.WriteLine($"[InertialNavSystem]: GetPosition - применен дрейф. Новая позиция: {_currentEstimatedPosition}");
            }

            return _currentEstimatedPosition;
        }

        public List<Coordinates> CalculateRoute(Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3)
        {
            if (!_isInitializedAndActive)
            {
                Console.WriteLine("[InertialNavSystem]: CalculateRoute - система НЕ АКТИВНА или НЕ ИНИЦИАЛИЗИРОВАНА.");
                return null;
            }

            Coordinates startPosition = this.GetPosition(); // Используем текущую оценку ИНС
            Console.WriteLine($"[InertialNavSystem]: Расчет маршрута от ТЕКУЩЕЙ ОЦЕНКИ ИНС ({startPosition}) до {targetPosition}.");

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
            return route;
        }

        public List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles)
        {
            if (!_isInitializedAndActive)
            {
                Console.WriteLine("[InertialNavSystem]: AdjustRoute - система НЕ АКТИВНА или НЕ ИНИЦИАЛИЗИРОВАНА.");
                return null;
            }

            Console.WriteLine($"[InertialNavSystem]: Запрос на корректировку маршрута ({currentRoute?.Count ?? 0} точек) из-за {detectedObstacles?.Count ?? 0} препятствий.");
            if (currentRoute == null || !currentRoute.Any()) { Console.WriteLine("[InertialNavSystem]: Нет оригинального маршрута."); return null; }
            if (detectedObstacles == null || !detectedObstacles.Any()) { Console.WriteLine("[InertialNavSystem]: Нет препятствий для корректировки."); return new List<Coordinates>(currentRoute); }

            if (_random.Next(0, 2) == 0)
            {
                Console.WriteLine("[InertialNavSystem]: Корректировка маршрута НЕВОЗМОЖНА (имитация для ИНС).");
                return null;
            }
            List<Coordinates> adjustedRoute = new List<Coordinates>(currentRoute);
            // Имитация корректировки
            Console.WriteLine($"[InertialNavSystem]: Маршрут был изменен (имитация).");
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
            Console.WriteLine($"[InertialNavSystem]: UpdateSimulatedPosition вызван с {newPosition}. Для ИНС это равносильно перевыставке.");
            _currentEstimatedPosition = newPosition;
            if (_isInitializedAndActive)
            {
                _lastInitializationTime = DateTime.Now;
                Console.WriteLine($"[InertialNavSystem]: Позиция ИНС принудительно обновлена, время дрейфа сброшено: {_currentEstimatedPosition}");
            }
            else
            {
                Console.WriteLine($"[InertialNavSystem]: Позиция ИНС обновлена, но система не была активна. Вызовите StartAndCalculateInitialRoute для полной инициализации.");
            }
        }
    }
}