using Traktor.Interfaces;

namespace Traktor.Sensors
{
    public class DistanceSensor : ISensors<double>
    {
        // ����� �������� �����������, ���� ������� ����� �����-�� ������������
        // public DistanceSensor() { }

        /// <summary>
        /// ��������� ��������� ������ � ���������� �� �������.
        /// </summary>
        /// <returns>���������� ���������� � ������.</returns>
        public double GetData()
        {
            // � �������� ������� ����� ����� ��� ��� �������������� � ���������� ��������.
            // ��� ������ ��������� ������.
            Random rnd = new Random();
            // �����������, ������ ����� "������" �� 0.1 �� 50 ������.
            // � ������ ����� ������� "������" ��� ���������� �������� (��������, -1 ��� double.NaN).
            // ���� ��� �������� ������ ��������� �������� �������� ��� ������.
            double distance = 0.1 + rnd.NextDouble() * (50.0 - 0.1);
            Console.WriteLine($"[DistanceSensor]: ������������� ����������: {distance:F2} �"); // ��������� ����� ��� �������
            return distance;
        }
    }
}