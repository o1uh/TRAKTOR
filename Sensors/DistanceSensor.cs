using Traktor.Interfaces;

namespace Traktor.Sensors
{
    /// <summary>
    /// ��������� ������ ������� ����������.
    /// </summary>
    public class DistanceSensor : ISensors<double>
    {
        private static readonly Random _random = new Random(); // ����� ������������ ���� ��������� Random

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="DistanceSensor"/>.
        /// </summary>
        public DistanceSensor()
        {
            // ����������� ����� ���� ����������� ��� ������������� ����������� � ��������� �������
            // ��� ��� ��������� ��������� ���������� ���������.
            Console.WriteLine($"[Sensors/DistanceSensor.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ������ ���������� ���������������.");
        }

        /// <summary>
        /// ��������� ��������� ������ � ���������� �� �������.
        /// </summary>
        /// <returns>���������� ���������� � ������ (��������� �������� �� 0.1 �� 50.0 ��� ������).</returns>
        public double GetData()
        {
            // � �������� ������� ����� ����� ��� ��� �������������� � ���������� ��������.
            // ��� ������ ��������� ������.

            // �����������, ������ ����� "������" �� 0.1 �� 50 ������.
            double distance = 0.1 + _random.NextDouble() * (50.0 - 0.1);
            Console.WriteLine($"[Sensors/DistanceSensor.cs]-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ������������� ����������: {distance:F2} �");
            return distance;
        }
    }
}