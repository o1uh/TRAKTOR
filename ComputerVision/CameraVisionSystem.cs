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
            Console.WriteLine("[CameraVisionSystem]: ����������������.");
        }

        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            if (!_isSystemActive)
            {
                Console.WriteLine("[CameraVisionSystem]: DetectObstacles - ������� �� �������.");
                return new List<ObstacleData>();
            }

            Console.WriteLine($"[CameraVisionSystem]: ����������� ����������� �� ������ ������. ������� �: {currentTractorPosition}");
            List<ObstacleData> detected = new List<ObstacleData>();

            if (_random.Next(0, 4) == 0)
            {
                // ���������� ����������� "�������" ��������
                double offsetY = 1.0 + _random.NextDouble() * 4.0;
                double offsetX = (_random.NextDouble() - 0.5) * 2.0;

                Coordinates obstaclePos = new Coordinates(
                    currentTractorPosition.Latitude + offsetY,
                    currentTractorPosition.Longitude + offsetX
                );
                string description = _random.Next(0, 2) == 0 ? "������ (������)" : "������ ����� (������)";

                detected.Add(new ObstacleData(obstaclePos, description));
                Console.WriteLine($"[CameraVisionSystem]: ����������: {description} � {obstaclePos}");
            }
            else { Console.WriteLine("[CameraVisionSystem]: ����������� �� ������ ������ �� ����������."); }
            return detected;
        }

        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition) // ����� areaOfInterest
        {
            if (!_isSystemActive)
            {
                Console.WriteLine("[CameraVisionSystem]: AnalyzeFieldFeatures - ������� �� �������.");
                return new List<FieldFeatureData>();
            }

            // �������� ��������� ������ � ������ � �� ���������
            Console.WriteLine($"[CameraVisionSystem]: ������ �������� �� ������ ������ �� ���� ������� ����. ������� �: {currentTractorPosition}.");
            List<FieldFeatureData> features = new List<FieldFeatureData>();

            if (_random.Next(0, 3) == 0)
            {
                int numberOfPatches = _random.Next(1, 2);
                for (int i = 0; i < numberOfPatches; i++)
                {
                    // ���������� ���������� �������� "�����" � ��������� � ��� "���� ������"
                    // "���� ������" - �������, � ��������� ������� ��� �������
                    double range = 5.0; // ������������ ���������� "������" ��� ��������
                    double angle = _random.NextDouble() * 2 * Math.PI; // ��������� ����
                    double distance = _random.NextDouble() * range;    // ��������� ���������� � �������� range

                    // ������������� � �������� (����� ���������, ��� ����� �������� ���������� ��������)
                    // �����������, ��� Latitude - ��� "������/�����", Longitude - "�����/������"
                    double offsetY = Math.Sin(angle) * distance;
                    double offsetX = Math.Cos(angle) * distance;

                    Coordinates weedPos = new Coordinates(
                        currentTractorPosition.Latitude + offsetY,
                        currentTractorPosition.Longitude + offsetX
                    );

                    features.Add(new FieldFeatureData(weedPos, FeatureType.DangerousWeed, "��������� ���������"));
                    Console.WriteLine($"[CameraVisionSystem]: ��������� ������� ������ � {weedPos}");
                }
            }
            else { Console.WriteLine("[CameraVisionSystem]: ������� �������� �� ���������� � ������� ����."); }
            return features;
        }

        // ������ ���������/����������� ����� ��������, ���� ControlUnit ����� ��� ���������
        public void ActivateSystem() { _isSystemActive = true; Console.WriteLine("[CameraVisionSystem]: ������������."); }
        public void DeactivateSystem() { _isSystemActive = false; Console.WriteLine("[CameraVisionSystem]: ��������������."); }
    }
}