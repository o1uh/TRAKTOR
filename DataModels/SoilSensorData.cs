namespace Traktor.DataModels
{
    /// <summary>
    /// ������������ ������, ���������� �� ������� ��������� �����.
    /// </summary>
    public struct SoilSensorData
    {
        /// <summary>
        /// ��������� ����� (��������, � ���������).
        /// </summary>
        public double Moisture { get; }

        /// <summary>
        /// ����������� ����� (��������, � �������� �������).
        /// </summary>
        public double Temperature { get; }

        // ����� �������� � ������ ��������� � �������, ���� ����� (pH, ������������������ � �.�.)

        /// <summary>
        /// �������������� ����� ��������� ��������� <see cref="SoilSensorData"/>.
        /// </summary>
        /// <param name="moisture">�������� ��������� �����.</param>
        /// <param name="temperature">�������� ����������� �����.</param>
        public SoilSensorData(double moisture, double temperature)
        {
            Moisture = moisture;
            Temperature = temperature;
        }

        /// <summary>
        /// ���������� ��������� ������������� ������ � ��������� �����.
        /// </summary>
        /// <returns>������ � ����������� � ��������� � ����������� �����.</returns>
        public override string ToString()
        {
            return $"�����: ���������={Moisture:F1}%, �����������={Temperature:F1}�C";
        }
    }
}