using Traktor.Interfaces;
using Traktor.DataModels;
using Traktor.Core;   // ��������� ��� Logger

namespace Traktor.Sensors
{
    /// <summary>
    /// ��������� ������ ������� ��������� �����.
    /// </summary>
    public class SoilSensor : ISensors<SoilSensorData>
    {
        private static readonly Random _random = new Random();
        private const string SourceFilePath = "Sensors/SoilSensor.cs"; // ���������� ��������� ��� ���� �����

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SoilSensor"/>.
        /// </summary>
        public SoilSensor()
        {
            // ���������� Logger.Instance.Info ��� ����������� �������������
            Logger.Instance.Info(SourceFilePath, "������ ����� ���������������.");
        }

        /// <summary>
        /// ��������� ��������� ������ � ��������� �����.
        /// </summary>
        /// <returns>��������� <see cref="SoilSensorData"/> � ��������������� ������� � ��������� � ����������� �����.</returns>
        public SoilSensorData GetData()
        {
            // � �������� ������� ����� ����� ��� ��� �������������� � ���������� ��������.
            // ��� ������ ��������� ������.
            double moisture = 10.0 + _random.NextDouble() * (60.0 - 10.0); // ��������� �� 10% �� 60%
            double temperature = 5.0 + _random.NextDouble() * (25.0 - 5.0);  // ����������� �� 5�C �� 25�C

            var data = new SoilSensorData(moisture, temperature);
            // ���������� Logger.Instance.Debug ��� ����������� ���������� ������
            Logger.Instance.Debug(SourceFilePath, $"�������� ������ �� ������� �����: {data}");
            return data;
        }
    }
}