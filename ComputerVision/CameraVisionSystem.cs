using Traktor.Interfaces;
using Traktor.DataModels;
using System.Drawing; // ��� Bitmap
using Traktor.Core;   // ��������� ��� Logger

namespace Traktor.ComputerVision
{
    /// <summary>
    /// ���������� ������� ������������� ������ �� ������ ������� ����������� � �����.
    /// �������� ������ �� ������� ������ ��� �������.
    /// </summary>
    public class CameraVisionSystem : IComputerVisionSystem
    {
        private static readonly Random _random = new Random();
        private bool _isSystemActive = true;
        private readonly ISensors<Bitmap> _cameraSensor; // ����������� �� ������� ������
        private const string SourceFilePath = "ComputerVision/CameraVisionSystem.cs"; // ���������� ���������

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="CameraVisionSystem"/>.
        /// </summary>
        /// <param name="cameraSensor">������ ������, ��������������� ����������� ��� �������.</param>
        /// <exception cref="ArgumentNullException">���� cameraSensor ����� null.</exception>
        public CameraVisionSystem(ISensors<Bitmap> cameraSensor)
        {
            _cameraSensor = cameraSensor ?? throw new ArgumentNullException(nameof(cameraSensor));
            // _isSystemActive = true; // �� ��������� �������
            Logger.Instance.Info(SourceFilePath, $"������� ������������ ������ �� ���� ����� ���������������� � ������� � �������� ������. �������: {_isSystemActive}");
        }

        /// <inheritdoc/>
        public List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition)
        {
            if (!_isSystemActive)
            {
                Logger.Instance.Warning(SourceFilePath, $"DetectObstacles: ������� ����� �� �������. ����������� ����������� ����������.");
                return new List<ObstacleData>();
            }

            Logger.Instance.Debug(SourceFilePath, $"DetectObstacles: ������ ������ � ������ ��� ����������� �����������. ������� ������� ��������: {currentTractorPosition}");
            Bitmap currentFrame = null;
            try
            {
                currentFrame = _cameraSensor.GetData();
                if (currentFrame == null)
                {
                    Logger.Instance.Warning(SourceFilePath, "DetectObstacles: �� ������� �������� ���� � ������ (null).");
                    return new List<ObstacleData>();
                }
                Logger.Instance.Debug(SourceFilePath, $"DetectObstacles: ������� ���� � ������ {currentFrame.Width}x{currentFrame.Height}. �������� �������...");
                // ����� ��� �� ������� ������ currentFrame...
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"DetectObstacles: ������ ��� ��������� ��� ��������� ����� � ������: {ex.Message}", ex);
                return new List<ObstacleData>();
            }
            finally
            {
                currentFrame?.Dispose(); // ����� ����������� ������� Bitmap
            }

            List<ObstacleData> detectedObstacles = new List<ObstacleData>();
            // �������� ����������� ����������� �� ������ "�������" �����
            // ����� ������������ currentFrame.Height/Width ��� ���� ���������� ������� imageCounter ��� ������� �� ������,
            // �� ��� �������� ������� ��� ����, �� ��� ���� ��������� ����� �����.
            if (_random.Next(0, 5) == 0)
            {
                const double metersToDegreesApproximation = 0.000009;
                double distanceForward = (1.0 + _random.NextDouble() * 5.0) * metersToDegreesApproximation;
                double sideOffset = (_random.NextDouble() - 0.5) * 2.0 * metersToDegreesApproximation;

                Coordinates obstaclePos = new Coordinates(
                    currentTractorPosition.Latitude + distanceForward,
                    currentTractorPosition.Longitude + sideOffset
                );

                string description = _random.Next(0, 2) == 0 ? "��������� ������ (������)" : "����� ������� ����� (������)";

                detectedObstacles.Add(new ObstacleData(obstaclePos, description));
                Logger.Instance.Info(SourceFilePath, $"DetectObstacles: �� ����������� ������� ����� ����������: \"{description}\" � {obstaclePos}");
            }
            else
            {
                Logger.Instance.Debug(SourceFilePath, "DetectObstacles: ����������� �� ������ ������� ����� �� ����������.");
            }
            return detectedObstacles;
        }

        /// <inheritdoc/>
        public List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition)
        {
            if (!_isSystemActive)
            {
                Logger.Instance.Warning(SourceFilePath, $"AnalyzeFieldFeatures: ������� ����� �� �������. ������ ������������ ���� ����������.");
                return new List<FieldFeatureData>();
            }

            Logger.Instance.Debug(SourceFilePath, $"AnalyzeFieldFeatures: ������ ������ � ������ ��� ������� ������������ ����. ������� ������� ��������: {currentTractorPosition}.");
            Bitmap currentFrame = null;
            try
            {
                currentFrame = _cameraSensor.GetData();
                if (currentFrame == null)
                {
                    Logger.Instance.Warning(SourceFilePath, "AnalyzeFieldFeatures: �� ������� �������� ���� � ������ (null).");
                    return new List<FieldFeatureData>();
                }
                Logger.Instance.Debug(SourceFilePath, $"AnalyzeFieldFeatures: ������� ���� � ������ {currentFrame.Width}x{currentFrame.Height}. �������� �������...");
                // ������ currentFrame �� ������� � �.�.
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"AnalyzeFieldFeatures: ������ ��� ��������� ��� ��������� ����� � ������: {ex.Message}", ex);
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
                    if (typeRoll == 0) { type = FeatureType.DangerousWeed; details = "���� ����� (������)"; }
                    else if (typeRoll == 1) { type = FeatureType.PestInfestation; details = "������� (������)"; }
                    else { type = FeatureType.WaterLogging; details = "��������� ���� (������)"; }

                    detectedFeatures.Add(new FieldFeatureData(featurePos, type, details));
                    Logger.Instance.Info(SourceFilePath, $"AnalyzeFieldFeatures: �� ����������� ������� ����� ���������� �����������: {type} \"{details}\" � {featurePos}");
                }
            }
            else
            {
                Logger.Instance.Debug(SourceFilePath, "AnalyzeFieldFeatures: �������������� ������������ �� ������ ������� ����� �� ����������.");
            }
            return detectedFeatures;
        }

        /// <summary>
        /// ���������� ������� ����� (���������� ����).
        /// </summary>
        public void ActivateInternal()
        {
            _isSystemActive = true;
            Logger.Instance.Info(SourceFilePath, "������� ����� ��������� ������������.");
        }

        /// <summary>
        /// ������������ ������� ����� (���������� ����).
        /// </summary>
        public void DeactivateInternal()
        {
            _isSystemActive = false;
            Logger.Instance.Info(SourceFilePath, "������� ����� ��������� ��������������.");
        }
    }
}