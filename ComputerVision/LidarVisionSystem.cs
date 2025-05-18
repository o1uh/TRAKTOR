using Traktor.Interfaces;
using Traktor.DataModels;

namespace Traktor.ComputerVision
{
    /// <summary>
    /// ���������� ������� ������������� ������ �� ������ LiDAR.
    /// ���������������� �� ����������� ���������� �����������.
    /// </summary>
    public class LidarVisionSystem : IComputerVisionSystem
    {
        private static readonly Random _random = new Random();
        private bool _isSystemActive = true; // ���������� ���� ����������, ���� �����.

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="LidarVisionSystem"/>.
        /// </summary>
        public LidarVisionSystem()
        {
            // _isSystemActive = true; // �� ��������� ������� ��� ��������
            Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ������� ������������ ������ LiDAR ����������������. �������: {_isSystemActive}");
        }

        /// <inheritdoc/>
        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            if (!_isSystemActive)
            {
                Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: ������� LiDAR �� �������. ����������� ����������� ����������.");
                return new List<ObstacleData>();
            }

            Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: ����������� ����������� �� ������ LiDAR. ������� ������� ��������: {currentTractorPosition}");
            List<ObstacleData> detectedObstacles = new List<ObstacleData>();

            // �������� ����������� ����������� LiDAR'��
            // LiDAR ����� ������������ ����������� �� ������� ���������� � � ������ ������������
            if (_random.Next(0, 4) == 0) // 25% ���� ���������� ���-��
            {
                // ���������� ����������� � ��������� ����������� � �� ��������� ����������
                double distance = 1.0 + _random.NextDouble() * 15.0; // ���������� �� 1 �� 16 ������
                double angle = _random.NextDouble() * 2 * Math.PI;   // ��������� ���� (0 - 2PI)

                // ����� ���������� ������� �������� ��������� � ��������� �������� ��� ������/�������
                // ��� ������� ��� �������� ������������, �� ��� ������ ��������.
                // �������, ��� Latitude ~ Y, Longitude ~ X
                // � ������� �������� �������� ���������� ��� ����� ����������
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
                Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: ����������: \"{description}\" � {obstaclePos}");
            }
            else
            {
                Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DetectObstacles: ����������� �� ������ LiDAR �� ����������.");
            }
            return detectedObstacles;
        }

        /// <inheritdoc/>
        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition)
        {
            // LiDAR ������ �� ������������ ��� ���������� ������� �������������� ������������ ����, ����� ��� �������.
            Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AnalyzeFieldFeatures (��� �������� � {currentTractorPosition}): ����� �� �������������� �������� LiDAR. ��������� ������ ������.");
            return new List<FieldFeatureData>(); // LiDAR �� ����������� ����� �����������
        }

        // ���� ControlUnit ������ ��������� ����������� ����� ����������� ������,
        // ����� �������� ������, �� �������� � ���������:
        /// <summary>
        /// ���������� ������� LiDAR (���������� ����).
        /// </summary>
        public void ActivateInternal()
        {
            _isSystemActive = true;
            Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ������� LiDAR ��������� ������������.");
        }

        /// <summary>
        /// ������������ ������� LiDAR (���������� ����).
        /// </summary>
        public void DeactivateInternal()
        {
            _isSystemActive = false;
            Console.WriteLine($"[ComputerVision/LidarVisionSystem.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ������� LiDAR ��������� ��������������.");
        }
    }
}