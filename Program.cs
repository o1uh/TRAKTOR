﻿using System.Drawing;
using Traktor.Adapters;
using Traktor.Builders;
using Traktor.Commands;
using Traktor.ComputerVision;
using Traktor.Core;
using Traktor.DataModels;
using Traktor.Decorators;
using Traktor.EquipmentFactories;
using Traktor.Facades;
using Traktor.FieldElements;
using Traktor.FieldElements;
using Traktor.Flyweights;
using Traktor.Implements;
using Traktor.Interfaces; 
using Traktor.MementoPattern;
using Traktor.Mocks;
using Traktor.Mocks;      
using Traktor.Navigation;
using Traktor.Observers;
using Traktor.OperationExecutors;
using Traktor.Operations;
using Traktor.Prototypes;
using Traktor.Proxies;
using Traktor.ReportGenerators;
using Traktor.Sensors;
using Traktor.States;
using Traktor.TaskComponents;
using Traktor.Visitors;
using Traktor.Prototypes;
using Traktor.ObjectPoolPattern;

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
            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Начало демонстрации паттерна Factory Method");
            Logger.Instance.Info(SourceFilePath, "==================================================");

            DemonstrateFactoryMethodPattern();

            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Конец демонстрации паттерна Factory Method");
            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "\n");

            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Начало демонстрации паттерна Abstract Factory");
            Logger.Instance.Info(SourceFilePath, "==================================================");

            DemonstrateAbstractFactoryPattern(); 

            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Конец демонстрации паттерна Abstract Factory");
            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "\n");

            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Начало демонстрации паттерна Prototype");
            Logger.Instance.Info(SourceFilePath, "==================================================");

            DemonstratePrototypePattern();

            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Конец демонстрации паттерна Prototype");
            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "\n");

            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Начало демонстрации паттерна Object Pool");
            Logger.Instance.Info(SourceFilePath, "==================================================");

            DemonstrateObjectPoolPattern();

            Logger.Instance.Info(SourceFilePath, "==================================================");
            Logger.Instance.Info(SourceFilePath, "Конец демонстрации паттерна Object Pool");
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
        private static void DemonstrateVisitorPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Visitor ---");
            try
            {
                // 1. Создаем структуру объектов (элементов поля)
                List<IFieldElement> fieldStructure = new List<IFieldElement>
                {
                    new ObstacleElement("Большой камень", new Coordinates(55.1, 37.1)),
                    new SoilPatchElement("Песчаная", 15.5),
                    new ObstacleElement("Упавшее дерево", new Coordinates(55.12, 37.15)),
                    new SoilPatchElement("Глинистая", 45.2),
                    new ObstacleElement("Канава", new Coordinates(55.13, 37.11))
                };
                Logger.Instance.Info(SourceFilePath, $"Visitor Demo: Создана структура из {fieldStructure.Count} элементов поля.");

                // 2. Создаем Посетителя
                IVisitor reportVisitor = new FieldReportVisitor();
                Logger.Instance.Info(SourceFilePath, $"Visitor Demo: Создан Visitor типа '{reportVisitor.GetType().Name}'.");

                // 3. Заставляем Посетителя "пройтись" по всем элементам структуры
                Logger.Instance.Info(SourceFilePath, "Visitor Demo: Запуск обхода элементов поля Посетителем...");
                foreach (var element in fieldStructure)
                {
                    element.Accept(reportVisitor);
                }
                Logger.Instance.Info(SourceFilePath, "Visitor Demo: Обход элементов Посетителем завершен.");

                // 4. (Опционально) Получаем результат работы посетителя, если он что-то накапливал
                if (reportVisitor is FieldReportVisitor concreteReportVisitor) // Проверка типа для доступа к специфичному методу
                {
                    string report = concreteReportVisitor.GetGeneratedReport();
                    Logger.Instance.Info(SourceFilePath, "Visitor Demo: Полный сгенерированный отчет (из StringBuilder Посетителя):\n" + report);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Visitor Demo: Ошибка при демонстрации Visitor: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Visitor ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }

        private static void DemonstrateStatePattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна State ---");
            try
            {
                // 1. Создаем Контекст (наш MockControlUnit)
                var mockContext = new MockControlUnit(); 

                // 2. Демонстрируем поведение в начальном состоянии ("Остановлен")
                Logger.Instance.Info(SourceFilePath, "State Demo: --- Текущее состояние: Остановлен ---");
                mockContext.RequestSimulationStep(); 
                mockContext.RequestStop();           

                // 3. Запрашиваем старт
                Logger.Instance.Info(SourceFilePath, "State Demo: Запрос на старт...");
                var target = new Coordinates(10, 20);
                var implement = ImplementType.Plough;
                mockContext.RequestStart(target, implement, null);

                // 4. Демонстрируем поведение в состоянии "Работает"
                Logger.Instance.Info(SourceFilePath, "State Demo: --- Текущее состояние: Работает (ожидается) ---");
                mockContext.RequestSimulationStep(); 
                mockContext.RequestSimulationStep();
                mockContext.RequestStart(target, implement, null); 

                // 5. Запрашиваем остановку
                Logger.Instance.Info(SourceFilePath, "State Demo: Запрос на остановку...");
                mockContext.RequestStop();

                // 6. Демонстрируем поведение в состоянии "Остановлен" снова
                Logger.Instance.Info(SourceFilePath, "State Demo: --- Текущее состояние: Остановлен (ожидается) ---");
                mockContext.RequestSimulationStep(); 
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"State Demo: Ошибка при демонстрации State: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна State ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }

        private static void DemonstrateMementoPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Memento ---");
            try
            {
                // 1. Создаем Originator (MockControlUnit) и Caretaker (History)
                var originator = new MockControlUnit();
                var history = new History();

                Logger.Instance.Info(SourceFilePath, $"Memento Demo: Начальное состояние Originator. LastOperation: '{originator.LastOperationPerformed}'.");

                // 2. Выполняем какие-то действия и сохраняем состояние
                Logger.Instance.Info(SourceFilePath, "Memento Demo: Выполняем StartOperation...");
                originator.RequestStart(new Coordinates(1, 1), ImplementType.Plough, null);
                Logger.Instance.Info(SourceFilePath, $"Memento Demo: Состояние Originator после Start. LastOperation: '{originator.LastOperationPerformed}'.");

                Logger.Instance.Info(SourceFilePath, "Memento Demo: Сохраняем текущее состояние в History...");
                history.PushMemento(originator.CreateMemento());

                // 3. Выполняем еще действия, изменяющие состояние
                Logger.Instance.Info(SourceFilePath, "Memento Demo: Выполняем SimulationStep...");
                originator.RequestSimulationStep();
                Logger.Instance.Info(SourceFilePath, $"Memento Demo: Состояние Originator после SimulationStep. LastOperation: '{originator.LastOperationPerformed}'.");

                Logger.Instance.Info(SourceFilePath, "Memento Demo: Выполняем StopOperation...");
                originator.RequestStop();
                Logger.Instance.Info(SourceFilePath, $"Memento Demo: Состояние Originator после Stop. LastOperation: '{originator.LastOperationPerformed}'.");

                // 4. Восстанавливаем предыдущее сохраненное состояние
                Logger.Instance.Info(SourceFilePath, "Memento Demo: Восстанавливаем предыдущее состояние из History...");
                object memento = history.PopMemento();
                if (memento != null)
                {
                    originator.RestoreMemento(memento);
                    Logger.Instance.Info(SourceFilePath, $"Memento Demo: Состояние Originator ПОСЛЕ ВОССТАНОВЛЕНИЯ. LastOperation: '{originator.LastOperationPerformed}'.");
                }
                else
                {
                    Logger.Instance.Warning(SourceFilePath, "Memento Demo: Не удалось получить Memento из History для восстановления.");
                }

                // 5. Попытка восстановить еще раз (стек должен быть пуст)
                Logger.Instance.Info(SourceFilePath, "Memento Demo: Попытка восстановить еще раз (History должна быть пуста)...");
                object ещеОдинMemento = history.PopMemento();
                if (ещеОдинMemento == null)
                {
                    Logger.Instance.Info(SourceFilePath, "Memento Demo: History пуста, как и ожидалось.");
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Memento Demo: Ошибка при демонстрации Memento: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Memento ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }


        private static void DemonstrateObserverPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Observer ---");
            try
            {
                // 1. Создаем Субъекта (MockControlUnit)
                var subjectControlUnit = new MockControlUnit();

                // 2. Создаем Наблюдателей
                var observer1 = new StatusDisplayObserver("Дисплей_Панели_Приборов");
                var observer2 = new StatusDisplayObserver("Удаленный_Монитор_Фермера");

                // 3. Подписываем Наблюдателей на Субъекта
                Logger.Instance.Info(SourceFilePath, "Observer Demo: Подписка наблюдателей на MockControlUnit...");
                subjectControlUnit.Attach(observer1);
                subjectControlUnit.Attach(observer2);

                // 4. Выполняем действия с Субъектом, которые должны вызывать Notify()
                Logger.Instance.Info(SourceFilePath, "Observer Demo: --- Действие 1: Запуск операций ---");
                subjectControlUnit.RequestStart(new Coordinates(77, 88), ImplementType.Sprayer);

                Logger.Instance.Info(SourceFilePath, "Observer Demo: --- Действие 2: Шаг симуляции ---");
                subjectControlUnit.RequestSimulationStep();

                // 5. Отписываем одного Наблюдателя
                Logger.Instance.Info(SourceFilePath, "Observer Demo: Отписка наблюдателя 'Дисплей_Панели_Приборов'...");
                subjectControlUnit.Detach(observer1);

                // 6. Выполняем еще одно действие
                Logger.Instance.Info(SourceFilePath, "Observer Demo: --- Действие 3: Остановка операций (после отписки observer1) ---");
                subjectControlUnit.RequestStop();

                // 7. Попытка отписать несуществующего или уже отписанного (для демонстрации логики Detach)
                Logger.Instance.Info(SourceFilePath, "Observer Demo: Попытка отписать 'Дисплей_Панели_Приборов' еще раз...");
                subjectControlUnit.Detach(observer1);
                var observer3 = new StatusDisplayObserver("Временный_Наблюдатель");
                Logger.Instance.Info(SourceFilePath, "Observer Demo: Попытка отписать 'Временный_Наблюдатель' (который не был подписан)...");
                subjectControlUnit.Detach(observer3);

            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Observer Demo: Ошибка при демонстрации Observer: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Observer ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }

        private static void DemonstrateFacadePattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Facade ---");
            try
            {
                // 1. Создаем "сложную" подсистему (наш MockControlUnit)
                var mockControlUnit = new MockControlUnit();
                Logger.Instance.Info(SourceFilePath, "Facade Demo: MockControlUnit (подсистема) создан.");

                // 2. Создаем Фасад, передавая ему подсистему
                var facade = new TractorAutopilotFacade(mockControlUnit);
                Logger.Instance.Info(SourceFilePath, "Facade Demo: TractorAutopilotFacade создан.");

                // 3. Используем упрощенные методы Фасада
                Logger.Instance.Info(SourceFilePath, "Facade Demo: --- Вызов StartFullFieldOperation через Фасад ---");
                facade.StartFullFieldOperation("Поле_Номер_1", 55.75, 37.61, ImplementType.Seeder);

                Logger.Instance.Info(SourceFilePath, "Facade Demo: --- Вызов GetQuickStatus через Фасад ---");
                string status = facade.GetQuickStatus();
                Logger.Instance.Info(SourceFilePath, $"Facade Demo: Быстрый статус от Фасада: '{status}'");


                Logger.Instance.Info(SourceFilePath, "Facade Demo: --- Вызов StopAllOperations через Фасад ---");
                facade.StopAllOperations();
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Facade Demo: Ошибка при демонстрации Facade: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Facade ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }

        private static void DemonstrateBridgePattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Bridge ---");
            try
            {
                // 1. Создаем конкретных Исполнителей (Implementors)
                IOperationExecutor detailedExecutor = new DetailedSimulationExecutor();
                IOperationExecutor fastExecutor = new FastSimulationExecutor();
                Logger.Instance.Info(SourceFilePath, "Bridge Demo: Созданы исполнители (Detailed и Fast).");

                // 2. Демонстрация: Подготовка почвы с детальной симуляцией
                Logger.Instance.Info(SourceFilePath, "\nBridge Demo: === Подготовка почвы (Детальная симуляция) ===");
                TractorOperationBase soilPrepDetailed = new SoilPreparationOperation(detailedExecutor);
                soilPrepDetailed.ExecuteOperation();

                // 3. Демонстрация: Подготовка почвы с быстрой симуляцией
                Logger.Instance.Info(SourceFilePath, "\nBridge Demo: === Подготовка почвы (Быстрая симуляция) ===");
                TractorOperationBase soilPrepFast = new SoilPreparationOperation(fastExecutor);
                soilPrepFast.ExecuteOperation();

                // 4. Демонстрация: Борьба с вредителями с детальной симуляцией
                Logger.Instance.Info(SourceFilePath, "\nBridge Demo: === Борьба с вредителями (Детальная симуляция) ===");
                TractorOperationBase pestControlDetailed = new PestControlOperation(detailedExecutor);
                pestControlDetailed.ExecuteOperation();

                // 5. Демонстрация: Борьба с вредителями с быстрой симуляцией
                Logger.Instance.Info(SourceFilePath, "\nBridge Demo: === Борьба с вредителями (Быстрая симуляция) ===");
                TractorOperationBase pestControlFast = new PestControlOperation(fastExecutor);
                pestControlFast.ExecuteOperation();

            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Bridge Demo: Ошибка при демонстрации Bridge: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Bridge ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }

        private static void DemonstrateFlyweightPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Flyweight ---");
            try
            {
                var factory = new FieldObjectTypeFactory();
                var fieldObjectsData = new[]
                {
                    new { TN = "Камень", Tex = "rock_texture.png", Col = ConsoleColor.Gray, ID = "R001", Lat = 55.1, Lon = 37.1 },
                    new { TN = "Пшеница", Tex = "wheat_texture.png", Col = ConsoleColor.Yellow, ID = "W001", Lat = 55.2, Lon = 37.2 },
                    new { TN = "Камень", Tex = "rock_texture.png", Col = ConsoleColor.Gray, ID = "R002", Lat = 55.3, Lon = 37.3 }, // Тот же тип "Камень"
                    new { TN = "Сорняк", Tex = "weed_texture.png", Col = ConsoleColor.Green, ID = "D001", Lat = 55.4, Lon = 37.4 },
                    new { TN = "Пшеница", Tex = "wheat_texture.png", Col = ConsoleColor.Yellow, ID = "W002", Lat = 55.5, Lon = 37.5 }  // Тот же тип "Пшеница"
                };

                List<IFieldObjectType> flyweightsUsed = new List<IFieldObjectType>();

                Logger.Instance.Info(SourceFilePath, "Flyweight Demo: Создание/получение и 'отображение' объектов на поле...");
                foreach (var data in fieldObjectsData)
                {
                    IFieldObjectType fieldObject = factory.GetFlyweight(data.TN, data.Tex, data.Col);
                    flyweightsUsed.Add(fieldObject);

                    Coordinates currentPosition = new Coordinates(data.Lat, data.Lon);
                    Logger.Instance.Info(SourceFilePath, $"Flyweight Demo (Клиент): Запрос на отображение объекта ID='{data.ID}' с типом '{data.TN}' в {currentPosition}");
                    fieldObject.Display(currentPosition, data.ID);
                    Logger.Instance.Debug(SourceFilePath, $"  Внутреннее состояние приспособленца: {fieldObject.GetIntrinsicStateDescription()}");
                }

                Logger.Instance.Info(SourceFilePath, $"Flyweight Demo: Всего создано/запрошено объектов: {fieldObjectsData.Length}");
                Logger.Instance.Info(SourceFilePath, $"Flyweight Demo: Уникальных объектов-приспособленцев в фабрике: {factory.GetFlyweightsCount()}");

                if (flyweightsUsed.Count >= 5) // Убедимся, что у нас достаточно элементов для проверки
                {
                    bool stonesAreSameInstance = Object.ReferenceEquals(flyweightsUsed[0], flyweightsUsed[2]);
                    bool wheatsAreSameInstance = Object.ReferenceEquals(flyweightsUsed[1], flyweightsUsed[4]);
                    Logger.Instance.Info(SourceFilePath, $"Flyweight Demo: Экземпляр 'Камень' для R001 и R002 один и тот же: {stonesAreSameInstance}");
                    Logger.Instance.Info(SourceFilePath, $"Flyweight Demo: Экземпляр 'Пшеница' для W001 и W002 один и тот же: {wheatsAreSameInstance}");
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Flyweight Demo: Ошибка при демонстрации Flyweight: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Flyweight ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }

        private static void DemonstrateFactoryMethodPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Factory Method ---");
            try
            {
                ReportGenerator[] generators = new ReportGenerator[]
                {
                    new SimpleTextReportGenerator(),
                    new DetailedXmlReportGenerator()
                };

                foreach (var generator in generators)
                {
                    Logger.Instance.Info(SourceFilePath, $"\nFactoryMethod Demo: Используется генератор типа '{generator.GetType().Name}'.");
                    generator.GenerateAndDisplayReport();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"FactoryMethod Demo: Ошибка при демонстрации Factory Method: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Factory Method ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }

        private static void ClientCodeForAbstractFactory(IFieldEquipmentFactory factory)
        {
            string factoryType = factory.GetType().Name;
            Logger.Instance.Info(SourceFilePath, $"ClientCode: Работа с фабрикой типа '{factoryType}'.");

            ISoilAnalyzerProduct soilAnalyzer = factory.CreateSoilAnalyzer();
            ITillageToolProduct tillageTool = factory.CreateTillageTool();

            Logger.Instance.Info(SourceFilePath, $"ClientCode (фабрика '{factoryType}'): Созданы продукты - Анализатор: '{soilAnalyzer.GetAnalyzerType()}', Инструмент: '{tillageTool.GetToolType()}'.");

            Logger.Instance.Info(SourceFilePath, $"ClientCode (фабрика '{factoryType}'): Использование анализатора...");
            soilAnalyzer.AnalyzeSoil();

            Logger.Instance.Info(SourceFilePath, $"ClientCode (фабрика '{factoryType}'): Использование инструмента...");
            tillageTool.PrepareSoil();
        }

        private static void DemonstrateAbstractFactoryPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Abstract Factory ---");
            try
            {
                // 1. Используем фабрику для стандартных полей
                Logger.Instance.Info(SourceFilePath, "\nAbstractFactory Demo: === Использование StandardFieldEquipmentFactory ===");
                IFieldEquipmentFactory standardFactory = new StandardFieldEquipmentFactory();
                ClientCodeForAbstractFactory(standardFactory);

                // 2. Используем фабрику для каменистых полей
                Logger.Instance.Info(SourceFilePath, "\nAbstractFactory Demo: === Использование RockyFieldEquipmentFactory ===");
                IFieldEquipmentFactory rockyFactory = new RockyFieldEquipmentFactory();
                ClientCodeForAbstractFactory(rockyFactory);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"AbstractFactory Demo: Ошибка при демонстрации Abstract Factory: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Abstract Factory ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }

        private static void DemonstratePrototypePattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Prototype ---");
            try
            {
                // 1. Создаем исходный объект-прототип
                var originalOrder = new FieldPloughingOrder(
                    orderId: "ORD-001",
                    fieldName: "Большое Западное Поле",
                    scheduledDate: new DateTime(2023, 11, 15),
                    ploughingDepth: 0.25
                );
                Logger.Instance.Info(SourceFilePath, "Prototype Demo: Создан оригинальный заказ (прототип):");
                originalOrder.DisplayOrderDetails(SourceFilePath);

                // 2. Клонируем прототип
                Logger.Instance.Info(SourceFilePath, "Prototype Demo: Клонирование оригинального заказа...");
                FieldPloughingOrder clonedOrder = (FieldPloughingOrder)originalOrder.Clone();

                Logger.Instance.Info(SourceFilePath, "Prototype Demo: Создан клон заказа:");
                clonedOrder.DisplayOrderDetails(SourceFilePath);

                // 3. Модифицируем клон, чтобы показать их независимость (если ID не меняется при клонировании)
                Logger.Instance.Info(SourceFilePath, "Prototype Demo: Модификация клонированного заказа...");
                clonedOrder.OrderId = "ORD-002-CLONE";
                clonedOrder.FieldName = "Малое Восточное Поле (клон)";
                clonedOrder.ScheduledDate = new DateTime(2023, 11, 20);
                clonedOrder.PloughingDepth = 0.30;

                Logger.Instance.Info(SourceFilePath, "Prototype Demo: Модифицированный клон:");
                clonedOrder.DisplayOrderDetails(SourceFilePath);

                Logger.Instance.Info(SourceFilePath, "Prototype Demo: Оригинальный заказ (должен остаться неизменным):");
                originalOrder.DisplayOrderDetails(SourceFilePath);

                // 4. Проверка ссылок (для демонстрации, что это разные объекты, если MemberwiseClone сработал)
                bool areSameReference = Object.ReferenceEquals(originalOrder, clonedOrder);
                Logger.Instance.Info(SourceFilePath, $"Prototype Demo: Оригинал и клон ссылаются на один и тот же объект в памяти: {areSameReference}");

            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"Prototype Demo: Ошибка при демонстрации Prototype: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Prototype ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }

        private static void DemonstrateObjectPoolPattern()
        {
            Logger.Instance.Info(SourceFilePath, "--- Начало демонстрации паттерна Object Pool ---");
            try
            {
                // 1. Создаем пул с начальным размером 2 и максимальным 3
                var processorPool = new SensorDataProcessorPool(initialSize: 2, maxSize: 3);
                Logger.Instance.Info(SourceFilePath, $"ObjectPool Demo: Пул создан. Доступно: {processorPool.GetAvailableCount()}, Используется: {processorPool.GetInUseCount()}.");

                // 2. Запрашиваем и используем несколько процессоров
                Logger.Instance.Info(SourceFilePath, "\nObjectPool Demo: --- Запрос первого процессора ---");
                SensorDataProcessor p1 = processorPool.AcquireProcessor();
                if (p1 != null)
                {
                    p1.Process("Данные от GPS");
                    Logger.Instance.Info(SourceFilePath, $"ObjectPool Demo: Статус пула. Доступно: {processorPool.GetAvailableCount()}, Используется: {processorPool.GetInUseCount()}.");
                }

                Logger.Instance.Info(SourceFilePath, "\nObjectPool Demo: --- Запрос второго процессора ---");
                SensorDataProcessor p2 = processorPool.AcquireProcessor();
                if (p2 != null)
                {
                    p2.Process("Данные от LiDAR");
                    Logger.Instance.Info(SourceFilePath, $"ObjectPool Demo: Статус пула. Доступно: {processorPool.GetAvailableCount()}, Используется: {processorPool.GetInUseCount()}.");
                }

                // 3. Возвращаем один процессор в пул
                Logger.Instance.Info(SourceFilePath, "\nObjectPool Demo: --- Возврат первого процессора (p1) ---");
                if (p1 != null)
                {
                    processorPool.ReleaseProcessor(p1);
                    Logger.Instance.Info(SourceFilePath, $"ObjectPool Demo: Статус пула. Доступно: {processorPool.GetAvailableCount()}, Используется: {processorPool.GetInUseCount()}.");
                }

                // 4. Запрашиваем третий процессор (должен быть p1, если он был возвращен, или новый, если пул создает до maxSize)
                Logger.Instance.Info(SourceFilePath, "\nObjectPool Demo: --- Запрос третьего процессора ---");
                SensorDataProcessor p3 = processorPool.AcquireProcessor();
                if (p3 != null)
                {
                    p3.Process("Данные от камеры");
                    Logger.Instance.Info(SourceFilePath, $"ObjectPool Demo: Статус пула. Доступно: {processorPool.GetAvailableCount()}, Используется: {processorPool.GetInUseCount()}.");
                    if (p1 != null && Object.ReferenceEquals(p1, p3))
                    {
                        Logger.Instance.Info(SourceFilePath, $"ObjectPool Demo: Процессор p3 (ID: {p3.Id}) - это переиспользованный p1 (ID: {p1.Id}).");
                    }
                }

                // 5. Запрашиваем четвертый процессор (пул создаст новый, т.к. maxSize=3, а занято 2: p2, p3)
                Logger.Instance.Info(SourceFilePath, "\nObjectPool Demo: --- Запрос четвертого процессора ---");
                SensorDataProcessor p4 = processorPool.AcquireProcessor();
                if (p4 != null)
                {
                    p4.Process("Данные от датчика почвы");
                    Logger.Instance.Info(SourceFilePath, $"ObjectPool Demo: Статус пула. Доступно: {processorPool.GetAvailableCount()}, Используется: {processorPool.GetInUseCount()}. (p2,p3,p4 заняты)");
                }

                // 6. Запрашиваем пятый процессор (пул должен вернуть null, т.к. maxSize=3 и все заняты)
                Logger.Instance.Info(SourceFilePath, "\nObjectPool Demo: --- Запрос пятого процессора (ожидаем null) ---");
                SensorDataProcessor p5 = processorPool.AcquireProcessor();
                if (p5 == null)
                {
                    Logger.Instance.Info(SourceFilePath, "ObjectPool Demo: Пятый процессор не может быть выдан, пул достиг максимального размера и все объекты заняты (как и ожидалось).");
                }
                Logger.Instance.Info(SourceFilePath, $"ObjectPool Demo: Статус пула. Доступно: {processorPool.GetAvailableCount()}, Используется: {processorPool.GetInUseCount()}.");


                // 7. Возвращаем все используемые процессоры
                Logger.Instance.Info(SourceFilePath, "\nObjectPool Demo: --- Возврат всех используемых процессоров ---");
                if (p2 != null) processorPool.ReleaseProcessor(p2);
                if (p3 != null) processorPool.ReleaseProcessor(p3);
                if (p4 != null) processorPool.ReleaseProcessor(p4);
                Logger.Instance.Info(SourceFilePath, $"ObjectPool Demo: Статус пула после возврата. Доступно: {processorPool.GetAvailableCount()}, Используется: {processorPool.GetInUseCount()}.");

            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"ObjectPool Demo: Ошибка при демонстрации Object Pool: {ex.Message}", ex);
            }
            Logger.Instance.Info(SourceFilePath, "--- Конец демонстрации паттерна Object Pool ---");
            Logger.Instance.Info(SourceFilePath, "--------------------------------------------------");
        }
    }
}