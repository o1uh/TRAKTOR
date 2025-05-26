using Traktor.Interfaces;
using Traktor.DataModels;
using Traktor.Core;   

namespace Traktor.ComputerVision
{
    /// <summary>
    /// Реализация системы компьютерного зрения на основе LiDAR.
    /// Специализируется на обнаружении физических препятствий.
    /// </summary>
    public class LidarVisionSystem : IComputerVisionSystem
    {
        private static readonly Random _random = new Random();
        private bool _isSystemActive = true; // Внутренний флаг активности
        private const string SourceFilePath = "ComputerVision/LidarVisionSystem.cs"; 

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LidarVisionSystem"/>.
        /// </summary>
        public LidarVisionSystem()
        {
            // _isSystemActive = true; // По умолчанию активна при создании
            Logger.Instance.Info(SourceFilePath, $"Система технического зрения LiDAR инициализирована. Активна: {_isSystemActive}");
        }

        /// <inheritdoc/>
        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            if (!_isSystemActive)
            {
                Logger.Instance.Warning(SourceFilePath, $"DetectObstacles: Система LiDAR не активна. Обнаружение препятствий невозможно.");
                return new List<ObstacleData>();
            }

            Logger.Instance.Debug(SourceFilePath, $"DetectObstacles: Обнаружение препятствий по данным LiDAR. Текущая позиция трактора: {currentTractorPosition}");
            List<ObstacleData> detectedObstacles = new List<ObstacleData>();

            // Имитация обнаружения препятствий LiDAR'ом
            // LiDAR может обнаруживать препятствия на большем расстоянии и в разных направлениях
            if (_random.Next(0, 4) == 0) // 25% шанс обнаружить что-то
            {
                // Генерируем препятствие в случайном направлении и на случайном расстоянии
                double distance = 1.0 + _random.NextDouble() * 15.0; // Расстояние от 1 до 16 метров
                double angle = _random.NextDouble() * 2 * Math.PI;   // Случайный угол (0 - 2PI)

                const double metersToDegreesApproximation = 0.000009; // Очень грубое приближение: 1 метр ~ 0.000009 градуса

                double offsetY = Math.Sin(angle) * distance * metersToDegreesApproximation;
                double offsetX = Math.Cos(angle) * distance * metersToDegreesApproximation;

                Coordinates obstaclePos = new Coordinates(
                    currentTractorPosition.Latitude + offsetY,
                    currentTractorPosition.Longitude + offsetX
                );

                string description;
                int typeRoll = _random.Next(0, 3);
                if (typeRoll == 0) description = "Большой камень (LiDAR)";
                else if (typeRoll == 1) description = "Канава/овраг (LiDAR)";
                else description = "Резкий уклон/обрыв (LiDAR)";

                detectedObstacles.Add(new ObstacleData(obstaclePos, description));
                Logger.Instance.Info(SourceFilePath, $"DetectObstacles: Обнаружено: \"{description}\" в {obstaclePos}");
            }
            else
            {
                Logger.Instance.Debug(SourceFilePath, "DetectObstacles: Препятствий по данным LiDAR не обнаружено.");
            }
            return detectedObstacles;
        }

        /// <inheritdoc/>
        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition)
        {
            // LiDAR обычно не используется для детального анализа агрономических особенностей поля, таких как сорняки.
            Logger.Instance.Info(SourceFilePath, $"AnalyzeFieldFeatures (для трактора в {currentTractorPosition}): Метод не поддерживается системой LiDAR. Возвращен пустой список.");
            return new List<FieldFeatureData>();
        }

        /// <summary>
        /// Активирует систему LiDAR (внутренний флаг).
        /// </summary>
        public void ActivateInternal()
        {
            _isSystemActive = true;
            Logger.Instance.Info(SourceFilePath, "Система LiDAR внутренне активирована.");
        }

        /// <summary>
        /// Деактивирует систему LiDAR (внутренний флаг).
        /// </summary>
        public void DeactivateInternal()
        {
            _isSystemActive = false;
            Logger.Instance.Info(SourceFilePath, "Система LiDAR внутренне деактивирована.");
        }
    }
}