using Traktor.Interfaces;
using Traktor.DataModels;

namespace Traktor.ComputerVision
{
    public class CameraVisionSystem : IComputerVisionSystem
    {
        private readonly Random _random = new Random();
        private bool _isSystemActive = true;

        
        public CameraVisionSystem()
        {
            Console.WriteLine("[CameraVisionSystem]: Инициализирована.");
        }

        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            if (!_isSystemActive)
            {
                Console.WriteLine("[CameraVisionSystem]: DetectObstacles - система не активна.");
                return new List<ObstacleData>();
            }

            Console.WriteLine($"[CameraVisionSystem]: Обнаружение препятствий по данным камеры. Трактор в: {currentTractorPosition}");
            List<ObstacleData> detected = new List<ObstacleData>();

            if (_random.Next(0, 4) == 0)
            {
                // Генерируем препятствие "впереди" трактора
                double offsetY = 1.0 + _random.NextDouble() * 4.0;
                double offsetX = (_random.NextDouble() - 0.5) * 2.0;

                Coordinates obstaclePos = new Coordinates(
                    currentTractorPosition.Latitude + offsetY,
                    currentTractorPosition.Longitude + offsetX
                );
                string description = _random.Next(0, 2) == 0 ? "Камень (камера)" : "Низкая ветка (камера)";

                detected.Add(new ObstacleData(obstaclePos, description));
                Console.WriteLine($"[CameraVisionSystem]: Обнаружено: {description} в {obstaclePos}");
            }
            else { Console.WriteLine("[CameraVisionSystem]: Препятствий по данным камеры не обнаружено."); }
            return detected;
        }

        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition) // Убран areaOfInterest
        {
            if (!_isSystemActive)
            {
                Console.WriteLine("[CameraVisionSystem]: AnalyzeFieldFeatures - система не активна.");
                return new List<FieldFeatureData>();
            }

            // Имитация получения данных с камеры и их обработки
            Console.WriteLine($"[CameraVisionSystem]: Анализ сорняков по данным камеры во всей видимой зоне. Трактор в: {currentTractorPosition}.");
            List<FieldFeatureData> features = new List<FieldFeatureData>();

            if (_random.Next(0, 3) == 0)
            {
                int numberOfPatches = _random.Next(1, 2);
                for (int i = 0; i < numberOfPatches; i++)
                {
                    // Генерируем координаты сорняков "рядом" с трактором в его "поле зрения"
                    // "Поле зрения" - условно, в некотором радиусе или впереди
                    double range = 5.0; // Максимальное расстояние "обзора" для сорняков
                    double angle = _random.NextDouble() * 2 * Math.PI; // Случайный угол
                    double distance = _random.NextDouble() * range;    // Случайное расстояние в пределах range

                    // Пересчитываем в смещения (очень упрощенно, без учета реальной ориентации трактора)
                    // Предположим, что Latitude - это "вперед/назад", Longitude - "влево/вправо"
                    double offsetY = Math.Sin(angle) * distance;
                    double offsetX = Math.Cos(angle) * distance;

                    Coordinates weedPos = new Coordinates(
                        currentTractorPosition.Latitude + offsetY,
                        currentTractorPosition.Longitude + offsetX
                    );

                    features.Add(new FieldFeatureData(weedPos, FeatureType.DangerousWeed, "Одиночный экземпляр"));
                    Console.WriteLine($"[CameraVisionSystem]: Обнаружен опасный сорняк в {weedPos}");
                }
            }
            else { Console.WriteLine("[CameraVisionSystem]: Опасных сорняков не обнаружено в видимой зоне."); }
            return features;
        }

        // Методы активации/деактивации можно оставить, если ControlUnit будет ими управлять
        public void ActivateSystem() { _isSystemActive = true; Console.WriteLine("[CameraVisionSystem]: Активирована."); }
        public void DeactivateSystem() { _isSystemActive = false; Console.WriteLine("[CameraVisionSystem]: Деактивирована."); }
    }
}