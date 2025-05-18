using System.Drawing; // Для Bitmap
using Traktor.Core;
using Traktor.Sensors;
using Traktor.Navigation;
using Traktor.ComputerVision;
using Traktor.Implements;
using Traktor.Interfaces;
using Traktor.DataModels; // Для Coordinates, ImplementType
using Traktor.Proxies;

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

            try
            {
                // 1. Инициализация сенсоров и их прокси
                Logger.Instance.Info(SourceFilePath, "Инициализация сенсоров...");

                var distanceSensor = new DistanceSensor();
                ISensors<double> distanceSensorProxy = new SensorProxy<double>(
                    () => distanceSensor, // Передаем уже созданный экземпляр (или можно () => new DistanceSensor())
                    TimeSpan.FromMilliseconds(500) // Кэш на 0.5 секунды
                );

                var soilSensor = new SoilSensor();
                ISensors<SoilSensorData> soilSensorProxy = new SensorProxy<SoilSensorData>(
                    () => soilSensor,
                    TimeSpan.FromSeconds(2) // Кэш на 2 секунды
                );

                var cameraSensor = new CameraSensor();
                ISensors<Bitmap> cameraSensorProxy = new SensorProxy<Bitmap>(
                    () => cameraSensor,
                    TimeSpan.FromMilliseconds(200) // Кэш на 0.2 секунды (кадры могут быть частыми)
                );
                Logger.Instance.Info(SourceFilePath, "Сенсоры и их прокси инициализированы.");

                // 2. Инициализация систем навигации
                Logger.Instance.Info(SourceFilePath, "Инициализация систем навигации...");
                INavigationSystem gpsNavigation = new GPSNavigationSystem();
                INavigationSystem inertialNavigation = new InertialNavigationSystem();
                // В реальном приложении мог бы быть NavigationProxy для переключения
                // Пока ControlUnit будет использовать одну (например, GPS)
                // Для демонстрации работы ИНС, ее нужно "выставить" перед использованием
                // inertialNavigation.UpdateSimulatedPosition(new Coordinates(55.0, 37.0)); // Пример выставки ИНС
                Logger.Instance.Info(SourceFilePath, "Системы навигации инициализированы.");

                // 3. Инициализация систем компьютерного зрения и их прокси
                Logger.Instance.Info(SourceFilePath, "Инициализация систем технического зрения...");
                Func<IComputerVisionSystem> cameraVisionFactory = () => new CameraVisionSystem(cameraSensorProxy); // CameraVisionSystem требует CameraSensor
                Func<IComputerVisionSystem> lidarVisionFactory = () => new LidarVisionSystem();

                IComputerVisionSystem visionProxy = new VisionSystemProxy(
                    cameraVisionFactory,      // Основная система - камера
                    TimeSpan.FromSeconds(1),  // Кэш для результатов зрения
                    lidarVisionFactory        // Резервная система - LiDAR
                );
                Logger.Instance.Info(SourceFilePath, "Системы технического зрения и их прокси инициализированы.");

                // 4. Инициализация системы управления навесным оборудованием
                Logger.Instance.Info(SourceFilePath, "Инициализация системы управления навесным оборудованием...");
                var implementControl = new ImplementControlSystem();
                Logger.Instance.Info(SourceFilePath, "Система управления навесным оборудованием инициализирована.");

                // 5. Инициализация ControlUnit
                Logger.Instance.Info(SourceFilePath, "Инициализация ControlUnit...");
                var controlUnit = new ControlUnit(
                    gpsNavigation,        // Используем GPS как основную систему навигации
                    visionProxy,
                    implementControl,
                    distanceSensorProxy,
                    soilSensorProxy,
                    cameraSensorProxy
                );
                Logger.Instance.Info(SourceFilePath, "ControlUnit инициализирован.");

                // 6. Инициализация и запуск UserInterface
                Logger.Instance.Info(SourceFilePath, "Инициализация UserInterface...");
                var userInterface = new UserInterface(controlUnit);
                Logger.Instance.Info(SourceFilePath, "UserInterface инициализирован. Запуск...");

                userInterface.Run(); // Запускаем основной цикл UI

            }
            catch (Exception ex)
            {
                Logger.Instance.Fatal(SourceFilePath, $"КРИТИЧЕСКАЯ ОШИБКА на старте приложения: {ex.Message}", ex);
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
    }
}