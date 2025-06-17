using System; 
using Traktor.Core;       // ��� Logger
using Traktor.Interfaces; // ��� ITractorConfigurationBuilder

namespace Traktor.Builders
{
    /// <summary>
    /// ���������� ���������� ��������� ��� TractorConfiguration.
    /// ��������� �������� ��������� � ����������� ������ ������������ ��������.
    /// </summary>
    public class StandardTractorConfigurationBuilder : ITractorConfigurationBuilder
    {
        private const string SourceFilePath = "Builders/StandardTractorConfigurationBuilder.cs";
        private TractorConfiguration _configuration;

        // ���������� ��� �������� ������� ����������, ���� Reset ���������� �� Build
        private string _modelName;
        private EngineType _engineType;

        /// <summary>
        /// �������������� ����� ��������� StandardTractorConfigurationBuilder.
        /// </summary>
        public StandardTractorConfigurationBuilder()
        {
            // ������������� ������� ���������� �� ��������� ��� ������� ����������
            // Reset() ����� ������ ��� �������� ������� ������� _configuration
            this.Reset();
            Logger.Instance.Info(SourceFilePath, "StandardTractorConfigurationBuilder ������ � ������� � ��������� ���������.");
        }

        /// <inheritdoc/>
        public void Reset()
        {
            // ������������� �����-�� ������� �������� �� ��������� ��� "������������" �����,
            // ������� ����� ������������ SetBaseParameters.
            _modelName = "DefaultModel";
            _engineType = EngineType.Diesel;
            _configuration = new TractorConfiguration(_modelName, _engineType);
            Logger.Instance.Debug(SourceFilePath, "Builder �������. ������ ����� ��������� TractorConfiguration � ����������� �� ���������.");
        }

        /// <inheritdoc/>
        public ITractorConfigurationBuilder SetBaseParameters(string modelName, EngineType engineType)
        {
            if (string.IsNullOrWhiteSpace(modelName))
            {
                // ��� ������ ������ ��������, � �������� ���� ����� ������� ArgumentException
                Logger.Instance.Warning(SourceFilePath, "SetBaseParameters: ������� ���������� ������ ��� ������. ������������ ���������� ��� 'DefaultModel'.");
            }
            else
            {
                _configuration.ModelName = modelName;
                _modelName = modelName; // ��������� ��� ������ Reset
                Logger.Instance.Debug(SourceFilePath, $"����������� ������: '{_configuration.ModelName}'.");
            }

            _configuration.Engine = engineType;
            _engineType = engineType; // ��������� ��� ������ Reset
            Logger.Instance.Debug(SourceFilePath, $"���������� ��� ���������: '{_configuration.Engine}'.");
            return this;
        }

        /// <inheritdoc/>
        public ITractorConfigurationBuilder SetTransmission(TransmissionType transmissionType)
        {
            _configuration.Transmission = transmissionType;
            Logger.Instance.Debug(SourceFilePath, $"����������� �����������: '{_configuration.Transmission}'.");
            return this;
        }

        /// <inheritdoc/>
        public ITractorConfigurationBuilder SetAirConditioning(bool hasAirConditioning)
        {
            _configuration.HasAirConditioning = hasAirConditioning;
            Logger.Instance.Debug(SourceFilePath, $"����������� ������� ������������: {_configuration.HasAirConditioning}.");
            return this;
        }

        /// <inheritdoc/>
        public ITractorConfigurationBuilder SetGPSModule(bool hasGPS)
        {
            _configuration.HasGPSModule = hasGPS;
            Logger.Instance.Debug(SourceFilePath, $"����������� ������� GPS ������: {_configuration.HasGPSModule}.");
            return this;
        }

        /// <inheritdoc/>
        public ITractorConfigurationBuilder SetHorsePower(int horsePower)
        {
            if (horsePower <= 0)
            {
                // ��� ������ ������ ��������, � �������� ���� ����� ������� ArgumentOutOfRangeException
                Logger.Instance.Warning(SourceFilePath, $"SetHorsePower: ������� ���������� ������������ �������� ({horsePower} �.�.). ������������ ���������� �������� ��� �������� �� ���������.");
            }
            else
            {
                _configuration.HorsePower = horsePower;
                Logger.Instance.Debug(SourceFilePath, $"����������� ��������: {_configuration.HorsePower} �.�.");
            }
            return this;
        }

        /// <inheritdoc/>
        public TractorConfiguration Build()
        {
            // ��������, ���� �� ����������� ������� ���������, ���� ��� ������ �����������
            if (_configuration.ModelName == "DefaultModel" && _modelName == "DefaultModel") // ��������, ��� SetBaseParameters �� ��������� � ����������� ������
            {
                Logger.Instance.Warning(SourceFilePath, "Build: ���������� ������������ � ������ ������ �� ���������. ��������, SetBaseParameters �� ��� ������ ��� ��� ������ � ������������ ������.");
            }

            Logger.Instance.Info(SourceFilePath, $"������ TractorConfiguration ��� ������ '{_configuration.ModelName}' ���������.");

            // �����: ���������� ������� _configuration � ������� ��������� � ��������� ������,
            // �������� ����� ��������� �������� ��� ����������.
            TractorConfiguration result = _configuration;
            this.Reset(); // ���������� ��������� ��� ����������� ���������� �������������
                          // ��� ����� �� �� ������ ������ �� ��� �������� �������.
                          // ���� Reset �� ����� ��� ���������� �������������, �� ��� ����� ������� ������ � ������������.
            return result;
        }
    }
}