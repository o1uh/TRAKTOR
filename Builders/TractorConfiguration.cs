using Traktor.Core; // ��� Logger
using System.Text;  // ��� StringBuilder

namespace Traktor.Builders
{
    /// <summary>
    /// ������������ ����� ���������� ��� ��������.
    /// </summary>
    public enum EngineType { Diesel, Electric, Hybrid }

    /// <summary>
    /// ������������ ����� ����������� ��� ��������.
    /// </summary>
    public enum TransmissionType { Manual, Automatic, CVT }

    /// <summary>
    /// ������������ ������� ������ "������������ ��������",
    /// ������� ����� ���������������� � ������� �������� ���������.
    /// </summary>
    public class TractorConfiguration
    {
        private const string SourceFilePath = "Builders/TractorConfiguration.cs";

        /// <summary>
        /// �������� ��� ������ (���������) �������� ������ ��������.
        /// </summary>
        public string ModelName { get; internal set; }

        /// <summary>
        /// �������� ��� ������ (���������) ��� ���������.
        /// </summary>
        public EngineType Engine { get; internal set; }

        /// <summary>
        /// �������� ��� ������ (���������) ��� �����������.
        /// �� ���������: Manual.
        /// </summary>
        public TransmissionType Transmission { get; internal set; } = TransmissionType.Manual;

        /// <summary>
        /// �������� ��� ������ (���������) ������� ������������.
        /// �� ���������: false.
        /// </summary>
        public bool HasAirConditioning { get; internal set; } = false;

        /// <summary>
        /// �������� ��� ������ (���������) ������� GPS ������.
        /// �� ���������: false.
        /// </summary>
        public bool HasGPSModule { get; internal set; } = false;

        /// <summary>
        /// �������� ��� ������ (���������) �������� ��������� � ��������� �����.
        /// �� ���������: 100.
        /// </summary>
        public int HorsePower { get; internal set; } = 100;

        /// <summary>
        /// ���������� �����������. ������������ ���������� ��� �������� ����������.
        /// </summary>
        /// <param name="modelName">�������� ������.</param>
        /// <param name="engineType">��� ���������.</param>
        internal TractorConfiguration(string modelName, EngineType engineType)
        {
            ModelName = modelName ?? "UnknownModel";
            Engine = engineType;
        }

        /// <summary>
        /// ���������� (��������) ������� ������������ ��������.
        /// </summary>
        public void DisplayConfiguration()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"--- Tractor Configuration: {ModelName} ---");
            sb.AppendLine($"  Engine Type: {Engine}");
            sb.AppendLine($"  Transmission Type: {Transmission}");
            sb.AppendLine($"  Horse Power: {HorsePower} HP");
            sb.AppendLine($"  Air Conditioning: {(HasAirConditioning ? "Yes" : "No")}");
            sb.AppendLine($"  GPS Module: {(HasGPSModule ? "Yes" : "No")}");
            sb.AppendLine($"--------------------------------------");

            Logger.Instance.Info(SourceFilePath, sb.ToString());
        }
    }
}