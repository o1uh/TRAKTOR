using Traktor.Interfaces;
using Traktor.DataModels;

namespace Traktor.Sensors
{
    public class SoilSensor : ISensors<SoilSensorData>
    {
        /// <summary>
        /// ��������� ��������� ������ � ��������� �����.
        /// </summary>
        /// <returns>��������� <see cref="SoilSensorData"/> � ������� � �����.</returns>
        public SoilSensorData GetData()
        {
            // � �������� ������� ����� ����� ��� ��� �������������� � ���������� ��������.
            // ��� ������ ��������� ������.
            Random rnd = new Random();

            double moisture = 10.0 + rnd.NextDouble() * (60.0 - 10.0); // ��������� �� 10% �� 60%
            double temperature = 5.0 + rnd.NextDouble() * (25.0 - 5.0);  // ����������� �� 5�C �� 25�C

            var data = new SoilSensorData(moisture, temperature);
            Console.WriteLine($"[SoilSensor]: {data}"); // ��������� �����
            return data;
        }
    }
}