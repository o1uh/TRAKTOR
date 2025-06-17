using Traktor.Builders; // ��� ������� � EngineType, TransmissionType, TractorConfiguration

namespace Traktor.Interfaces
{
    /// <summary>
    /// ��������� ��������� ��� ���������� �������� ������� TractorConfiguration.
    /// </summary>
    public interface ITractorConfigurationBuilder
    {
        /// <summary>
        /// ������ ������ � ��� ��������� (������ ��������� ���).
        /// </summary>
        /// <param name="modelName">�������� ������.</param>
        /// <param name="engineType">��� ���������.</param>
        /// <returns>������� ��������� ��������� ��� ������� �������.</returns>
        ITractorConfigurationBuilder SetBaseParameters(string modelName, EngineType engineType);

        /// <summary>
        /// ������ ��� �����������.
        /// </summary>
        /// <param name="transmissionType">��� �����������.</param>
        /// <returns>������� ��������� ��������� ��� ������� �������.</returns>
        ITractorConfigurationBuilder SetTransmission(TransmissionType transmissionType);

        /// <summary>
        /// ������������� ������� ������������.
        /// </summary>
        /// <param name="hasAirConditioning">True, ���� ����������� ����, ����� false.</param>
        /// <returns>������� ��������� ��������� ��� ������� �������.</returns>
        ITractorConfigurationBuilder SetAirConditioning(bool hasAirConditioning);

        /// <summary>
        /// ������������� ������� GPS ������.
        /// </summary>
        /// <param name="hasGPS">True, ���� GPS ������ ����, ����� false.</param>
        /// <returns>������� ��������� ��������� ��� ������� �������.</returns>
        ITractorConfigurationBuilder SetGPSModule(bool hasGPS);

        /// <summary>
        /// ������ �������� ���������.
        /// </summary>
        /// <param name="horsePower">�������� � ��������� �����.</param>
        /// <returns>������� ��������� ��������� ��� ������� �������.</returns>
        ITractorConfigurationBuilder SetHorsePower(int horsePower);

        /// <summary>
        /// ���������� ��������� � ��������� ��������� (���� ���������� ��������� �������������).
        /// </summary>
        void Reset();

        /// <summary>
        /// ���������� ��������� ������ TractorConfiguration.
        /// </summary>
        /// <returns>������������������ ��������� TractorConfiguration.</returns>
        TractorConfiguration Build();
    }
}