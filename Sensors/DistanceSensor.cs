using Traktor.Interfaces;
using Traktor.Core;   // ��������� ��� Logger

namespace Traktor.Sensors
{
    /// <summary>
    /// ��������� ������ ������� ����������.
    /// </summary>
    public class DistanceSensor : ISensors<double>
    {
        private static readonly Random _random = new Random(); // ����� ������������ ���� ��������� Random
        private const string SourceFilePath = "Sensors/DistanceSensor.cs"; // ���������� ��������� ��� ���� �����

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="DistanceSensor"/>.
        /// </summary>
        public DistanceSensor()
        {
            // ����������� ����� ���� ����������� ��� ������������� ����������� � ��������� �������
            // ��� ��� ��������� ��������� ���������� ���������.
            // ���������� Logger.Instance.Info ��� ����������� �������������
            Logger.Instance.Info(SourceFilePath, "������ ���������� ���������������.");
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
            // ���������� Logger.Instance.Debug ��� ����������� ����������� ����������, �.�. ��� ����� ���� ������ ��������
            Logger.Instance.Debug(SourceFilePath, $"������������� ����������: {distance:F2} �");
            return distance;
        }
    }
}