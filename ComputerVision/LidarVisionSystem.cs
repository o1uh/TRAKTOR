using Traktor.Interfaces;
using Traktor.DataModels;
using Traktor.Core;   

namespace Traktor.ComputerVision
{
    /// <summary>
    /// ���������� ������� ������������� ������ �� ������ LiDAR.
    /// ���������������� �� ����������� ���������� �����������.
    /// </summary>
    public class LidarVisionSystem : IComputerVisionSystem
    {
        private static readonly Random _random = new Random();
        private bool _isSystemActive = true; // ���������� ���� ����������
        private const string SourceFilePath = "ComputerVision/LidarVisionSystem.cs"; 

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="LidarVisionSystem"/>.
        /// </summary>
        public LidarVisionSystem()
        {
            // _isSystemActive = true; // �� ��������� ������� ��� ��������
            Logger.Instance.Info(SourceFilePath, $"������� ������������ ������ LiDAR ����������������. �������: {_isSystemActive}");
        }

        /// <inheritdoc/>
        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            if (!_isSystemActive)
            {
                Logger.Instance.Warning(SourceFilePath, $"DetectObstacles: ������� LiDAR �� �������. ����������� ����������� ����������.");
                return new List<ObstacleData>();
            }

            Logger.Instance.Debug(SourceFilePath, $"DetectObstacles: ����������� ����������� �� ������ LiDAR. ������� ������� ��������: {currentTractorPosition}");
            List<ObstacleData> detectedObstacles = new List<ObstacleData>();

            // �������� ����������� ����������� LiDAR'��
            // LiDAR ����� ������������ ����������� �� ������� ���������� � � ������ ������������
            if (_random.Next(0, 4) == 0) // 25% ���� ���������� ���-��
            {
                // ���������� ����������� � ��������� ����������� � �� ��������� ����������
                double distance = 1.0 + _random.NextDouble() * 15.0; // ���������� �� 1 �� 16 ������
                double angle = _random.NextDouble() * 2 * Math.PI;   // ��������� ���� (0 - 2PI)

                const double metersToDegreesApproximation = 0.000009; // ����� ������ �����������: 1 ���� ~ 0.000009 �������

                double offsetY = Math.Sin(angle) * distance * metersToDegreesApproximation;
                double offsetX = Math.Cos(angle) * distance * metersToDegreesApproximation;

                Coordinates obstaclePos = new Coordinates(
                    currentTractorPosition.Latitude + offsetY,
                    currentTractorPosition.Longitude + offsetX
                );

                string description;
                int typeRoll = _random.Next(0, 3);
                if (typeRoll == 0) description = "������� ������ (LiDAR)";
                else if (typeRoll == 1) description = "������/����� (LiDAR)";
                else description = "������ �����/����� (LiDAR)";

                detectedObstacles.Add(new ObstacleData(obstaclePos, description));
                Logger.Instance.Info(SourceFilePath, $"DetectObstacles: ����������: \"{description}\" � {obstaclePos}");
            }
            else
            {
                Logger.Instance.Debug(SourceFilePath, "DetectObstacles: ����������� �� ������ LiDAR �� ����������.");
            }
            return detectedObstacles;
        }

        /// <inheritdoc/>
        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition)
        {
            // LiDAR ������ �� ������������ ��� ���������� ������� �������������� ������������ ����, ����� ��� �������.
            Logger.Instance.Info(SourceFilePath, $"AnalyzeFieldFeatures (��� �������� � {currentTractorPosition}): ����� �� �������������� �������� LiDAR. ��������� ������ ������.");
            return new List<FieldFeatureData>();
        }

        /// <summary>
        /// ���������� ������� LiDAR (���������� ����).
        /// </summary>
        public void ActivateInternal()
        {
            _isSystemActive = true;
            Logger.Instance.Info(SourceFilePath, "������� LiDAR ��������� ������������.");
        }

        /// <summary>
        /// ������������ ������� LiDAR (���������� ����).
        /// </summary>
        public void DeactivateInternal()
        {
            _isSystemActive = false;
            Logger.Instance.Info(SourceFilePath, "������� LiDAR ��������� ��������������.");
        }
    }
}