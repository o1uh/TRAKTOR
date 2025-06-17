using Traktor.Core;       // ��� Logger
using Traktor.Interfaces; // ��� ITractorConfigurationBuilder

namespace Traktor.Builders
{
    /// <summary>
    /// �������� (�������������) ��� �������� ���������.
    /// ���������� ������� ������ ����� ���������� ��� ��������
    /// ��������� ���������������� ������������ TractorConfiguration.
    /// </summary>
    public class TractorConfigurationDirector
    {
        private const string SourceFilePath = "Builders/TractorConfigurationDirector.cs";
        private ITractorConfigurationBuilder _builder;

        /// <summary>
        /// �������������� ����� ��������� ���������.
        /// </summary>
        /// <param name="builder">���������, ������� ����� �������������� ��� ��������������� ��������.</param>
        public TractorConfigurationDirector(ITractorConfigurationBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Logger.Instance.Info(SourceFilePath, $"TractorConfigurationDirector ������. ������������ ���������: {_builder.GetType().FullName}.");
        }

        /// <summary>
        /// ������ ������������ ���������.
        /// </summary>
        /// <param name="builder">����� ���������.</param>
        public void SetBuilder(ITractorConfigurationBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Logger.Instance.Info(SourceFilePath, $"��������: ��������� ������� ��: {_builder.GetType().FullName}.");
        }

        /// <summary>
        /// ������������ "�������" ������������ ��������.
        /// </summary>
        public void ConstructBasicTractor(string modelName)
        {
            Logger.Instance.Info(SourceFilePath, $"��������: ������ ��������������� ������� ������������ ��� ������ '{modelName}'.");
            _builder.Reset(); // ��������, ��� ��������� � ������ ���������
            _builder.SetBaseParameters(modelName, EngineType.Diesel)
                    .SetTransmission(TransmissionType.Manual)
                    .SetHorsePower(120);
            // ��������� ��������� �������� �� ��������� (��� ������������, ��� GPS)
            Logger.Instance.Info(SourceFilePath, $"��������: ������� ������������ ��� '{modelName}' ������ ���������.");
        }

        /// <summary>
        /// ������������ "�����������" ������������ ��������.
        /// </summary>
        public void ConstructAdvancedTractor(string modelName)
        {
            Logger.Instance.Info(SourceFilePath, $"��������: ������ ��������������� ����������� ������������ ��� ������ '{modelName}'.");
            _builder.Reset();
            _builder.SetBaseParameters(modelName, EngineType.Hybrid)
                    .SetTransmission(TransmissionType.Automatic)
                    .SetHorsePower(250)
                    .SetAirConditioning(true)
                    .SetGPSModule(true);
            Logger.Instance.Info(SourceFilePath, $"��������: ����������� ������������ ��� '{modelName}' ������ ���������.");
        }

        /// <summary>
        /// ������������ "�������������" ������������ ��������.
        /// </summary>
        public void ConstructElectricTractor(string modelName)
        {
            Logger.Instance.Info(SourceFilePath, $"��������: ������ ��������������� ������������� ������������ ��� ������ '{modelName}'.");
            _builder.Reset();
            _builder.SetBaseParameters(modelName, EngineType.Electric)
                    .SetTransmission(TransmissionType.CVT) // ����� � ������� ���� �����������
                    .SetHorsePower(180) // ������� ����� ����� ������� �������� ������
                    .SetAirConditioning(true) // ������ ����
                    .SetGPSModule(true);
            Logger.Instance.Info(SourceFilePath, $"��������: ������������� ������������ ��� '{modelName}' ������ ���������.");
        }
    }
}