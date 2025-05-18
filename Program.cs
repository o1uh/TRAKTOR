using System.Drawing;
using Traktor.Core;
using Traktor.Sensors;
using Traktor.Navigation;
using Traktor.ComputerVision;
using Traktor.Implements;
using Traktor.Interfaces; 
using Traktor.DataModels;
using Traktor.Proxies;
using Traktor.Decorators;
using Traktor.Adapters;
using Traktor.TaskComponents;

namespace Traktor
{
    class Program
    {
        private const string SourceFilePath = "Program.cs";

        static void Main(string[] args)
        {
            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Приложение автопилота трактора запускается...");
            Logger.Instance.Info(SourceFilePath, $"Версия .NET: {Environment.Version}");
            Logger.Instance.Info(SourceFilePath, "==================================================");

            // --- Демонстрация структурных паттернов (ЛР 05-06) ---
            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Начало демонстрации структурных паттернов (ЛР 05-06)");
            Logger.Instance.Info(SourceFilePath, "==================================================");

            DemonstrateIteratorPattern();
            DemonstrateDecoratorPattern();
            DemonstrateAdapterPattern();
            DemonstrateCompositePattern();

            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Конец демонстрации структурных паттернов (ЛР 05-06)");
            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "\n");

            Logger.Instance.Info(SourceFilePath, "--- Начало основной логики приложения ---");
            try
            {
                // 1. Инициализация сенсоров и их прокси
                Logger.Instance.Info(SourceFilePath, "Инициализация сенсоров...");
                var distanceSensor = new DistanceSensor();
                ISensors<double> distanceSensorProxy = new SensorProxy<double>(() => distanceSensor, TimeSpan.FromMilliseconds(500));
                var soilSensor = new SoilSensor();
                ISensors<SoilSensorData> soilSensorProxy = new SensorProxy<SoilSensorData>(() => soilSensor, TimeSpan.FromSeconds(2));
                var cameraSensor = new CameraSensor();
                ISensors<Bitmap> cameraSensorProxy = new SensorProxy<Bitmap>(() => cameraSensor, TimeSpan.FromMilliseconds(200));
                Logger.Instance.Info(SourceFilePath, "Сенсоры и их прокси инициализированы.");

                // 2. Инициализация систем навигации
                Logger.Instance.Info(SourceFilePath, "Инициализация систем навигации...");
                INavigationSystem gpsNavigation = new GPSNavigationSystem();
                INavigationSystem inertialNavigation = new InertialNavigationSystem();
                Logger.Instance.Info(SourceFilePath, "Системы навигации инициализированы.");

                // 3. Инициализация систем компьютерного зрения и их прокси
                Logger.Instance.Info(SourceFilePath, "Инициализация систем технического зрения...");
                Func<IComputerVisionSystem> cameraVisionFactory = () => new CameraVisionSystem(cameraSensorProxy);
                Func<IComputerVisionSystem> lidarVisionFactory = () => new LidarVisionSystem();
                IComputerVisionSystem visionProxy = new VisionSystemProxy(cameraVisionFactory, TimeSpan.FromSeconds(1), lidarVisionFactory);
                Logger.Instance.Info(SourceFilePath, "Системы технического зрения и их прокси инициализированы.");

                // 4. Инициализация системы управления навесным оборудованием
                Logger.Instance.Info(SourceFilePath, "Инициализация системы управления навесным оборудованием...");
                var implementControl = new ImplementControlSystem();
                Logger.Instance.Info(SourceFilePath, "Система управления навесным оборудованием инициализирована.");

                // 5. Инициализация ControlUnit
                Logger.Instance.Info(SourceFilePath, "Инициализация ControlUnit...");
                var controlUnit = new ControlUnit(gpsNavigation, visionProxy, implementControl, distanceSensorProxy, soilSensorProxy, cameraSensorProxy);
                Logger.Instance.Info(SourceFilePath, "ControlUnit инициализирован.");

                // 6. Инициализация и запуск UserInterface
                Logger.Instance.Info(SourceFilePath, "Инициализация UserInterface...");
                var userInterface = new UserInterface(controlUnit);
                Logger.Instance.Info(SourceFilePath, "UserInterface инициализирован. Запуск...");
                userInterface.Run();
            }
            catch (Exception ex)
            {
                Logger.Instance.Fatal(SourceFilePath, $"КРИТИЧЕСКАЯ ОШИБКА на старте/в процессе работы основной логики приложения: {ex.Message}", ex);
                Console.WriteLine($"КРИТИЧЕСКАЯ ОШИБКА: {ex.Message}");
                Console.WriteLine("Подробности см. в лог-файле.");
            }
            finally
            {
                Logger.Instance.Info(SourceFilePath, "==================================================");
                Logger.Instance.Info(SourceFilePath, "Приложение автопилота трактора завершает работу.");
                Logger.Instance.Info(SourceFilePath, "==================================================");
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        private static void DemonstrateIteratorPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Iterator ---");
            try
            {
                var routePoints = new List<Coordinates>
                {
                    new Coordinates(55.123456, 37.123456),
                    new Coordinates(55.223456, 37.223456),
                    new Coordinates(55.323456, 37.323456)
                };
                Logger.Instance.Info(SourceFilePath, $"Iterator Demo: Создан список из {routePoints.Count} координат для маршрута.");
                var tractorRoute = new Traktor.Navigation.TractorRoute(routePoints);
                Logger.Instance.Info(SourceFilePath, "Iterator Demo: Начинаем итерацию по TractorRoute с помощью foreach...");
                int pointIndex = 0;
                foreach (var point in tractorRoute)
                {
                    pointIndex++;
                    Logger.Instance.Info(SourceFilePath, $"Iterator Demo (Клиент): Получена точка #{pointIndex} из маршрута: {point}");
                }
                Logger.Instance.Info(SourceFilePath, $"Iterator Demo: Итерация по TractorRoute завершена. Всего обработано точек клиентом: {pointIndex}.");
                Logger.Instance.Info(SourceFilePath, $"Iterator Demo: Проверка GetPointCount() у tractorRoute: {tractorRoute.GetPointCount()} точек.");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Iterator Demo: Ошибка при демонстрации Iterator: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Iterator ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }

        private static void DemonstrateDecoratorPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Decorator ---");
            try
            {
                ISensors<double> realDistanceSensor = new Traktor.Sensors.DistanceSensor();
                Logger.Instance.Info(SourceFilePath, $"Decorator Demo: Создан реальный сенсор: {realDistanceSensor.GetType().FullName}");
                ISensors<double> loggedDistanceSensor = new Traktor.Decorators.LoggingSensorDecorator<double>(realDistanceSensor);
                Logger.Instance.Info(SourceFilePath, "Decorator Demo: Вызываем GetData() у декорированного (залогированного) DistanceSensor...");
                double distance = loggedDistanceSensor.GetData();
                Logger.Instance.Info(SourceFilePath, $"Decorator Demo: Клиент получил данные от декорированного DistanceSensor: {distance:F2} м");

                Logger.Instance.Info(SourceFilePath, "Decorator Demo: --- Демонстрация с SoilSensor ---");
                ISensors<SoilSensorData> realSoilSensor = new Traktor.Sensors.SoilSensor();
                Logger.Instance.Info(SourceFilePath, $"Decorator Demo: Создан реальный сенсор: {realSoilSensor.GetType().FullName}");
                ISensors<SoilSensorData> loggedSoilSensor = new Traktor.Decorators.LoggingSensorDecorator<SoilSensorData>(realSoilSensor);
                Logger.Instance.Info(SourceFilePath, "Decorator Demo: Вызываем GetData() у декорированного (залогированного) SoilSensor...");
                SoilSensorData soilData = loggedSoilSensor.GetData();
                Logger.Instance.Info(SourceFilePath, $"Decorator Demo: Клиент получил данные от декорированного SoilSensor: {soilData}");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Decorator Demo: Ошибка при демонстрации Decorator: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Decorator ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }

        private static void DemonstrateAdapterPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Adapter ---");
            try
            {
                var legacySystem = new Traktor.Adapters.LegacyDiagnosticsSystem();
                Logger.Instance.Info(SourceFilePath, $"Adapter Demo: Создана 'старая' система: {legacySystem.GetType().FullName}");
                Traktor.Interfaces.ITractorDiagnostics diagnosticsAdapter = new Traktor.Adapters.DiagnosticsAdapter(legacySystem);
                Logger.Instance.Info(SourceFilePath, "Adapter Demo: Вызываем GetDiagnosticInfo() у адаптера...");
                Dictionary<string, string> diagnosticInfo = diagnosticsAdapter.GetDiagnosticInfo();
                Logger.Instance.Info(SourceFilePath, "Adapter Demo: Клиент получил диагностическую информацию через адаптер:");
                if (diagnosticInfo.Count > 0)
                {
                    foreach (var kvp in diagnosticInfo)
                    {
                        Logger.Instance.Info(SourceFilePath, $"  - {kvp.Key}: {kvp.Value}");
                    }
                }
                else
                {
                    Logger.Instance.Info(SourceFilePath, "  - Диагностическая информация пуста или не была получена (см. предыдущие логи).");
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Adapter Demo: Ошибка при демонстрации Adapter: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Adapter ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }

        private static void DemonstrateCompositePattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Composite ---");
            try
            {
                Logger.Instance.Info(SourceFilePath, "Composite Demo: Создание иерархии задач...");

                Traktor.Interfaces.ITaskComponent ploughField = new Traktor.TaskComponents.SimpleTask("Вспахать поле");
                Traktor.Interfaces.ITaskComponent sowSeeds = new Traktor.TaskComponents.SimpleTask("Посеять семена");
                Traktor.Interfaces.ITaskComponent fertilizeField = new Traktor.TaskComponents.SimpleTask("Удобрить поле");
                Traktor.Interfaces.ITaskComponent harvestCrops = new Traktor.TaskComponents.SimpleTask("Собрать урожай");

                var preCultivation = new Traktor.TaskComponents.ComplexTask("Предпосевная обработка");
                preCultivation.AddSubTask(ploughField);
                preCultivation.AddSubTask(new Traktor.TaskComponents.SimpleTask("Боронование"));

                var mainCultivation = new Traktor.TaskComponents.ComplexTask("Основной цикл работ");
                mainCultivation.AddSubTask(preCultivation);
                mainCultivation.AddSubTask(sowSeeds);
                mainCultivation.AddSubTask(fertilizeField);

                var seasonalWork = new Traktor.TaskComponents.ComplexTask("Сезонные работы на поле");
                seasonalWork.AddSubTask(mainCultivation);
                seasonalWork.AddSubTask(harvestCrops);
                seasonalWork.AddSubTask(new Traktor.TaskComponents.SimpleTask("Подготовка поля к зиме"));

                Logger.Instance.Info(SourceFilePath, "Composite Demo: Иерархия задач создана. Запускаем выполнение корневой задачи...");
                seasonalWork.Execute();

                Logger.Instance.Info(SourceFilePath, "Composite Demo: Демонстрация выполнения отдельной подзадачи...");
                preCultivation.Execute("  (Отдельный вызов) ");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Composite Demo: Ошибка при демонстрации Composite: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Composite ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }
    }
}