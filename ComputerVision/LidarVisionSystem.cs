using Traktor.Interfaces;
using Traktor.DataModels;

namespace Traktor.ComputerVision
{
    /// <summary>
    /// Реализация системы компьютерного зрения на основе LiDAR.
    /// Специализируется на обнаружении физических препятствий.
    /// </summary>
    public class LidarVisionSystem : IComputerVisionSystem
    {
        private static readonly Random _random = new Random();
        private bool _isSystemActive = true; // Внутренний флаг активности, если нужен.

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LidarVisionSystem"/>.
        /// </summary>
        public LidarVisionSystem()
        {
            // _isSystemActive = true; // По умолчанию активна при создании
            Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Система технического зрения LiDAR инициализирована. Активна: {_isSystemActive}");
        }

        /// <inheritdoc/>
        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            if (!_isSystemActive)
            {
                Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: Система LiDAR не активна. Обнаружение препятствий невозможно.");
                return new List<ObstacleData>();
            }

            Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: Обнаружение препятствий по данным LiDAR. Текущая позиция трактора: {currentTractorPosition}");
            List<ObstacleData> detectedObstacles = new List<ObstacleData>();

            // Имитация обнаружения препятствий LiDAR'ом
            // LiDAR может обнаруживать препятствия на большем расстоянии и в разных направлениях
            if (_random.Next(0, 4) == 0) // 25% шанс обнаружить что-то
            {
                // Генерируем препятствие в случайном направлении и на случайном расстоянии
                double distance = 1.0 + _random.NextDouble() * 15.0; // Расстояние от 1 до 16 метров
                double angle = _random.NextDouble() * 2 * Math.PI;   // Случайный угол (0 - 2PI)

                // Очень упрощенный перевод полярных координат в декартовы смещения для широты/долготы
                // Это неточно для реальных геокоординат, но для макета подойдет.
                // Считаем, что Latitude ~ Y, Longitude ~ X
                // И масштаб градусов примерно одинаковый для малых расстояний
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
                Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: Обнаружено: \"{description}\" в {obstaclePos}");
            }
            else
            {
                Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: Препятствий по данным LiDAR не обнаружено.");
            }
            return detectedObstacles;
        }

        /// <inheritdoc/>
        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition)
        {
            // LiDAR обычно не используется для детального анализа агрономических особенностей поля, таких как сорняки.
            Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures (для трактора в {currentTractorPosition}): Метод не поддерживается системой LiDAR. Возвращен пустой список.");
            return new List<FieldFeatureData>(); // LiDAR не анализирует такие особенности
        }

        // Если ControlUnit должен управлять активностью этого конкретного модуля,
        // можно добавить методы, не входящие в интерфейс:
        /// <summary>
        /// Активирует систему LiDAR (внутренний флаг).
        /// </summary>
        public void ActivateInternal()
        {
            _isSystemActive = true;
            Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Система LiDAR внутренне активирована.");
        }

        /// <summary>
        /// Деактивирует систему LiDAR (внутренний флаг).
        /// </summary>
        public void DeactivateInternal()
        {
            _isSystemActive = false;
            Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: Система LiDAR внутренне деактивирована.");
        }
    }
}