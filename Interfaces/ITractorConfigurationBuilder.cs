using Traktor.Builders; // Для доступа к EngineType, TransmissionType, TractorConfiguration

namespace Traktor.Interfaces
{
    /// <summary>
    /// Интерфейс Строителя для пошагового создания объекта TractorConfiguration.
    /// </summary>
    public interface ITractorConfigurationBuilder
    {
        /// <summary>
        /// Задает модель и тип двигателя (обычно начальный шаг).
        /// </summary>
        /// <param name="modelName">Название модели.</param>
        /// <param name="engineType">Тип двигателя.</param>
        /// <returns>Текущий экземпляр строителя для цепочки вызовов.</returns>
        ITractorConfigurationBuilder SetBaseParameters(string modelName, EngineType engineType);

        /// <summary>
        /// Задает тип трансмиссии.
        /// </summary>
        /// <param name="transmissionType">Тип трансмиссии.</param>
        /// <returns>Текущий экземпляр строителя для цепочки вызовов.</returns>
        ITractorConfigurationBuilder SetTransmission(TransmissionType transmissionType);

        /// <summary>
        /// Устанавливает наличие кондиционера.
        /// </summary>
        /// <param name="hasAirConditioning">True, если кондиционер есть, иначе false.</param>
        /// <returns>Текущий экземпляр строителя для цепочки вызовов.</returns>
        ITractorConfigurationBuilder SetAirConditioning(bool hasAirConditioning);

        /// <summary>
        /// Устанавливает наличие GPS модуля.
        /// </summary>
        /// <param name="hasGPS">True, если GPS модуль есть, иначе false.</param>
        /// <returns>Текущий экземпляр строителя для цепочки вызовов.</returns>
        ITractorConfigurationBuilder SetGPSModule(bool hasGPS);

        /// <summary>
        /// Задает мощность двигателя.
        /// </summary>
        /// <param name="horsePower">Мощность в лошадиных силах.</param>
        /// <returns>Текущий экземпляр строителя для цепочки вызовов.</returns>
        ITractorConfigurationBuilder SetHorsePower(int horsePower);

        /// <summary>
        /// Сбрасывает строитель в начальное состояние (если необходимо повторное использование).
        /// </summary>
        void Reset();

        /// <summary>
        /// Возвращает собранный объект TractorConfiguration.
        /// </summary>
        /// <returns>Сконфигурированный экземпляр TractorConfiguration.</returns>
        TractorConfiguration Build();
    }
}