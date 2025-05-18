using Traktor.Interfaces;
using Traktor.DataModels;
using System.Drawing; // Для Bitmap
using Traktor.Core;   // Добавлено для Logger

namespace Traktor.ComputerVision
{
    /// <summary>
    /// Реализация системы компьютерного зрения на основе анализа изображений с камер.
    /// Получает данные от сенсора камеры для анализа.
    /// </summary>
    public class CameraVisionSystem : IComputerVisionSystem
    {
        private static readonly Random _random = new Random();
        private bool _isSystemActive = true;
        private readonly ISensors<Bitmap> _cameraSensor; // Зависимость от сенсора камеры
        private const string SourceFilePath = "ComputerVision/CameraVisionSystem.cs"; // Определяем константу

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CameraVisionSystem"/>.
        /// </summary>
        /// <param name="cameraSensor">Сенсор камеры, предоставляющий изображения для анализа.</param>
        /// <exception cref="ArgumentNullException">Если cameraSensor равен null.</exception>
        public CameraVisionSystem(ISensors<Bitmap> cameraSensor)
        {
            _cameraSensor = cameraSensor ?? throw new ArgumentNullException(nameof(cameraSensor));
            // _isSystemActive = true; // По умолчанию активна
            Logger.Instance.Info(SourceFilePath, $"Система технического зрения на базе камер инициализирована и связана с сенсором камеры. Активна: {_isSystemActive}");
        }

        /// <inheritdoc/>
        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            if (!_isSystemActive)
            {
                Logger.Instance.Warning(SourceFilePath, $"DetectObstacles: Система камер не активна. Обнаружение препятствий невозможно.");
                return new List<ObstacleData>();
            }

            Logger.Instance.Debug(SourceFilePath, $"DetectObstacles: Запрос данных с камеры для обнаружения препятствий. Текущая позиция трактора: {currentTractorPosition}");
            Bitmap currentFrame = null;
            try
            {
                currentFrame = _cameraSensor.GetData();
                if (currentFrame == null)
                {
                    Logger.Instance.Warning(SourceFilePath, "DetectObstacles: Не удалось получить кадр с камеры (null).");
                    return new List<ObstacleData>();
                }
                Logger.Instance.Debug(SourceFilePath, $"DetectObstacles: Получен кадр с камеры {currentFrame.Width}x{currentFrame.Height}. Имитация анализа...");
                // Здесь был бы сложный анализ currentFrame...
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"DetectObstacles: Ошибка при получении или обработке кадра с камеры: {ex.Message}", ex);
                return new List<ObstacleData>();
            }
            finally
            {
                currentFrame?.Dispose(); // Важно освобождать ресурсы Bitmap
            }

            List<ObstacleData> detectedObstacles = new List<ObstacleData>();
            // Имитация обнаружения препятствий на основе "анализа" кадра
            // Можно использовать currentFrame.Height/Width или даже попытаться извлечь imageCounter для влияния на рандом,
            // но для простоты оставим как было, но сам факт получения кадра важен.
            if (_random.Next(0, 5) == 0)
            {
                const double metersToDegreesApproximation = 0.000009;
                double distanceForward = (1.0 + _random.NextDouble() * 5.0) * metersToDegreesApproximation;
                double sideOffset = (_random.NextDouble() - 0.5) * 2.0 * metersToDegreesApproximation;

                Coordinates obstaclePos = new Coordinates(
                    currentTractorPosition.Latitude + distanceForward,
                    currentTractorPosition.Longitude + sideOffset
                );

                string description = _random.Next(0, 2) == 0 ? "Небольшой камень (камера)" : "Низко висящая ветка (камера)";

                detectedObstacles.Add(new ObstacleData(obstaclePos, description));
                Logger.Instance.Info(SourceFilePath, $"DetectObstacles: По результатам анализа кадра обнаружено: \"{description}\" в {obstaclePos}");
            }
            else
            {
                Logger.Instance.Debug(SourceFilePath, "DetectObstacles: Препятствий по данным анализа кадра не обнаружено.");
            }
            return detectedObstacles;
        }

        /// <inheritdoc/>
        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition)
        {
            if (!_isSystemActive)
            {
                Logger.Instance.Warning(SourceFilePath, $"AnalyzeFieldFeatures: Система камер не активна. Анализ особенностей поля невозможен.");
                return new List<FieldFeatureData>();
            }

            Logger.Instance.Debug(SourceFilePath, $"AnalyzeFieldFeatures: Запрос данных с камеры для анализа особенностей поля. Текущая позиция трактора: {currentTractorPosition}.");
            Bitmap currentFrame = null;
            try
            {
                currentFrame = _cameraSensor.GetData();
                if (currentFrame == null)
                {
                    Logger.Instance.Warning(SourceFilePath, "AnalyzeFieldFeatures: Не удалось получить кадр с камеры (null).");
                    return new List<FieldFeatureData>();
                }
                Logger.Instance.Debug(SourceFilePath, $"AnalyzeFieldFeatures: Получен кадр с камеры {currentFrame.Width}x{currentFrame.Height}. Имитация анализа...");
                // Анализ currentFrame на сорняки и т.д.
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"AnalyzeFieldFeatures: Ошибка при получении или обработке кадра с камеры: {ex.Message}", ex);
                return new List<FieldFeatureData>();
            }
            finally
            {
                currentFrame?.Dispose();
            }

            List<FieldFeatureData> detectedFeatures = new List<FieldFeatureData>();
            if (_random.Next(0, 3) == 0)
            {
                int numberOfFeatures = _random.Next(1, 4);
                for (int i = 0; i < numberOfFeatures; i++)
                {
                    const double metersToDegreesApproximation = 0.000009;
                    double range = (1.0 + _random.NextDouble() * 4.0) * metersToDegreesApproximation;
                    double angle = _random.NextDouble() * Math.PI - Math.PI / 2;

                    double offsetY = Math.Sin(angle) * range;
                    double offsetX = Math.Cos(angle) * range;

                    Coordinates featurePos = new Coordinates(
                        currentTractorPosition.Latitude + offsetX,
                        currentTractorPosition.Longitude + offsetY
                    );

                    FeatureType type = FeatureType.Unknown;
                    string details = "";
                    int typeRoll = _random.Next(0, 3);
                    if (typeRoll == 0) { type = FeatureType.DangerousWeed; details = "Очаг пырея (камера)"; }
                    else if (typeRoll == 1) { type = FeatureType.PestInfestation; details = "Личинки (камера)"; }
                    else { type = FeatureType.WaterLogging; details = "Небольшая лужа (камера)"; }

                    detectedFeatures.Add(new FieldFeatureData(featurePos, type, details));
                    Logger.Instance.Info(SourceFilePath, $"AnalyzeFieldFeatures: По результатам анализа кадра обнаружена особенность: {type} \"{details}\" в {featurePos}");
                }
            }
            else
            {
                Logger.Instance.Debug(SourceFilePath, "AnalyzeFieldFeatures: Агрономических особенностей по данным анализа кадра не обнаружено.");
            }
            return detectedFeatures;
        }

        /// <summary>
        /// Активирует систему камер (внутренний флаг).
        /// </summary>
        public void ActivateInternal()
        {
            _isSystemActive = true;
            Logger.Instance.Info(SourceFilePath, "Система камер внутренне активирована.");
        }

        /// <summary>
        /// Деактивирует систему камер (внутренний флаг).
        /// </summary>
        public void DeactivateInternal()
        {
            _isSystemActive = false;
            Logger.Instance.Info(SourceFilePath, "Система камер внутренне деактивирована.");
        }
    }
}