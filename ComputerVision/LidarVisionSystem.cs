using System.Collections.Generic;
using Traktor.Interfaces;
using Traktor.DataModels;
using System;

namespace Traktor.ComputerVision
{
    public class LidarVisionSystem : IComputerVisionSystem
    {
        private readonly Random _random = new Random();
        private bool _isSystemActive = true;

        public LidarVisionSystem()
        {
            Console.WriteLine("[LidarVisionSystem]: Инициализирована.");
        }

        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            if (!_isSystemActive)
            {
                Console.WriteLine("[LidarVisionSystem]: DetectObstacles - система не активна.");
                return new List<ObstacleData>();
            }

            // Имитация получения данных с LiDAR и их обработки
            Console.WriteLine($"[LidarVisionSystem]: Обнаружение препятствий по данным LiDAR. Трактор в: {currentTractorPosition}");
            List<ObstacleData> detected = new List<ObstacleData>();

            if (_random.Next(0, 3) == 0)
            {
                double offsetX = (_random.NextDouble() - 0.5) * 5.0;
                double offsetY = 0.5 + _random.NextDouble() * 6.0; // LiDAR может "видеть" дальше или другие типы препятствий
                Coordinates obstaclePos = new Coordinates(
                    currentTractorPosition.Latitude + offsetY,
                    currentTractorPosition.Longitude + offsetX
                );
                string description = _random.Next(0, 3) == 0 ? "Большой камень (LiDAR)" : (_random.Next(0, 2) == 0 ? "Канава (LiDAR)" : "Резкий уклон (LiDAR)");

                detected.Add(new ObstacleData(obstaclePos, description));
                Console.WriteLine($"[LidarVisionSystem]: Обнаружено: {description} в {obstaclePos}");
            }
            else { Console.WriteLine("[LidarVisionSystem]: Препятствий по данным LiDAR не обнаружено."); }
            return detected;
        }

        // LidarVisionSystem не специализируется на анализе сорняков
        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition) // Убран areaOfInterest
        {
            Console.WriteLine($"[LidarVisionSystem]: AnalyzeFieldFeatures (для трактора в {currentTractorPosition}) не поддерживается этой системой (LiDAR). Возвращен пустой список.");
            
            return new List<FieldFeatureData>();
        }

        public void ActivateSystem() { _isSystemActive = true; Console.WriteLine("[LidarVisionSystem]: Активирована."); }
        public void DeactivateSystem() { _isSystemActive = false; Console.WriteLine("[LidarVisionSystem]: Деактивирована."); }
    }
}