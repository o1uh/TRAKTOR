using Traktor.Interfaces;
using Traktor.DataModels;


namespace Traktor.Navigation
{
    public class GPSNavigationSystem : INavigationSystem
    {
        private Coordinates _currentSimulatedPosition;
        private bool _isActive = false;
        private readonly Random _random = new Random();

        public GPSNavigationSystem() // Конструктор без параметров
        {
            _currentSimulatedPosition = new Coordinates(0, 0); // Значение по умолчанию
            Console.WriteLine($"[GPSNavigationSystem]: Объект создан. Позиция по умолчанию: {_currentSimulatedPosition}. Система НЕ АКТИВНА.");
        }

        public Coordinates GetPosition()
        {
            if (!_isActive)
            {
                Console.WriteLine("[GPSNavigationSystem]: GetPosition - система НЕ АКТИВНА. Возвращаем последнюю известную позицию.");
            }
            else // Можно убрать else, если нет специфичного лога для активного состояния здесь
            {
                 Console.WriteLine("[GPSNavigationSystem]: GetPosition - система АКТИВНА.");
            }

            Coordinates reportedPosition = new Coordinates(
                _currentSimulatedPosition.Latitude + (_random.NextDouble() - 0.5) * 0.000001, // Легкий шум
                _currentSimulatedPosition.Longitude + (_random.NextDouble() - 0.5) * 0.000001
            );
            Console.WriteLine($"[GPSNavigationSystem]: GetPosition возвращает: {reportedPosition}"); // Закомментировано, чтобы не спамить
            return reportedPosition;
        }

        public List<Coordinates> CalculateRoute(Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3)
        {
            if (!_isActive)
            {
                Console.WriteLine("[GPSNavigationSystem]: CalculateRoute - система НЕ АКТИВНА, расчет маршрута невозможен.");
                return null;
            }

            Coordinates startPosition = this.GetPosition(); // Используем текущую позицию как стартовую
            Console.WriteLine($"[GPSNavigationSystem]: Расчет маршрута от ТЕКУЩЕЙ ({startPosition}) до {targetPosition} с {precisionPoints} промежуточными точками.");

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

            Console.WriteLine($"[GPSNavigationSystem]: Маршрут рассчитан, {route.Count} точек.");
            return new List<Coordinates>(route);
        }

        public List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles)
        {
            if (!_isActive)
            {
                Console.WriteLine("[GPSNavigationSystem]: AdjustRoute - система НЕ АКТИВНА, корректировка невозможна.");
                return null;
            }

            Console.WriteLine($"[GPSNavigationSystem]: Запрос на корректировку маршрута ({currentRoute?.Count ?? 0} точек) из-за {detectedObstacles?.Count ?? 0} препятствий.");

            if (currentRoute == null || !currentRoute.Any())
            {
                Console.WriteLine("[GPSNavigationSystem]: Нет оригинального маршрута для корректировки.");
                return null;
            }

            if (detectedObstacles == null || !detectedObstacles.Any())
            {
                Console.WriteLine("[GPSNavigationSystem]: Корректировка не требуется (препятствий нет).");
                return new List<Coordinates>(currentRoute);
            }

            if (_random.Next(0, 3) == 0)
            {
                Console.WriteLine("[GPSNavigationSystem]: Корректировка маршрута НЕВОЗМОЖНА (имитация).");
                return null;
            }

            List<Coordinates> adjustedRoute = new List<Coordinates>(currentRoute);
            bool containsRock = detectedObstacles.Any(o => o.Description.ToLower().Contains("камень"));

            if (containsRock)
            {
                Console.WriteLine("[GPSNavigationSystem]: Обнаружен 'камень', применяем специфическую корректировку.");
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
                Console.WriteLine("[GPSNavigationSystem]: Обнаружено другое препятствие, применяем стандартную корректировку.");
                for (int i = 0; i < adjustedRoute.Count; i++)
                {
                    adjustedRoute[i] = new Coordinates(
                        adjustedRoute[i].Latitude + (_random.NextDouble() - 0.5) * 0.0001,
                        adjustedRoute[i].Longitude + (_random.NextDouble() - 0.5) * 0.0001
                    );
                }
            }
            Console.WriteLine($"[GPSNavigationSystem]: Маршрут был изменен.");
            Console.WriteLine($"[GPSNavigationSystem]: Скорректированный маршрут готов ({adjustedRoute.Count} точек).");
            return new List<Coordinates>(adjustedRoute);
        }

        public void StopNavigation()
        {
            _isActive = false;
            Console.WriteLine("[GPSNavigationSystem]: Система GPS деактивирована (StopNavigation).");
        }

        public List<Coordinates> StartNavigation(Coordinates systemInitialВыставкаPosition, Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3)
        {
            Console.WriteLine($"[GPSNavigationSystem]: Попытка активации системы GPS. Начальная позиция для выставки: {systemInitialВыставкаPosition}.");

            if (_random.Next(0, 5) == 0)
            {
                _isActive = false;
                Console.WriteLine("[GPSNavigationSystem]: НЕУДАЧА АКТИВАЦИИ. GPS модуль не смог запуститься корректно.");
                return null;
            }

            _isActive = true;
            // Устанавливаем _currentSimulatedPosition в точку, где система "выставляется"
            _currentSimulatedPosition = systemInitialВыставкаPosition;
            Console.WriteLine($"[GPSNavigationSystem]: Система GPS успешно активирована. Симулированная позиция установлена на {_currentSimulatedPosition}.");

            Console.WriteLine("[GPSNavigationSystem]: Выполняется первоначальный расчет маршрута при активации...");
            // CalculateRoute теперь не принимает startPosition, он возьмет ее из this.GetPosition()
            List<Coordinates> calculatedRoute = CalculateRoute(initialTargetPosition, initialBoundaries, initialPrecisionPoints);

            if (calculatedRoute == null)
            {
                Console.WriteLine("[GPSNavigationSystem]: Первоначальный расчет маршрута не удался (несмотря на активный GPS).");
                // _isActive остается true, так как сам GPS модуль "завелся". ControlUnit обработает null.
            }

            return calculatedRoute;
        }

        public void UpdateSimulatedPosition(Coordinates newPosition)
        {
            _currentSimulatedPosition = newPosition;
            Console.WriteLine($"[GPSNavigationSystem]: Симулированная позиция обновлена ControlUnit'ом: {_currentSimulatedPosition}");
        }
    }
}