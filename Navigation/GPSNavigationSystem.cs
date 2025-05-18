using Traktor.Interfaces;
using Traktor.DataModels;

namespace Traktor.Navigation
{
    /// <summary>
    /// Реализация системы навигации на основе GPS.
    /// Имитирует работу GPS-модуля, включая получение координат, расчет и корректировку маршрута.
    /// Является основной системой навигации.
    /// </summary>
    public class GPSNavigationSystem : INavigationSystem
    {
        private Coordinates _currentSimulatedPosition;
        private bool _isActive = false;
        private static readonly Random _random = new Random(); // Один экземпляр Random для всего класса

        private const double GPS_NOISE_MAGNITUDE = 0.000005; // Небольшой шум для имитации неточности GPS

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GPSNavigationSystem"/>.
        /// Начальная позиция устанавливается в (0,0), система неактивна.
        /// </summary>
        public GPSNavigationSystem()
        {
            _currentSimulatedPosition = new Coordinates(0, 0); // Значение по умолчанию
            _isActive = false;
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Система GPS навигации инициализирована. Позиция по умолчанию: {_currentSimulatedPosition}. Система НЕ АКТИВНА.");
        }

        /// <inheritdoc/>
        public Coordinates GetPosition()
        {
            string activeStatus = _isActive ? "АКТИВНА" : "НЕ АКТИВНА";
            // Логируем только если что-то важное (например, система не активна, но пытаются получить позицию)
            // или для детальной отладки. Пока оставим основной лог.
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: GetPosition вызван. Система {activeStatus}.");

            // Имитация небольшого шума/погрешности GPS
            Coordinates reportedPosition = new Coordinates(
                _currentSimulatedPosition.Latitude + (_random.NextDouble() - 0.5) * GPS_NOISE_MAGNITUDE,
                _currentSimulatedPosition.Longitude + (_random.NextDouble() - 0.5) * GPS_NOISE_MAGNITUDE
            );
            // Следующий лог может спамить, если GetPosition вызывается часто. Оставим его закомментированным для продакшена.
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: GetPosition. Симулированная: {_currentSimulatedPosition}, Возвращаемая с шумом: {reportedPosition}");
            return reportedPosition;
        }

        /// <inheritdoc/>
        public List<Coordinates> CalculateRoute(Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3)
        {
            if (!_isActive)
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: CalculateRoute: Система НЕ АКТИВНА, расчет маршрута невозможен.");
                return null; // Или пустой список: new List<Coordinates>();
            }

            Coordinates startPosition = this.GetPosition(); // Получаем "реальную" позицию с шумом
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: CalculateRoute: Расчет маршрута от {startPosition} до {targetPosition}. Промежуточных точек на сегмент: {precisionPoints}. Границы поля: {(boundaries == null ? "не заданы" : "заданы")}.");

            List<Coordinates> route = new List<Coordinates>();
            route.Add(startPosition); // Начальная точка - это "текущая позиция"

            // Простая линейная интерполяция для маршрута
            // precisionPoints - это количество ДОПОЛНИТЕЛЬНЫХ точек МЕЖДУ началом и концом сегмента.
            // Если precisionPoints = 0, то только start и target.
            // Если precisionPoints = 1, то start, middle, target (всего 1+1 = 2 сегмента)
            // Общее количество сегментов = precisionPoints + 1
            int totalSegments = Math.Max(1, precisionPoints + 1); // Хотя бы один сегмент

            for (int i = 1; i < totalSegments; i++) // Добавляем precisionPoints точек
            {
                double fraction = (double)i / totalSegments;
                route.Add(new Coordinates(
                   startPosition.Latitude + (targetPosition.Latitude - startPosition.Latitude) * fraction,
                   startPosition.Longitude + (targetPosition.Longitude - startPosition.Longitude) * fraction
               ));
            }
            route.Add(targetPosition); // Конечная точка

            // Здесь могла бы быть проверка на выход за boundaries, если они заданы.
            // Для макета пока опускаем.

            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: CalculateRoute: Маршрут рассчитан, {route.Count} точек.");
            return route; // Возвращаем сам список, нет нужды в new List<Coordinates>(route) здесь
        }

        /// <inheritdoc/>
        public List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles)
        {
            if (!_isActive)
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: Система НЕ АКТИВНА, корректировка невозможна.");
                return null;
            }

            int currentRouteCount = currentRoute?.Count ?? 0;
            int obstaclesCount = detectedObstacles?.Count ?? 0;
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: Запрос на корректировку маршрута ({currentRouteCount} точек) из-за {obstaclesCount} препятствий.");

            if (currentRoute == null || !currentRoute.Any())
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: Нет оригинального маршрута для корректировки.");
                return null;
            }

            if (detectedObstacles == null || !detectedObstacles.Any())
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: Корректировка не требуется (препятствий нет). Возвращаем копию текущего маршрута.");
                return new List<Coordinates>(currentRoute); // Возвращаем копию, чтобы избежать неожиданных изменений оригинала
            }

            // Имитация сложной логики корректировки
            if (_random.Next(0, 5) == 0) // Увеличим шанс успешной корректировки
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: Имитация: корректировка маршрута НЕВОЗМОЖНА.");
                return null;
            }

            List<Coordinates> adjustedRoute = new List<Coordinates>(currentRoute); // Работаем с копией
            bool containsRock = detectedObstacles.Any(o => o.Description != null && o.Description.ToLower().Contains("камень"));

            if (containsRock)
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: Обнаружен 'камень', применяем специфическую корректировку (небольшой сдвиг).");
                for (int i = 0; i < adjustedRoute.Count; i++)
                {
                    // Небольшой "объезд"
                    adjustedRoute[i] = new Coordinates(
                        adjustedRoute[i].Latitude + (_random.NextDouble() - 0.3) * 0.00003, // Меньший сдвиг
                        adjustedRoute[i].Longitude + (_random.NextDouble() - 0.3) * 0.00003
                    );
                }
            }
            else
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: Обнаружено другое препятствие, применяем стандартную корректировку (небольшой сдвиг).");
                for (int i = 0; i < adjustedRoute.Count; i++)
                {
                    // Еще меньший, общий сдвиг
                    adjustedRoute[i] = new Coordinates(
                        adjustedRoute[i].Latitude + (_random.NextDouble() - 0.5) * 0.00001,
                        adjustedRoute[i].Longitude + (_random.NextDouble() - 0.5) * 0.00001
                    );
                }
            }
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AdjustRoute: Скорректированный маршрут готов ({adjustedRoute.Count} точек).");
            return adjustedRoute;
        }

        /// <inheritdoc/>
        public void StartNavigation()
        {
            if (_isActive)
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation: Система GPS уже активна.");
                return;
            }

            // Имитация возможности сбоя при включении модуля
            if (_random.Next(0, 10) == 0) // 10% шанс сбоя
            {
                _isActive = false;
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation: НЕУДАЧА АКТИВАЦИИ. GPS модуль не смог запуститься корректно.");
            }
            else
            {
                _isActive = true;
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation: Система GPS успешно активирована. Текущая симулированная позиция: {_currentSimulatedPosition}.");
            }
        }

        /// <inheritdoc/>
        public List<Coordinates> StartNavigation(Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3)
        {
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (с параметрами): Попытка активации системы GPS и расчета маршрута к {initialTargetPosition}.");

            this.StartNavigation(); // Сначала пытаемся активировать систему (или проверяем, что она уже активна)

            if (!_isActive)
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (с параметрами): Система GPS не активна после попытки запуска, первоначальный расчет маршрута невозможен.");
                return null;
            }

            // Если система активна, текущая симулированная позиция уже должна быть установлена через UpdateSimulatedPosition.
            // Если нет, то _currentSimulatedPosition будет (0,0) или последнее известное значение.
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (с параметрами): Система активна. Текущая позиция для расчета: {_currentSimulatedPosition}. Выполняется первоначальный расчет маршрута...");

            List<Coordinates> calculatedRoute = CalculateRoute(initialTargetPosition, initialBoundaries, initialPrecisionPoints);

            if (calculatedRoute == null || !calculatedRoute.Any())
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (с параметрами): Первоначальный расчет маршрута не удался или вернул пустой маршрут (несмотря на активный GPS).");
            }
            else
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StartNavigation (с параметрами): Первоначальный маршрут успешно рассчитан ({calculatedRoute.Count} точек).");
            }
            return calculatedRoute;
        }


        /// <inheritdoc/>
        public void StopNavigation()
        {
            if (!_isActive)
            {
                Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StopNavigation: Система GPS уже неактивна.");
                return;
            }
            _isActive = false;
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: StopNavigation: Система GPS деактивирована.");
        }

        /// <inheritdoc/>
        public void UpdateSimulatedPosition(Coordinates newPosition)
        {
            _currentSimulatedPosition = newPosition;
            Console.WriteLine($"[Navigation/GPSNavigationSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: UpdateSimulatedPosition: Симулированная позиция GPS обновлена на: {newPosition}.");
        }
    }
}