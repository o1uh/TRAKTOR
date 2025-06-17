using Traktor.Core;       // Для Logger
using Traktor.Interfaces; // Для ITractorConfigurationBuilder

namespace Traktor.Builders
{
    /// <summary>
    /// Директор (Распорядитель) для паттерна Строитель.
    /// Определяет порядок вызова шагов построения для создания
    /// различных предопределенных конфигураций TractorConfiguration.
    /// </summary>
    public class TractorConfigurationDirector
    {
        private const string SourceFilePath = "Builders/TractorConfigurationDirector.cs";
        private ITractorConfigurationBuilder _builder;

        /// <summary>
        /// Инициализирует новый экземпляр директора.
        /// </summary>
        /// <param name="builder">Строитель, который будет использоваться для конструирования объектов.</param>
        public TractorConfigurationDirector(ITractorConfigurationBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Logger.Instance.Info(SourceFilePath, $"TractorConfigurationDirector создан. Используемый строитель: {_builder.GetType().FullName}.");
        }

        /// <summary>
        /// Меняет используемый строитель.
        /// </summary>
        /// <param name="builder">Новый строитель.</param>
        public void SetBuilder(ITractorConfigurationBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Logger.Instance.Info(SourceFilePath, $"Директор: Строитель изменен на: {_builder.GetType().FullName}.");
        }

        /// <summary>
        /// Конструирует "базовую" конфигурацию трактора.
        /// </summary>
        public void ConstructBasicTractor(string modelName)
        {
            Logger.Instance.Info(SourceFilePath, $"Директор: Начало конструирования базовой конфигурации для модели '{modelName}'.");
            _builder.Reset(); // Убедимся, что строитель в чистом состоянии
            _builder.SetBaseParameters(modelName, EngineType.Diesel)
                    .SetTransmission(TransmissionType.Manual)
                    .SetHorsePower(120);
            // Остальные параметры остаются по умолчанию (без кондиционера, без GPS)
            Logger.Instance.Info(SourceFilePath, $"Директор: Базовая конфигурация для '{modelName}' задана строителю.");
        }

        /// <summary>
        /// Конструирует "продвинутую" конфигурацию трактора.
        /// </summary>
        public void ConstructAdvancedTractor(string modelName)
        {
            Logger.Instance.Info(SourceFilePath, $"Директор: Начало конструирования продвинутой конфигурации для модели '{modelName}'.");
            _builder.Reset();
            _builder.SetBaseParameters(modelName, EngineType.Hybrid)
                    .SetTransmission(TransmissionType.Automatic)
                    .SetHorsePower(250)
                    .SetAirConditioning(true)
                    .SetGPSModule(true);
            Logger.Instance.Info(SourceFilePath, $"Директор: Продвинутая конфигурация для '{modelName}' задана строителю.");
        }

        /// <summary>
        /// Конструирует "электрическую" конфигурацию трактора.
        /// </summary>
        public void ConstructElectricTractor(string modelName)
        {
            Logger.Instance.Info(SourceFilePath, $"Директор: Начало конструирования электрической конфигурации для модели '{modelName}'.");
            _builder.Reset();
            _builder.SetBaseParameters(modelName, EngineType.Electric)
                    .SetTransmission(TransmissionType.CVT) // Часто у электро свои трансмиссии
                    .SetHorsePower(180) // Электро может иметь хороший крутящий момент
                    .SetAirConditioning(true) // Обычно есть
                    .SetGPSModule(true);
            Logger.Instance.Info(SourceFilePath, $"Директор: Электрическая конфигурация для '{modelName}' задана строителю.");
        }
    }
}