using System;

namespace TractorAutopilot.DataModels
{
    /// <summary>
    /// ������� ����� ��� ������, ���������� � ��������.
    /// </summary>
    public abstract class SensorDataBase
    {
        /// <summary>
        /// �������� ��������� ����� ��������� ������.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// �������������� ����� ��������� <see cref="SensorDataBase"/>.
        /// </summary>
        protected SensorDataBase()
        {
            Timestamp = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// ������������ ������, ���������� � �������.
    /// </summary>
    /// <typeparam name="T">��� �������� ������.</typeparam>
    public class SensorData<T> : SensorDataBase
    {
        /// <summary>
        /// �������� �������� ������.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// �������� ��� ������ ��� ������� ��� ������.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SensorData{T}"/>.
        /// </summary>
        /// <param name="value">�������� ������.</param>
        /// <param name="dataType">��� ������/������� (�����������).</param>
        public SensorData(T value, string dataType = null)
        {
            Value = value;
            DataType = dataType ?? typeof(T).Name;
        }

        /// <summary>
        /// ���������� ��������� ������������� ������ �������.
        /// </summary>
        /// <returns>������, ����������� ������ �������.</returns>
        public override string ToString() => $"���: {DataType}, ��������: {Value} ({Timestamp})";
    }

    // ������� ���������� ����� ������ ��� ��������:
    public class DistanceData
    {
        public float Meters { get; }
        public DistanceData(float meters) { Meters = meters; }
        public override string ToString() => $"{Meters} �";
    }

    public class SoilMoistureData
    {
        public float Percentage { get; }
        public SoilMoistureData(float percentage) { Percentage = percentage; }
        public override string ToString() => $"{Percentage}%";
    }

    public class ImageData // ����� ��������� ����� ����������� ��� ���� � �����
    {
        public byte[] RawData { get; }
        public int Width { get; }
        public int Height { get; }
        public ImageData(byte[] rawData, int width, int height)
        {
            RawData = rawData;
            Width = width;
            Height = height;
        }
        public override string ToString() => $"����������� {Width}x{Height}, {RawData?.Length ?? 0} ����";
    }
}