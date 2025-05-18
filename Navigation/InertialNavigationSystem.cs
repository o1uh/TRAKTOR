using Traktor.Interfaces;
using Traktor.DataModels;
using Traktor.Core;   // Добавлено для Logger

namespace Traktor.Navigation
{
    /// <summary>
    /// Реализация системы навигации на основе инерциальных датчиков (ИНС).
    /// Имитирует работу ИНС, включая накопление ошибки (дрейф) со временем.
    /// Используется как вспомогательная или резервная система навигации.
    /// </summary>
    public class InertialNavigationSystem : INavigationSystem
    {
        private Coordinates _currentEstimatedPosition; // Текущая оценка позиции с учетом дрейфа
        private DateTime _lastCalibrationTime;      // Время последней точной калибровки/выставки позиции
        private bool _isInitializedAndActive = false; // Флаг, что система активна и была инициализирована начальной позицией
        private static readonly Random _random = new Random();

        private const double DRIFT_RATE_PER_SECOND = 0.000001; // Условная скорость дрейфа (градусы в секунду)
        private const string SourceFilePath = "Navigation/InertialNavigationSystem.cs"; // Определяем константу

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="InertialNavigationSystem"/>.
        /// Система неактивна и требует инициализации начальной позицией.
        /// </summary>
        public InertialNavigationSystem()
        {
            _currentEstimatedPosition = new Coordinates(0, 0); // Начальное значение до инициализации
            _lastCalibrationTime = DateTime.MinValue;           // Не было калибровки
            _isInitializedAndActive = false;
            Logger.Instance.Info(SourceFilePath, "Инерциальная система навигации (ИНС) инициализирована. Ожидает активации и выставки начальной позиции.");
        }

        /// <inheritdoc/>
        public Coordinates GetPosition()
        {
            if (!_isInitializedAndActive)
            {
                Logger.Instance.Warning(SourceFilePath, $"GetPosition: ИНС НЕ АКТИВНА или НЕ ИНИЦИАЛИЗИРОВАНА. Возвращаем последнюю известную (возможно, невалидную) позицию: {_currentEstimatedPosition}.");
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
                // Лог применения дрейфа может быть частым, используем Debug
                Logger.Instance.Debug(SourceFilePath, $"GetPosition: Применен дрейф ({elapsedSeconds:F2}с). Новая оценка ИНС: {_currentEstimatedPosition}");
            }
            return _currentEstimatedPosition;
        }

        /// <inheritdoc/>
        public List<Coordinates> CalculateRoute(Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3)
        {
            if (!_isInitializedAndActive)
            {
                Logger.Instance.Warning(SourceFilePath, "CalculateRoute: ИНС НЕ АКТИВНА или НЕ ИНИЦИАЛИЗИРОВАНА, расчет маршрута невозможен.");
                return null;
            }

            Coordinates startPosition = this.GetPosition();
            Logger.Instance.Info(SourceFilePath, $"CalculateRoute: Расчет маршрута от текущей оценки ИНС ({startPosition}) до {targetPosition}. Промежуточных точек на сегмент: {precisionPoints}. Границы: {(boundaries == null ? "не заданы" : "заданы")}.");

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

            Logger.Instance.Info(SourceFilePath, $"CalculateRoute: Маршрут рассчитан, {route.Count} точек.");
            return route;
        }

        /// <inheritdoc/>
        public List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles)
        {
            if (!_isInitializedAndActive)
            {
                Logger.Instance.Warning(SourceFilePath, "AdjustRoute: ИНС НЕ АКТИВНА или НЕ ИНИЦИАЛИЗИРОВАНА, корректировка невозможна.");
                return null;
            }

            int currentRouteCount = currentRoute?.Count ?? 0;
            int obstaclesCount = detectedObstacles?.Count ?? 0;
            Logger.Instance.Info(SourceFilePath, $"AdjustRoute: Запрос на корректировку маршрута ({currentRouteCount} точек) из-за {obstaclesCount} препятствий.");

            if (currentRoute == null || !currentRoute.Any())
            {
                Logger.Instance.Warning(SourceFilePath, "AdjustRoute: Нет оригинального маршрута для корректировки.");
                return null;
            }

            if (detectedObstacles == null || !detectedObstacles.Any())
            {
                Logger.Instance.Info(SourceFilePath, "AdjustRoute: Корректировка не требуется (препятствий нет). Возвращаем копию текущего маршрута.");
                return new List<Coordinates>(currentRoute);
            }

            if (_random.Next(0, 3) == 0)
            {
                Logger.Instance.Warning(SourceFilePath, "AdjustRoute: Имитация: корректировка маршрута с помощью ИНС НЕВОЗМОЖНА.");
                return null;
            }

            List<Coordinates> adjustedRoute = new List<Coordinates>(currentRoute);
            Logger.Instance.Info(SourceFilePath, "AdjustRoute: Имитация стандартной корректировки маршрута для ИНС (небольшой сдвиг).");
            for (int i = 0; i < adjustedRoute.Count; i++)
            {
                adjustedRoute[i] = new Coordinates(
                    adjustedRoute[i].Latitude + (_random.NextDouble() - 0.5) * 0.00002,
                    adjustedRoute[i].Longitude + (_random.NextDouble() - 0.5) * 0.00002
                );
            }
            Logger.Instance.Info(SourceFilePath, $"AdjustRoute: Скорректированный маршрут ИНС готов ({adjustedRoute.Count} точек).");
            return adjustedRoute;
        }

        /// <inheritdoc/>
        public void StartNavigation()
        {
            if (_isInitializedAndActive)
            {
                Logger.Instance.Info(SourceFilePath, "StartNavigation: ИНС уже активна и инициализирована.");
                return;
            }
            _isInitializedAndActive = false; // Убедимся, что флаг сброшен, если просто "включаем" без калибровки
            Logger.Instance.Info(SourceFilePath, "StartNavigation: ИНС модуль 'включен', но ТРЕБУЕТ КАЛИБРОВКИ начальной позиции для корректной работы.");
        }

        /// <inheritdoc/>
        public List<Coordinates> StartNavigation(Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3)
        {
            Logger.Instance.Info(SourceFilePath, $"StartNavigation (с параметрами): Попытка активации ИНС и расчета маршрута к {initialTargetPosition}.");

            if (!_isInitializedAndActive)
            {
                Logger.Instance.Warning(SourceFilePath, "StartNavigation (с параметрами): ИНС НЕ ИНИЦИАЛИЗИРОВАНА (не откалибрована начальная позиция через UpdateSimulatedPosition). Расчет маршрута невозможен.");
                return null;
            }

            Logger.Instance.Info(SourceFilePath, $"StartNavigation (с параметрами): ИНС активна и инициализирована. Текущая позиция для расчета: {_currentEstimatedPosition}. Выполняется первоначальный расчет маршрута...");

            List<Coordinates> calculatedRoute = CalculateRoute(initialTargetPosition, initialBoundaries, initialPrecisionPoints);

            if (calculatedRoute == null || !calculatedRoute.Any())
            {
                Logger.Instance.Warning(SourceFilePath, "StartNavigation (с параметрами): Первоначальный расчет маршрута не удался или вернул пустой маршрут.");
            }
            else
            {
                Logger.Instance.Info(SourceFilePath, $"StartNavigation (с параметрами): Первоначальный маршрут успешно рассчитан ({calculatedRoute.Count} точек).");
            }
            return calculatedRoute;
        }

        /// <inheritdoc/>
        public void StopNavigation()
        {
            if (!_isInitializedAndActive) // Если система и так не была инициализирована и активна
            {
                Logger.Instance.Info(SourceFilePath, "StopNavigation: ИНС уже неактивна или не была инициализирована.");
                return;
            }
            _isInitializedAndActive = false;
            Logger.Instance.Info(SourceFilePath, "StopNavigation: Система ИНС деактивирована.");
        }

        /// <inheritdoc/>
        public void UpdateSimulatedPosition(Coordinates newPosition)
        {
            _currentEstimatedPosition = newPosition;
            _lastCalibrationTime = DateTime.Now;
            _isInitializedAndActive = true;
            Logger.Instance.Info(SourceFilePath, $"UpdateSimulatedPosition (Калибровка ИНС): Позиция ИНС обновлена/откалибрована на: {newPosition}. Дрейф сброшен (счетчик времени дрейфа обнулен). Система АКТИВНА и ИНИЦИАЛИЗИРОВАНА.");
        }
    }
}