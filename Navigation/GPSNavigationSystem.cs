using Traktor.Interfaces;
using Traktor.DataModels;

namespace Traktor.Navigation
{
    public class GPSNavigationSystem : INavigationSystem
    {
        private Coordinates _currentSimulatedPosition;
        private bool _isActive = false; // Система по умолчанию не активна (в "спящем режиме")
        private readonly Random _random = new Random();

        public GPSNavigationSystem(Coordinates initialPosition)
        {
            // При инициализации объекта GPS-системы, она может быть еще не "включена"
            // _currentSimulatedPosition хранит позицию, где трактор "появился" или был в последний раз.
            _currentSimulatedPosition = initialPosition;
            Console.WriteLine($"[GPSNavigationSystem]: Объект создан. Начальная симулированная позиция: {_currentSimulatedPosition}. Система НЕ АКТИВНА.");
        }

        public Coordinates GetPosition()
        {
            if (!_isActive)
            {
                Console.WriteLine("[GPSNavigationSystem]: GetPosition - система НЕ АКТИВНА. Возвращаем последнюю известную позицию (или можно бросить исключение).");
                // Для макета просто вернем, но в реальной системе это может быть ошибкой.
            }
            else
            {
                Console.WriteLine("[GPSNavigationSystem]: GetPosition - система АКТИВНА.");
            }

            Coordinates reportedPosition = new Coordinates(
                _currentSimulatedPosition.Latitude + (_random.NextDouble() - 0.5) * 0.000001, // Легкий шум
                _currentSimulatedPosition.Longitude + (_random.NextDouble() - 0.5) * 0.000001
            );
            Console.WriteLine($"[GPSNavigationSystem]: GetPosition возвращает: {reportedPosition}");
            return reportedPosition;
        }

        public List<Coordinates> CalculateRoute(Coordinates startPosition, Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3)
        {
            if (!_isActive)
            {
                Console.WriteLine("[GPSNavigationSystem]: CalculateRoute - система НЕ АКТИВНА, расчет маршрута невозможен.");
                return null;
            }

            Console.WriteLine($"[GPSNavigationSystem]: Расчет маршрута от {startPosition} до {targetPosition} с {precisionPoints} промежуточными точками.");

            List<Coordinates> route = new List<Coordinates>();
            route.Add(startPosition);

            int totalSegments = Math.Max(1, precisionPoints + 1); // Минимум 1 сегмент (прямая)
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
            // Этот метод не меняет _currentSimulatedPosition, т.к. это просто расчет.
            // Движение и обновление позиции - ответственность ControlUnit через UpdateSimulatedPosition.
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

        public List<Coordinates> StartNavigation(Coordinates initialStartPosition, Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3)
        {
            Console.WriteLine("[GPSNavigationSystem]: Попытка активации системы GPS (StartNavigation)...");

            // Имитация возможной неудачи при запуске GPS модуля (например, не поймал спутники)
            if (_random.Next(0, 5) == 0) // 20% шанс неудачи запуска
            {
                _isActive = false; // Убедимся, что система не активна
                Console.WriteLine("[GPSNavigationSystem]: НЕУДАЧА АКТИВАЦИИ. GPS модуль не смог запуститься корректно.");
                return null; // Возвращаем null, чтобы ControlUnit понял, что запуск не удался
            }

            _isActive = true;
            UpdateSimulatedPosition(initialStartPosition); // Устанавливаем начальную позицию, как и раньше
            Console.WriteLine($"[GPSNavigationSystem]: Система GPS успешно активирована. Симулированная позиция установлена на {initialStartPosition}.");

            // Если активация успешна, сразу же рассчитываем первый маршрут
            Console.WriteLine("[GPSNavigationSystem]: Выполняется первоначальный расчет маршрута при активации...");
            List<Coordinates> calculatedRoute = CalculateRoute(initialStartPosition, initialTargetPosition, initialBoundaries, initialPrecisionPoints);

            if (calculatedRoute == null)
            {
                Console.WriteLine("[GPSNavigationSystem]: Первоначальный расчет маршрута не удался (несмотря на активный GPS). Система остается активной, но без маршрута.");
                // _isActive можно оставить true, т.к. сам GPS модуль "завелся", но маршрут не построился.
                // ControlUnit должен будет обработать null от CalculateRoute.
            }

            return calculatedRoute;
        }

        public void UpdateSimulatedPosition(Coordinates newPosition)
        {
            // Этот метод нужен ControlUnit'у, чтобы сообщать GPS-системе (в рамках симуляции),
            // где трактор "оказался" после имитации шага движения.
            // В реальной системе GPS сам бы определял новую позицию.
            _currentSimulatedPosition = newPosition;
            Console.WriteLine($"[GPSNavigationSystem]: Симулированная позиция обновлена ControlUnit'ом: {_currentSimulatedPosition}");
        }
    }
}