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
            Console.WriteLine("[LidarVisionSystem]: ����������������.");
        }

        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            if (!_isSystemActive)
            {
                Console.WriteLine("[LidarVisionSystem]: DetectObstacles - ������� �� �������.");
                return new List<ObstacleData>();
            }

            // �������� ��������� ������ � LiDAR � �� ���������
            Console.WriteLine($"[LidarVisionSystem]: ����������� ����������� �� ������ LiDAR. ������� �: {currentTractorPosition}");
            List<ObstacleData> detected = new List<ObstacleData>();

            if (_random.Next(0, 3) == 0)
            {
                double offsetX = (_random.NextDouble() - 0.5) * 5.0;
                double offsetY = 0.5 + _random.NextDouble() * 6.0; // LiDAR ����� "������" ������ ��� ������ ���� �����������
                Coordinates obstaclePos = new Coordinates(
                    currentTractorPosition.Latitude + offsetY,
                    currentTractorPosition.Longitude + offsetX
                );
                string description = _random.Next(0, 3) == 0 ? "������� ������ (LiDAR)" : (_random.Next(0, 2) == 0 ? "������ (LiDAR)" : "������ ����� (LiDAR)");

                detected.Add(new ObstacleData(obstaclePos, description));
                Console.WriteLine($"[LidarVisionSystem]: ����������: {description} � {obstaclePos}");
            }
            else { Console.WriteLine("[LidarVisionSystem]: ����������� �� ������ LiDAR �� ����������."); }
            return detected;
        }

        // LidarVisionSystem �� ���������������� �� ������� ��������
        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition) // ����� areaOfInterest
        {
            Console.WriteLine($"[LidarVisionSystem]: AnalyzeFieldFeatures (��� �������� � {currentTractorPosition}) �� �������������� ���� �������� (LiDAR). ��������� ������ ������.");
            
            return new List<FieldFeatureData>();
        }

        public void ActivateSystem() { _isSystemActive = true; Console.WriteLine("[LidarVisionSystem]: ������������."); }
        public void DeactivateSystem() { _isSystemActive = false; Console.WriteLine("[LidarVisionSystem]: ��������������."); }
    }
}