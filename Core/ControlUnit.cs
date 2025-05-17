// Core/ControlUnit.cs
using TractorAutopilot.Interfaces;
using System.Drawing; // Для Bitmap
using TractorAutopilot.Sensors; // Для SoilCondition

namespace TractorAutopilot.Core
{
    public class ControlUnit
    {
        private readonly INavigationSystem _navigationSystem;
        private readonly IComputerVisionSystem _computerVisionSystem;
        private readonly ISensors<double> _distanceSensor;
        private readonly ISensors<Bitmap> _cameraSensor;
        private readonly ISensors<SoilCondition> _soilSensor;
        private readonly IUserInterface _userInterface;
        private readonly Logger _logger = Logger.Instance; // Добавляем логгер

        /// <summary>
        /// Конструктор класса ControlUnit.
        /// </summary>
        /// <param name="navigationSystem">Система навигации.</param>
        /// <param name="computerVisionSystem">Система компьютерного зрения.</param>
        /// <param name="distanceSensor">Датчик расстояния.</param>
        /// <param name="cameraSensor">Камера.</param>
        /// <param name="soilSensor">Датчик состояния почвы.</param>
        /// <param name="userInterface">Пользовательский интерфейс.</param>
        public ControlUnit(INavigationSystem navigationSystem, IComputerVisionSystem computerVisionSystem, ISensors<double> distanceSensor, ISensors<Bitmap> cameraSensor, ISensors<SoilCondition> soilSensor, IUserInterface userInterface)
        {
            _navigationSystem = navigationSystem;
            _computerVisionSystem = computerVisionSystem;
            _distanceSensor = distanceSensor;
            _cameraSensor = cameraSensor;
            _soilSensor = soilSensor;
            _userInterface = userInterface;
        }

        /// <summary>
        /// Запускает процесс управления трактором.
        /// </summary>
        public void Start()
        {
            _logger.LogDebug("ControlUnit.Start() вызван."); // Добавляем логирование

            // Основная логика управления трактором
            _userInterface.DisplayStatus("Система запущена...");
            _logger.LogDebug("Пользовательский интерфейс: Система запущена..."); // Добавляем логирование

            // Пример использования системы навигации
            var location = _navigationSystem.GetPosition();
            _userInterface.DisplayStatus($"Текущее местоположение: {location.Latitude}, {location.Longitude}");
            _logger.LogDebug($"Система навигации: Текущее местоположение: {location.Latitude}, {location.Longitude}"); // Добавляем логирование

            // Пример использования системы компьютерного зрения
            var obstacles = _computerVisionSystem.DetectObstacles();
            if (obstacles.Length > 0)
            {
                _userInterface.ShowAlerts($"Обнаружено {obstacles.Length} препятствий!");
                _logger.LogWarning($"Система компьютерного зрения: Обнаружено {obstacles.Length} препятствий!"); // Добавляем логирование
            }

            // Пример использования датчиков
            var distance = _distanceSensor.GetData();
            _userInterface.DisplayStatus($"Расстояние: {distance} м");
            _logger.LogDebug($"Датчик расстояния: Расстояние: {distance} м"); // Добавляем логирование

            var image = _cameraSensor.GetData();
            _userInterface.DisplayStatus($"Получено изображение с камеры (размеры: {image.Width}x{image.Height})");
            _logger.LogDebug($"Датчик камеры: Получено изображение с камеры (размеры: {image.Width}x{image.Height})"); // Добавляем логирование

            var soilCondition = _soilSensor.GetData();
            _userInterface.DisplayStatus($"Влажность почвы: {soilCondition.Moisture} %, Плотность почвы: {soilCondition.Density} г/см³");
            _logger.LogDebug($"Датчик почвы: Влажность почвы: {soilCondition.Moisture} %, Плотность почвы: {soilCondition.Density} г/см³"); // Добавляем логирование

            _userInterface.DisplayStatus("Система завершила работу.");
            _logger.LogDebug("Пользовательский интерфейс: Система завершила работу."); // Добавляем логирование
            _logger.LogDebug("ControlUnit.Start() завершен."); // Добавляем логирование
        }
    }
}