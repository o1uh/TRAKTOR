using System.Drawing;
using Traktor.Adapters;
using Traktor.Builders;
using Traktor.ComputerVision;
using Traktor.Core;
using Traktor.DataModels;
using Traktor.Decorators;
using Traktor.Implements;
using Traktor.Interfaces; 
using Traktor.Navigation;
using Traktor.Proxies;
using Traktor.Sensors;
using Traktor.TaskComponents;
using Traktor.Commands;
using Traktor.Mocks;

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

            // --- Демонстрация паттерна Command---
            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Начало демонстрации паттерна Command (ЛР 14-15)");
            Logger.Instance.Info(SourceFilePath, "==================================================");

            DemonstrateCommandPattern();

            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Конец демонстрации паттерна Command (ЛР 14-15)");
            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "\n");

            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Начало демонстрации паттерна Chain of Responsibility (ЛР 14-15)");
            Logger.Instance.Info(SourceFilePath, "==================================================");

            DemonstrateChainOfResponsibilityPattern();

            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Конец демонстрации паттерна Chain of Responsibility (ЛР 14-15)");
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

        private static void DemonstrateBuilderPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Builder (ЛР 11) ---");
            try
            {
                Logger.Instance.Info(SourceFilePath, "Builder Demo: --- Прямое использование Строителя ---");
                ITractorConfigurationBuilder builder = new StandardTractorConfigurationBuilder();

                Logger.Instance.Info(SourceFilePath, "Builder Demo: Собираем кастомную конфигурацию...");
                TractorConfiguration customConfig = builder
                    .SetBaseParameters("CustomTraktor X100", EngineType.Hybrid)
                    .SetTransmission(TransmissionType.CVT)
                    .SetHorsePower(300)
                    .SetGPSModule(true)
                    .Build();

                Logger.Instance.Info(SourceFilePath, "Builder Demo: Кастомная конфигурация собрана:");
                customConfig.DisplayConfiguration();

                Logger.Instance.Info(SourceFilePath, "Builder Demo: --- Использование Директора ---");
                TractorConfigurationDirector director = new TractorConfigurationDirector(builder);

                Logger.Instance.Info(SourceFilePath, "Builder Demo: Директор собирает базовую конфигурацию...");
                director.ConstructBasicTractor("TerraForce Basic");
                TractorConfiguration basicConfig = builder.Build();
                Logger.Instance.Info(SourceFilePath, "Builder Demo: Базовая конфигурация собрана директором:");
                basicConfig.DisplayConfiguration();

                Logger.Instance.Info(SourceFilePath, "Builder Demo: Директор собирает продвинутую конфигурацию...");
                director.ConstructAdvancedTractor("AgroMaster Pro");
                TractorConfiguration advancedConfig = builder.Build();
                Logger.Instance.Info(SourceFilePath, "Builder Demo: Продвинутая конфигурация собрана директором:");
                advancedConfig.DisplayConfiguration();

                Logger.Instance.Info(SourceFilePath, "Builder Demo: Директор собирает электрическую конфигурацию...");
                director.ConstructElectricTractor("EcoVolt E-200");
                TractorConfiguration electricConfig = builder.Build();
                Logger.Instance.Info(SourceFilePath, "Builder Demo: Электрическая конфигурация собрана директором:");
                electricConfig.DisplayConfiguration();

                Logger.Instance.Info(SourceFilePath, "Builder Demo: --- Повторное использование Строителя (после сброса) ---");
                TractorConfiguration anotherCustomConfig = builder
                    .SetBaseParameters("MiniAgro", EngineType.Electric)
                    .SetHorsePower(50)
                    .Build();
                Logger.Instance.Info(SourceFilePath, "Builder Demo: Еще одна кастомная конфигурация собрана:");
                anotherCustomConfig.DisplayConfiguration();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Builder Demo: Ошибка при демонстрации Builder: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Builder (ЛР 11) ---");
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
        private static void DemonstrateCommandPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Command ---");
            try
            {
                // 1. Создаем Получателя (Receiver).
                var mockReceiver = new Traktor.Mocks.MockControlUnit(); ; // Используем наш простой макет

                // 2. Создаем Инициатора (Invoker)
                var invoker = new Traktor.Core.CommandInvoker();

                // 3. Создаем и выполняем команду Start
                Logger.Instance.Info(SourceFilePath, "Command Demo: --- Демонстрация StartAutopilotCommand ---");
                var startTarget = new Coordinates(50.0, 30.0);
                var startImplement = ImplementType.Seeder;
                ICommand startCommand = new Traktor.Commands.StartAutopilotCommand(mockReceiver, startTarget, startImplement);

                invoker.SetCommand(startCommand);
                invoker.ExecuteCommand();

                // 4. Создаем и выполняем команду Stop
                Logger.Instance.Info(SourceFilePath, "Command Demo: --- Демонстрация StopAutopilotCommand ---");
                ICommand stopCommand = new Traktor.Commands.StopAutopilotCommand(mockReceiver);

                invoker.SetCommand(stopCommand);
                invoker.ExecuteCommand();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Command Demo: Ошибка при демонстрации Command: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Command ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }
        private static void DemonstrateChainOfResponsibilityPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Chain of Responsibility ---");
            try
            {
                // 1. Создаем Получателя (Receiver) для команд - наш MockControlUnit
                // MockControlUnit находится в Traktor.Mocks, реализует IControlUnitCommands
                IControlUnitCommands mockReceiver = new Traktor.Mocks.MockControlUnit();

                // 2. Создаем обработчики
                // Обработчики находятся в Traktor.Commands
                ICommandHandler startHandler = new Traktor.Commands.StartCommandHandler();
                ICommandHandler stopHandler = new Traktor.Commands.StopCommandHandler();
                // Можно добавить UnknownCommandHandler в конец цепочки, если нужно обрабатывать нераспознанные команды
                // ICommandHandler unknownHandler = new Traktor.Commands.UnknownCommandHandler(); 

                // 3. Строим цепочку: start -> stop (-> unknown)
                Logger.Instance.Info(SourceFilePath, "Chain Demo: Построение цепочки обработчиков: StartHandler -> StopHandler.");
                startHandler.SetNext(stopHandler);
                // stopHandler.SetNext(unknownHandler); // Если есть unknownHandler

                // 4. Создаем Инициатора команд (если хотим выполнить полученные команды)
                // CommandInvoker находится в Traktor.Core
                var invoker = new Traktor.Core.CommandInvoker();

                // 5. Тестовые строки ввода
                string[] userInputs = {
                    "start 51.0 31.0 plough",
                    "do_something_else",
                    "stop",
                    "start 52", // Неверные аргументы для start
                    "help" // Неизвестная команда для текущей цепочки
                };

                foreach (string input in userInputs)
                {
                    Logger.Instance.Info(SourceFilePath, $"Chain Demo: --- Обработка ввода: '{input}' ---");
                    ICommand command = startHandler.HandleRequest(input, mockReceiver); // Отправляем запрос первому в цепочке

                    if (command != null)
                    {
                        Logger.Instance.Info(SourceFilePath, $"Chain Demo: Цепочка вернула команду типа '{command.GetType().Name}'. Выполняем...");
                        invoker.SetCommand(command);
                        invoker.ExecuteCommand();
                    }
                    else
                    {
                        Logger.Instance.Warning(SourceFilePath, $"Chain Demo: Команда для ввода '{input}' не была создана/распознана цепочкой.");
                    }
                    Logger.Instance.Info(SourceFilePath, "--------------------------------------");
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Chain Demo: Ошибка при демонстрации Chain of Responsibility: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Chain of Responsibility ---");
            Logger.Instance.Info(SourceFilePath, "-------------------------------------------------------------");
        }
    }
}