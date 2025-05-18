using System;
using System.Collections.Generic;
using System.Linq; // Для .Any()
using Traktor.Interfaces;
using Traktor.DataModels;
using Traktor.Navigation;       // Для GPSNavigationSystem, InertialNavigationSystem
using Traktor.ComputerVision;   // Для CameraVisionSystem, LidarVisionSystem
using Traktor.Sensors;          // Для конкретных сенсоров
using Traktor.Implements;       // Для ImplementControlSystem
using System.Drawing;
// using System.Threading;      // Если будем делать цикл с задержками

namespace Traktor.Core
{
    // Перечисление для текущего состояния ControlUnit (очень простое)
    public enum TractorSystemState
    {
        Idle,               // Ожидание команд
        Navigating,         // Движение по маршруту
        FieldOperation,     // Выполнение полевой операции (может быть совмещено с Navigating)
        Error,              // Критическая ошибка
        AwaitingOperator    // Ожидание решения оператора
    }

    public class ControlUnit
    {
        // --- Зависимости (компоненты системы) ---
        private INavigationSystem _activeNavigationSystem; // Активная система навигации
        private GPSNavigationSystem _gpsNavigationSystem;
        private InertialNavigationSystem _inertialNavigationSystem;

        private IComputerVisionSystem _activeVisionSystem; // Активная система зрения
        private CameraVisionSystem _cameraVisionSystem;
        private LidarVisionSystem _lidarVisionSystem;

        // Конкретные сенсоры (можно было бы сделать через ISensors и список, но для макета так проще)
        private ISensors<double> _distanceSensor;   // Датчик расстояния (например, передний)
        private ISensors<Bitmap> _cameraSensor;     // Камера как источник Bitmap
        private ISensors<SoilSensorData> _soilSensor; // Датчик почвы

        private ImplementControlSystem _implementControl;
        private UserInterface _ui;

        // --- Внутреннее состояние ControlUnit ---
        private Coordinates _currentTractorPosition;
        private List<Coordinates> _currentRoute;
        private int _currentTargetPointIndex = -1;
        private TractorSystemState _currentState = TractorSystemState.Idle;
        private bool _isRunning = true;

        // Настройки (могли бы загружаться из файла)
        private NavigationPreference _userNavPreference = NavigationPreference.PreferGPS; // Предпочтение пользователя

        public enum NavigationPreference
        {
            PreferGPS,
            PreferINS,
            GPSOnly,
            INSOnly
        }

        public ControlUnit()
        {
            _ui = new UserInterface();

            // Инициализация навигационных систем
            // Начальная позиция для инициализации самих объектов, не обязательно текущая трактора
            _gpsNavigationSystem = new GPSNavigationSystem();
            _inertialNavigationSystem = new InertialNavigationSystem();
            // _activeNavigationSystem будет выбрана позже

            // Инициализация систем компьютерного зрения
            _cameraVisionSystem = new CameraVisionSystem();
            _lidarVisionSystem = new LidarVisionSystem();
            // _activeVisionSystem будет выбрана позже

            // Инициализация сенсоров
            _distanceSensor = new DistanceSensor();
            _cameraSensor = new CameraSensor();
            _soilSensor = new SoilSensor();

            _implementControl = new ImplementControlSystem();

            // Начальная позиция трактора (могла бы быть получена при первом запуске)
            _currentTractorPosition = new Coordinates(0, 0); // Стартуем с (0,0)
            _ui.DisplayMessage($"ControlUnit: Начальная позиция трактора установлена в {_currentTractorPosition}");

            // Сообщаем начальную позицию навигационным системам (для их внутреннего состояния)
            _gpsNavigationSystem.UpdateSimulatedPosition(_currentTractorPosition);
            _inertialNavigationSystem.UpdateSimulatedPosition(_currentTractorPosition);
        }

        public void Run()
        {
            _ui.DisplayMessage("Система управления трактором ЗАПУЩЕНА.");
            _ui.DisplayMessage($"Текущая симулированная позиция: {_currentTractorPosition}");
            SetInitialActiveSystems(); // Выбираем активные системы при старте

            while (_isRunning)
            {
                _ui.ShowAvailableCommands();
                string command = _ui.GetOperatorCommand();
                ProcessCommand(command);

                // В реальной системе здесь был бы цикл обновления состояния,
                // автоматическое движение, если есть маршрут, и т.д.
                // Для макета мы управляем через команды.
                // SimulateStep(); // Если бы была автоматическая симуляция
                // Thread.Sleep(1000); // Задержка
            }
            _ui.DisplayMessage("Система управления трактором ОСТАНОВЛЕНА.");
        }

        private void SetInitialActiveSystems()
        {
            // Выбор навигационной системы
            // Пытаемся запустить предпочтительную, если не вышло - резервную.
            bool navSysStarted = false;
            if (_userNavPreference == NavigationPreference.PreferGPS || _userNavPreference == NavigationPreference.GPSOnly)
            {
                _ui.DisplayMessage("ControlUnit: Попытка запуска GPS как основной навигационной системы...");
                // Для первого запуска StartAndCalculateInitialRoute не вызываем, т.к. нет целевой точки
                // Просто активируем систему. Маршрут будет рассчитан по команде.
                if (_gpsNavigationSystem.Sta (_currentTractorPosition)) // Передаем текущую позицию для выставки
                {
                    _activeNavigationSystem = _gpsNavigationSystem;
                    _ui.DisplayMessage("ControlUnit: GPS успешно активирован.");
                    navSysStarted = true;
                }
                else
                {
                    _ui.ShowError("ControlUnit: Не удалось запустить GPS.");
                    if (_userNavPreference == NavigationPreference.GPSOnly) _currentState = TractorSystemState.Error;
                }
            }

            if (!navSysStarted && (_userNavPreference == NavigationPreference.PreferINS || _userNavPreference == NavigationPreference.INSOnly || _userNavPreference == NavigationPreference.PreferGPS))
            {
                _ui.DisplayMessage("ControlUnit: Попытка запуска ИНС как навигационной системы...");
                if (_inertialNavigationSystem.StartNavigation(_currentTractorPosition)) // ИНС тоже выставляем по текущей
                {
                    _activeNavigationSystem = _inertialNavigationSystem;
                    _ui.DisplayMessage("ControlUnit: ИНС успешно активирована.");
                    navSysStarted = true;
                }
                else
                {
                    _ui.ShowError("ControlUnit: Не удалось запустить ИНС.");
                    if (_userNavPreference == NavigationPreference.INSOnly || !navSysStarted) _currentState = TractorSystemState.Error;
                }
            }

            if (!navSysStarted)
            {
                _ui.ShowError("ControlUnit: Ни одна навигационная система не смогла запуститься! Работа невозможна.");
                _currentState = TractorSystemState.Error;
            }

            // Выбор системы компьютерного зрения (пока просто выбираем камеру, если доступна)
            if (_cameraVisionSystem != null) // Предполагаем, что она всегда "создается" успешно
            {
                _activeVisionSystem = _cameraVisionSystem;
                _activeVisionSystem.ActivateSystem(); // Активируем ее (если есть такой метод)
                _ui.DisplayMessage("ControlUnit: CameraVisionSystem выбрана как активная.");
            }
            else if (_lidarVisionSystem != null)
            {
                _activeVisionSystem = _lidarVisionSystem;
                _activeVisionSystem.ActivateSystem();
                _ui.DisplayMessage("ControlUnit: LidarVisionSystem выбрана как активная (камера недоступна).");
            }
            else
            {
                _ui.ShowWarning("ControlUnit: Системы компьютерного зрения не доступны.");
            }
        }


        private void ProcessCommand(string commandString)
        {
            if (string.IsNullOrWhiteSpace(commandString)) return;

            string[] parts = commandString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0];

            if (_currentState == TractorSystemState.Error && command != "exit")
            {
                _ui.ShowError("Система в состоянии ошибки. Доступна только команда 'exit'.");
                return;
            }

            try
            {
                switch (command)
                {
                    case "start": // Пример: start gps 55.5 37.3  или start ins 55.5 37.3
                        HandleStartNavigationCommand(parts);
                        break;
                    case "stop":
                        if (parts.Length > 1 && parts[1] == "nav") HandleStopNavigationCommand();
                        else _ui.ShowWarning("Неизвестная подкоманда для 'stop'. Используйте 'stop nav'.");
                        break;
                    case "status":
                        DisplayFullStatus();
                        break;
                    case "move":
                        SimulateSingleStep();
                        break;
                    case "scan": // Пример: scan obstacles или scan features
                        HandleScanCommand(parts);
                        break;
                    case "attach": // Пример: attach плуг
                        HandleAttachImplementCommand(parts);
                        break;
                    case "activate":
                        if (parts.Length > 1 && parts[1] == "imp") _implementControl.ActivateImplement();
                        else _ui.ShowWarning("Неизвестная подкоманда для 'activate'. Используйте 'activate imp'.");
                        break;
                    case "deactivate":
                        if (parts.Length > 1 && parts[1] == "imp") _implementControl.DeactivateImplement();
                        else _ui.ShowWarning("Неизвестная подкоманда для 'deactivate'. Используйте 'deactivate imp'.");
                        break;
                    case "set": // Пример: set depth 0.2
                        HandleSetImplementParameterCommand(parts);
                        break;
                    case "exit":
                        _isRunning = false;
                        break;
                    default:
                        _ui.ShowWarning($"Неизвестная команда: {commandString}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _ui.ShowError($"Ошибка при выполнении команды: {ex.Message}");
                // Здесь можно добавить логирование исключения с помощью Logger
            }
        }

        private void HandleStartNavigationCommand(string[] parts)
        {
            if (parts.Length < 4)
            {
                _ui.ShowWarning("Использование: start <gps|ins> <lat_цели> <lon_цели>");
                return;
            }

            string navType = parts[1].ToLower();
            if (!double.TryParse(parts[2], out double lat) || !double.TryParse(parts[3], out double lon))
            {
                _ui.ShowWarning("Неверный формат координат цели.");
                return;
            }
            Coordinates targetPos = new Coordinates(lat, lon);

            INavigationSystem selectedNavSystem = null;
            string selectedNavSystemName = "";

            if (navType == "gps") { selectedNavSystem = _gpsNavigationSystem; selectedNavSystemName = "GPS"; }
            else if (navType == "ins") { selectedNavSystem = _inertialNavigationSystem; selectedNavSystemName = "ИНС"; }
            else { _ui.ShowWarning("Неизвестный тип навигационной системы: " + navType); return; }

            _ui.DisplayMessage($"ControlUnit: Попытка запуска {selectedNavSystemName} с целью {targetPos} от текущей позиции {_currentTractorPosition}.");

            // Теперь StartAndCalculateInitialRoute принимает начальную позицию для выставки
            // и целевую для первого маршрута.
            _currentRoute = selectedNavSystem.StartAndCalculateInitialRoute(_currentTractorPosition, targetPos);

            if (_currentRoute != null && _currentRoute.Any())
            {
                _activeNavigationSystem = selectedNavSystem; // Устанавливаем как активную только при успехе
                _currentTargetPointIndex = 0;
                _currentState = TractorSystemState.Navigating;
                _ui.DisplayMessage($"{selectedNavSystemName} успешно активирована. Маршрут рассчитан. {_currentRoute.Count} точек.");
                _ui.DisplayRoute(_currentRoute, _currentTargetPointIndex);
            }
            else
            {
                _ui.ShowError($"Не удалось запустить {selectedNavSystemName} или рассчитать маршрут.");
                // _activeNavigationSystem не меняем, если запуск не удался
                // _currentState может остаться Idle или перейти в AwaitingOperator
            }
        }

        private void HandleStopNavigationCommand()
        {
            if (_activeNavigationSystem != null)
            {
                _activeNavigationSystem.StopNavigation();
                _ui.DisplayMessage($"ControlUnit: Активная навигационная система ({_activeNavigationSystem.GetType().Name}) остановлена.");
                _activeNavigationSystem = null; // Сбрасываем активную систему
            }
            else
            {
                _ui.DisplayMessage($"ControlUnit: Нет активной навигационной системы для остановки.");
            }
            _currentRoute = null;
            _currentTargetPointIndex = -1;
            _currentState = TractorSystemState.Idle;
        }

        private void DisplayFullStatus()
        {
            _ui.DisplayStatus($"Текущее состояние системы: {_currentState}");
            _currentTractorPosition = UpdateAndGetActualPosition(); // Обновляем позицию перед отображением
            _ui.DisplayTractorPosition(_currentTractorPosition);

            if (_activeNavigationSystem != null)
            {
                _ui.DisplayMessage($"Активная навигационная система: {_activeNavigationSystem.GetType().Name}, Готовность: {_activeNavigationSystem.IsReady()}");
            }
            else
            {
                _ui.DisplayMessage("Активная навигационная система: не выбрана/не активна.");
            }
            _ui.DisplayRoute(_currentRoute, _currentTargetPointIndex);
            // Можно добавить вывод состояния других систем
        }

        private Coordinates UpdateAndGetActualPosition()
        {
            if (_activeNavigationSystem != null && _activeNavigationSystem.IsReady())
            {
                // Для GPS, GetPosition() считывает "текущее" значение.
                // Для ИНС, GetPosition() вычисляет на основе дрейфа.
                // _currentTractorPosition обновляется ControlUnit'ом после "шага" движения.
                // Здесь мы просто запрашиваем то, что система навигации думает о текущей позиции.
                return _activeNavigationSystem.GetPosition();
            }
            // Если нет активной навигации, возвращаем последнюю известную позицию трактора
            // или начальную, если движения не было.
            // Важно: _currentTractorPosition обновляется методом SimulateSingleStep.
            return _currentTractorPosition;
        }


        private void SimulateSingleStep()
        {
            if (_currentState != TractorSystemState.Navigating || _currentRoute == null || !_currentRoute.Any() || _activeNavigationSystem == null || !_activeNavigationSystem.IsReady())
            {
                _ui.DisplayMessage("ControlUnit: Нет активного маршрута или навигационной системы для имитации движения.");
                return;
            }

            if (_currentTargetPointIndex < 0 || _currentTargetPointIndex >= _currentRoute.Count)
            {
                _ui.DisplayMessage("ControlUnit: Достигнут конец маршрута или неверный индекс цели.");
                HandleStopNavigationCommand(); // Завершаем навигацию
                return;
            }

            Coordinates targetPoint = _currentRoute[_currentTargetPointIndex];
            _ui.DisplayMessage($"ControlUnit: Имитация шага движения к точке {targetPoint} (индекс {_currentTargetPointIndex}).");

            // --- Имитация движения трактора ---
            // В реальности здесь были бы команды на СУДТ
            // Для макета, просто "перемещаем" _currentTractorPosition немного к цели
            double stepSize = 0.1; // Условный размер шага к цели (например, 10% от оставшегося расстояния до точки)
                                   // или фиксированный шаг, например, 0.0001 в единицах координат

            _currentTractorPosition = new Coordinates(
                _currentTractorPosition.Latitude + (targetPoint.Latitude - _currentTractorPosition.Latitude) * stepSize,
                _currentTractorPosition.Longitude + (targetPoint.Longitude - _currentTractorPosition.Longitude) * stepSize
            );

            // Сообщаем навигационной системе новую позицию (важно для ИНС и для GPS в нашей симуляции)
            _activeNavigationSystem.UpdateSimulatedPosition(_currentTractorPosition);
            _ui.DisplayTractorPosition(_currentTractorPosition);

            // Проверка достижения цели (очень упрощенная)
            double distanceToTarget = Math.Sqrt(Math.Pow(targetPoint.Latitude - _currentTractorPosition.Latitude, 2) + Math.Pow(targetPoint.Longitude - _currentTractorPosition.Longitude, 2));
            if (distanceToTarget < 0.00005) // Порог достижения цели
            {
                _ui.DisplayMessage($"ControlUnit: Точка {targetPoint} достигнута!");
                _currentTargetPointIndex++;
                if (_currentTargetPointIndex >= _currentRoute.Count)
                {
                    _ui.DisplayMessage("ControlUnit: Весь маршрут пройден!");
                    HandleStopNavigationCommand();
                }
                else
                {
                    _ui.DisplayMessage($"ControlUnit: Следующая цель: {_currentRoute[_currentTargetPointIndex]} (индекс {_currentTargetPointIndex}).");
                    _ui.DisplayRoute(_currentRoute, _currentTargetPointIndex);
                }
            }
        }


        private void HandleScanCommand(string[] parts)
        {
            if (_activeVisionSystem == null) { _ui.ShowWarning("Система компьютерного зрения не активна."); return; }
            if (parts.Length < 2) { _ui.ShowWarning("Использование: scan <obstacles|features>"); return; }

            // Для сканирования нам нужен "кадр" с камеры и, возможно, данные с дальномера
            Bitmap currentFrame = _cameraSensor.GetData(); // Получаем кадр
            double? frontDistance = _distanceSensor.GetData(); // Получаем расстояние (может быть просто double, не ISensors<double>)

            // Для LidarVisionSystem мы передадим null для cameraFrame и distanceData, и что-то для lidarRawData (пока null)
            object lidarDataForLidarSystem = null; // Заглушка для "сырых" LiDAR данных


            if (parts[1] == "obstacles")
            {
                List<ObstacleData> obstacles;
                if (_activeVisionSystem is LidarVisionSystem)
                {
                    obstacles = _activeVisionSystem.DetectObstacles(null, null, lidarDataForLidarSystem, _currentTractorPosition);
                }
                else // CameraVisionSystem или другая
                {
                    obstacles = _activeVisionSystem.DetectObstacles(currentFrame, frontDistance, null, _currentTractorPosition);
                }
                _ui.DisplayObstacles(obstacles);

                if (obstacles != null && obstacles.Any() && _currentRoute != null && _currentRoute.Any())
                {
                    if (_ui.RequestConfirmation("Обнаружены препятствия. Попытаться скорректировать маршрут?"))
                    {
                        if (_activeNavigationSystem == null || !_activeNavigationSystem.IsReady())
                        {
                            _ui.ShowError("Невозможно скорректировать маршрут: навигационная система не активна/не готова.");
                            return;
                        }
                        _ui.DisplayMessage("ControlUnit: Попытка корректировки маршрута...");
                        List<Coordinates> newRoute = _activeNavigationSystem.AdjustRoute(_currentRoute, obstacles);
                        if (newRoute != null)
                        {
                            _currentRoute = newRoute;
                            // _currentTargetPointIndex нужно будет переопределить, возможно, на 0 или ближайшую точку
                            _currentTargetPointIndex = 0; // Для простоты начинаем новый маршрут с начала
                            _ui.DisplayMessage("ControlUnit: Маршрут скорректирован.");
                            _ui.DisplayRoute(_currentRoute, _currentTargetPointIndex);
                        }
                        else
                        {
                            _ui.ShowError("ControlUnit: Не удалось скорректировать маршрут. Движение остановлено.");
                            HandleStopNavigationCommand();
                            _currentState = TractorSystemState.AwaitingOperator;
                        }
                    }
                }
            }
            else if (parts[1] == "features")
            {
                List<FieldFeatureData> features;
                if (_activeVisionSystem is LidarVisionSystem)
                {
                    features = _activeVisionSystem.AnalyzeFieldFeatures(null, null, _currentTractorPosition); // Lidar не использует кадр/дистанцию
                }
                else
                {
                    features = _activeVisionSystem.AnalyzeFieldFeatures(currentFrame, frontDistance, _currentTractorPosition);
                }
                _ui.DisplayFieldFeatures(features);
            }
            else
            {
                _ui.ShowWarning("Неизвестный тип сканирования. Используйте 'obstacles' или 'features'.");
            }
        }

        private void HandleAttachImplementCommand(string[] parts)
        {
            if (parts.Length < 2) { _ui.ShowWarning("Использование: attach <тип_оборудования>"); return; }
            string typeStr = parts[1].ToLower();
            ImplementType type;
            switch (typeStr)
            {
                case "плуг": type = ImplementType.Plough; break;
                case "сеялка": type = ImplementType.Seeder; break;
                case "опрыскиватель": type = ImplementType.Sprayer; break;
                default: _ui.ShowWarning("Неизвестный тип оборудования."); return;
            }
            _implementControl.AttachImplement(type);
        }

        private void HandleSetImplementParameterCommand(string[] parts)
        {
            if (parts.Length < 3) { _ui.ShowWarning("Использование: set <depth|rate|intensity> <значение>"); return; }
            if (!double.TryParse(parts[2], out double value)) { _ui.ShowWarning("Неверное значение параметра."); return; }

            string param = parts[1].ToLower();
            switch (param)
            {
                case "depth": _implementControl.SetPloughDepth(value); break;
                case "rate": _implementControl.SetSeederRate(value); break;
                case "intensity": _implementControl.SetSprayerIntensity(value); break;
                default: _ui.ShowWarning("Неизвестный параметр оборудования."); break;
            }
        }
    }
}