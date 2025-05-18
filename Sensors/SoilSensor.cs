using Traktor.Interfaces;
using Traktor.DataModels;

namespace Traktor.Sensors
{
    /// <summary>
    /// ��������� ������ ������� ��������� �����.
    /// </summary>
    public class SoilSensor : ISensors<SoilSensorData>
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SoilSensor"/>.
        /// </summary>
        public SoilSensor()
        {
            Console.WriteLine($"[Sensors/SoilSensor.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ������ ����� ���������������.");
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
            Console.WriteLine($"[Sensors/SoilSensor.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: �������� ������ �� ������� �����: {data}");
            return data;
        }
    }
}