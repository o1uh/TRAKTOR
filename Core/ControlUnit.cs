using Traktor.Interfaces;
using Traktor.DataModels;
using Traktor.Implements;
using System.Drawing; 

namespace Traktor.Core
{
    /// <summary>
    /// Центральный управляющий блок автопилота трактора.
    /// Координирует работу всех подсистем, принимает решения и управляет движением и операциями.
    /// Предоставляет данные для отображения в пользовательском интерфейсе.
    /// </summary>
    public class ControlUnit
    {
        private readonly INavigationSystem _navigationSystem;
        private readonly IComputerVisionSystem _computerVisionSystem;
        private readonly ImplementControlSystem _implementControl;

        // Сенсоры (прокси), данные которых могут понадобиться для отображения в UI или принятия решений
        private readonly ISensors<double> _distanceSensorProxy;
        private readonly ISensors<SoilSensorData> _soilSensorProxy;
        private readonly ISensors<Bitmap> _cameraSensorProxy;

        private List<Coordinates> _currentRoute = null;
        private bool _isOperating = false;
        private Coordinates _currentTargetPoint;

        private const string SourceFilePath = "Core/ControlUnit.cs";

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ControlUnit"/>.
        /// </summary>
        /// <param name="navigationSystem">Система навигации.</param>
        /// <param name="computerVisionSystem">Система технического зрения.</param>
        /// <param name="implementControl">Система управления навесным оборудованием.</param>
        /// <param name="distanceSensorProxy">Прокси датчика расстояния.</param>
        /// <param name="soilSensorProxy">Прокси датчика состояния почвы.</param>
        /// <param name="cameraSensorProxy">Прокси датчика камеры.</param>
        /// <exception cref="ArgumentNullException">Если один из обязательных параметров равен null.</exception>
        public ControlUnit(
            INavigationSystem navigationSystem,
            IComputerVisionSystem computerVisionSystem,
            ImplementControlSystem implementControl,
            ISensors<double> distanceSensorProxy,
            ISensors<SoilSensorData> soilSensorProxy,
            ISensors<Bitmap> cameraSensorProxy)
        {
            _navigationSystem = navigationSystem ?? throw new ArgumentNullException(nameof(navigationSystem));
            _computerVisionSystem = computerVisionSystem ?? throw new ArgumentNullException(nameof(computerVisionSystem));
            _implementControl = implementControl ?? throw new ArgumentNullException(nameof(implementControl));

            _distanceSensorProxy = distanceSensorProxy ?? throw new ArgumentNullException(nameof(distanceSensorProxy));
            _soilSensorProxy = soilSensorProxy ?? throw new ArgumentNullException(nameof(soilSensorProxy));
            _cameraSensorProxy = cameraSensorProxy ?? throw new ArgumentNullException(nameof(cameraSensorProxy));

            Logger.Instance.Info(SourceFilePath, "ControlUnit инициализирован со всеми необходимыми системами и сенсорами.");
        }

        /// <summary>
        /// Запускает основной цикл работы автопилота с указанной целью и оборудованием.
        /// </summary>
        /// <param name="initialTargetPosition">Начальная целевая позиция для построения первого маршрута.</param>
        /// <param name="fieldBoundaries">Границы поля (опционально).</param>
        /// <param name="attachedImplement">Тип подключенного навесного оборудования.</param>
        public void StartOperation(Coordinates initialTargetPosition, FieldBoundaries fieldBoundaries = null, ImplementType attachedImplement = ImplementType.None)
        {
            if (_isOperating)
            {
                Logger.Instance.Warning(SourceFilePath, "StartOperation: Работа автопилота уже запущена.");
                return;
            }

            Logger.Instance.Info(SourceFilePath, $"StartOperation: Запуск работы автопилота. Цель: {initialTargetPosition}, Оборудование: {attachedImplement}."); 

            _currentRoute = _navigationSystem.StartNavigation(initialTargetPosition, fieldBoundaries);

            if (_currentRoute == null || !_currentRoute.Any())
            {
                Logger.Instance.Error(SourceFilePath, "StartOperation: Не удалось построить первоначальный маршрут. Операция не может быть начата.");
                _navigationSystem.StopNavigation();
                return;
            }

            _implementControl.AttachImplement(attachedImplement);
            if (attachedImplement != ImplementType.None)
            {
                _implementControl.ActivateOperation();
                if (_implementControl.IsOperationActive())
                {
                    Logger.Instance.Info(SourceFilePath, $"StartOperation: Навесное оборудование '{attachedImplement}' успешно активировано.");
                }
                else
                {
                    Logger.Instance.Warning(SourceFilePath, $"StartOperation: Навесное оборудование '{attachedImplement}' НЕ было активировано (возможно, проблема в ImplementControlSystem).");
                }
            }
            else
            {
                Logger.Instance.Info(SourceFilePath, "StartOperation: Навесное оборудование не указано (None).");
            }

            _isOperating = true;
            if (_currentRoute.Any())
            {
                _currentTargetPoint = _currentRoute.FirstOrDefault();
                Logger.Instance.Info(SourceFilePath, $"StartOperation: Автопилот запущен. Маршрут построен ({_currentRoute.Count} точек). Текущая цель: {_currentTargetPoint}");
            }
            else
            {
                Logger.Instance.Error(SourceFilePath, "StartOperation: Маршрут пуст после попытки построения! Остановка.");
                StopOperation();
                return;
            }
        }

        /// <summary>
        /// Останавливает работу автопилота и всех связанных систем.
        /// </summary>
        public void StopOperation()
        {
            if (!_isOperating)
            {
                // Logger.Instance.Debug(SourceFilePath, "StopOperation: Работа автопилота уже была остановлена.");
                return;
            }
            Logger.Instance.Info(SourceFilePath, "StopOperation: Остановка работы автопилота...");
            _isOperating = false;
            _navigationSystem.StopNavigation();
            _implementControl.DeactivateOperation();
            _currentRoute = null;
            Logger.Instance.Info(SourceFilePath, "StopOperation: Работа автопилота остановлена.");
        }

        /// <summary>
        /// Выполняет один шаг симуляции работы автопилота: получает данные, принимает решения, имитирует движение.
        /// В реальной системе этот метод вызывался бы в цикле с определенной частотой.
        /// </summary>
        public void SimulateOneStep()
        {
            if (!_isOperating)
            {
                // Logger.Instance.Debug(SourceFilePath, "SimulateOneStep: Автопилот не запущен. Шаг симуляции не выполнен.");
                return;
            }

            Logger.Instance.Debug(SourceFilePath, "SimulateOneStep: Начало шага симуляции.");

            // 1. Получение текущей позиции
            Coordinates currentPosition = _navigationSystem.GetPosition();
            Logger.Instance.Debug(SourceFilePath, $"SimulateOneStep: Текущая позиция трактора: {currentPosition}");

            if (_currentRoute == null || !_currentRoute.Any())
            {
                Logger.Instance.Warning(SourceFilePath, "SimulateOneStep: Маршрут отсутствует или пуст. Возможно, операция была остановлена. Пропуск шага.");
                if (_isOperating) StopOperation(); // Если маршрута нет, а мы работаем - останавливаемся.
                return;
            }

            // 2. Имитация движения и проверка достижения цели
            double distanceToTarget = CalculateDistance(currentPosition, _currentTargetPoint);
            Logger.Instance.Debug(SourceFilePath, $"SimulateOneStep: Расстояние до текущей цели ({_currentTargetPoint}): {distanceToTarget:F5}");

            if (distanceToTarget < 0.00005) // Условная "близость" к точке для смены цели
            {
                Logger.Instance.Info(SourceFilePath, $"SimulateOneStep: Точка маршрута {_currentTargetPoint} достигнута.");
                int currentIndex = _currentRoute.IndexOf(_currentTargetPoint);
                if (currentIndex < _currentRoute.Count - 1)
                {
                    _currentTargetPoint = _currentRoute[currentIndex + 1];
                    Logger.Instance.Info(SourceFilePath, $"SimulateOneStep: Новая цель по маршруту: {_currentTargetPoint}");
                }
                else
                {
                    Logger.Instance.Info(SourceFilePath, "SimulateOneStep: Конечная точка маршрута достигнута. Завершение операции.");
                    StopOperation();
                    return;
                }
            }
            else
            {
                // Имитируем движение к _currentTargetPoint
                Coordinates nextSimulatedPosition = MoveTowards(currentPosition, _currentTargetPoint, 0.00002); // Условный "шаг" движения
                _navigationSystem.UpdateSimulatedPosition(nextSimulatedPosition); // Обновляем симулированную позицию в навигационной системе
                Logger.Instance.Debug(SourceFilePath, $"SimulateOneStep: Трактор 'перемещен' к {nextSimulatedPosition} (движение к цели {_currentTargetPoint})");
            }

            // 3. Обнаружение препятствий
            List<ObstacleData> obstacles = _computerVisionSystem.DetectObstacles(currentPosition);
            if (obstacles.Any())
            {
                Logger.Instance.Warning(SourceFilePath, $"SimulateOneStep: Обнаружены препятствия ({obstacles.Count} шт.)!");
                foreach (var obs in obstacles)
                {
                    Logger.Instance.Warning(SourceFilePath, $"  - Препятствие: {obs.Description} в {obs.Position}");
                }

                // Попытка скорректировать маршрут
                List<Coordinates> adjustedRoute = _navigationSystem.AdjustRoute(new List<Coordinates>(_currentRoute), obstacles); // Передаем копию
                if (adjustedRoute != null && adjustedRoute.Any())
                {
                    _currentRoute = adjustedRoute;
                    _currentTargetPoint = _currentRoute.FirstOrDefault(); // Обновляем текущую цель на скорректированном маршруте
                    Logger.Instance.Info(SourceFilePath, $"SimulateOneStep: Маршрут скорректирован из-за препятствий. Новый маршрут: {_currentRoute.Count} точек. Новая цель: {_currentTargetPoint}");
                }
                else
                {
                    Logger.Instance.Error(SourceFilePath, "SimulateOneStep: Не удалось скорректировать маршрут для обхода препятствий! ОСТАНОВКА ОПЕРАЦИЙ.");
                    StopOperation(); // Критическая ситуация, останавливаемся
                    return;
                }
            }

            // 4. Получение данных с других сенсоров 
            double frontDistance = _distanceSensorProxy.GetData();
            SoilSensorData soilData = _soilSensorProxy.GetData();
            Bitmap cameraFrame = _cameraSensorProxy.GetData(); // Условно, дабы показать, что компиляция проходит

            Logger.Instance.Debug(SourceFilePath, $"SimulateOneStep: Данные датчиков (для информации/UI): Расстояние={frontDistance:F2}м, Почва=[{soilData}]");

            // 5. Принятие решений на основе данных сенсоров (пример)
            if (frontDistance < 1.0 && frontDistance > 0) // Слишком близко к чему-то по данным переднего датчика
            {
                Logger.Instance.Warning(SourceFilePath, $"SimulateOneStep: КРИТИЧЕСКОЕ СБЛИЖЕНИЕ ({frontDistance:F2}м)! Требуются экстренные меры (в макете - остановка).");
                StopOperation();
                return; 
            }

            // Здесь могла бы быть логика, использующая soilData для адаптивного управления ImplementControlSystem,
            // или данные с cameraFrame (AnalyzeFieldFeatures) для специфических действий.

            Logger.Instance.Debug(SourceFilePath, "SimulateOneStep: Шаг симуляции завершен.");
        }

        /// <summary>
        /// Вспомогательный метод для имитации движения трактора к целевой точке.
        /// </summary>
        private Coordinates MoveTowards(Coordinates current, Coordinates target, double stepSize)
        {
            double dx = target.Longitude - current.Longitude;
            double dy = target.Latitude - current.Latitude;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            if (distance < stepSize || distance == 0) return target; // Если уже у цели или очень близко

            // Перемещаемся на stepSize в направлении цели
            return new Coordinates(
                current.Latitude + dy / distance * stepSize,
                current.Longitude + dx / distance * stepSize
            );
        }

        /// <summary>
        /// Вспомогательный метод для расчета евклидова расстояния между двумя координатами (упрощенно).
        /// </summary>
        private double CalculateDistance(Coordinates p1, Coordinates p2)
        {
            double dx = p1.Longitude - p2.Longitude;
            double dy = p1.Latitude - p2.Latitude;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // --- Публичные свойства и методы для доступа извне ---

        /// <summary>
        /// Возвращает true, если автопилот в данный момент выполняет операцию.
        /// </summary>
        public bool IsOperating => _isOperating;

        /// <summary>
        /// Получает копию текущего активного маршрута.
        /// </summary>
        /// <returns>Список координат маршрута или null, если маршрут не задан.</returns>
        public List<Coordinates> GetCurrentRoute() => _currentRoute != null ? new List<Coordinates>(_currentRoute) : null;

        /// <summary>
        /// Получает текущую оцененную позицию трактора от навигационной системы.
        /// </summary>
        public Coordinates GetCurrentPosition() => _navigationSystem.GetPosition(); // Всегда запрашиваем актуальную позицию

        /// <summary>
        /// Получает текущую целевую точку на маршруте.
        /// </summary>
        public Coordinates GetCurrentTargetPoint() => _currentTargetPoint;

        /// <summary>
        /// Предоставляет доступ к системе управления навесным оборудованием.
        /// </summary>
        public ImplementControlSystem ImplementControl => _implementControl; // Прямой доступ для UI для управления/отображения

        /// <summary>
        /// Получает текущее значение с датчика расстояния.
        /// </summary>
        public double GetCurrentDistanceValue() => _distanceSensorProxy.GetData();

        /// <summary>
        /// Получает текущие данные о состоянии почвы.
        /// </summary>
        public SoilSensorData GetCurrentSoilData() => _soilSensorProxy.GetData();

        /// <summary>
        /// Получает текущий кадр с камеры. 
        /// <remarks>Вызывающий код ответственен за вызов Dispose() для возвращенного Bitmap.</remarks>
        /// </summary>
        public Bitmap GetCurrentCameraFrame() => _cameraSensorProxy.GetData();
    }
}