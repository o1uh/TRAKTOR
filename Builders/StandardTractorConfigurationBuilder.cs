using System; 
using Traktor.Core;       // Для Logger
using Traktor.Interfaces; // Для ITractorConfigurationBuilder

namespace Traktor.Builders
{
    /// <summary>
    /// Конкретная реализация строителя для TractorConfiguration.
    /// Позволяет пошагово создавать и настраивать объект конфигурации трактора.
    /// </summary>
    public class StandardTractorConfigurationBuilder : ITractorConfigurationBuilder
    {
        private const string SourceFilePath = "Builders/StandardTractorConfigurationBuilder.cs";
        private TractorConfiguration _configuration;

        // Переменные для хранения базовых параметров, если Reset вызывается до Build
        private string _modelName;
        private EngineType _engineType;

        /// <summary>
        /// Инициализирует новый экземпляр StandardTractorConfigurationBuilder.
        /// </summary>
        public StandardTractorConfigurationBuilder()
        {
            // Инициализация базовых параметров по умолчанию или пустыми значениями
            // Reset() будет вызван для создания первого объекта _configuration
            this.Reset();
            Logger.Instance.Info(SourceFilePath, "StandardTractorConfigurationBuilder создан и сброшен в начальное состояние.");
        }

        /// <inheritdoc/>
        public void Reset()
        {
            // Устанавливаем какие-то базовые значения по умолчанию для "обязательных" полей,
            // которые будут перезаписаны SetBaseParameters.
            _modelName = "DefaultModel";
            _engineType = EngineType.Diesel;
            _configuration = new TractorConfiguration(_modelName, _engineType);
            Logger.Instance.Debug(SourceFilePath, "Builder сброшен. Создан новый экземпляр TractorConfiguration с параметрами по умолчанию.");
        }

        /// <inheritdoc/>
        public ITractorConfigurationBuilder SetBaseParameters(string modelName, EngineType engineType)
        {
            if (string.IsNullOrWhiteSpace(modelName))
            {
                // Для макета просто логируем, в реальном коде можно бросить ArgumentException
                Logger.Instance.Warning(SourceFilePath, "SetBaseParameters: Попытка установить пустое имя модели. Используется предыдущее или 'DefaultModel'.");
            }
            else
            {
                _configuration.ModelName = modelName;
                _modelName = modelName; // Сохраняем для случая Reset
                Logger.Instance.Debug(SourceFilePath, $"Установлена модель: '{_configuration.ModelName}'.");
            }

            _configuration.Engine = engineType;
            _engineType = engineType; // Сохраняем для случая Reset
            Logger.Instance.Debug(SourceFilePath, $"Установлен тип двигателя: '{_configuration.Engine}'.");
            return this;
        }

        /// <inheritdoc/>
        public ITractorConfigurationBuilder SetTransmission(TransmissionType transmissionType)
        {
            _configuration.Transmission = transmissionType;
            Logger.Instance.Debug(SourceFilePath, $"Установлена трансмиссия: '{_configuration.Transmission}'.");
            return this;
        }

        /// <inheritdoc/>
        public ITractorConfigurationBuilder SetAirConditioning(bool hasAirConditioning)
        {
            _configuration.HasAirConditioning = hasAirConditioning;
            Logger.Instance.Debug(SourceFilePath, $"Установлено наличие кондиционера: {_configuration.HasAirConditioning}.");
            return this;
        }

        /// <inheritdoc/>
        public ITractorConfigurationBuilder SetGPSModule(bool hasGPS)
        {
            _configuration.HasGPSModule = hasGPS;
            Logger.Instance.Debug(SourceFilePath, $"Установлено наличие GPS модуля: {_configuration.HasGPSModule}.");
            return this;
        }

        /// <inheritdoc/>
        public ITractorConfigurationBuilder SetHorsePower(int horsePower)
        {
            if (horsePower <= 0)
            {
                // Для макета просто логируем, в реальном коде можно бросить ArgumentOutOfRangeException
                Logger.Instance.Warning(SourceFilePath, $"SetHorsePower: Попытка установить некорректную мощность ({horsePower} л.с.). Используется предыдущее значение или значение по умолчанию.");
            }
            else
            {
                _configuration.HorsePower = horsePower;
                Logger.Instance.Debug(SourceFilePath, $"Установлена мощность: {_configuration.HorsePower} л.с.");
            }
            return this;
        }

        /// <inheritdoc/>
        public TractorConfiguration Build()
        {
            // Проверка, были ли установлены базовые параметры, если они строго обязательны
            if (_configuration.ModelName == "DefaultModel" && _modelName == "DefaultModel") // Проверка, что SetBaseParameters не вызывался с осмысленным именем
            {
                Logger.Instance.Warning(SourceFilePath, "Build: Построение конфигурации с именем модели по умолчанию. Возможно, SetBaseParameters не был вызван или был вызван с некорректным именем.");
            }

            Logger.Instance.Info(SourceFilePath, $"Сборка TractorConfiguration для модели '{_configuration.ModelName}' завершена.");

            // Важно: возвращаем текущий _configuration и готовим строитель к следующей сборке,
            // создавая новый экземпляр продукта для накопления.
            TractorConfiguration result = _configuration;
            this.Reset(); // Сбрасываем строитель для возможности повторного использования
                          // или чтобы он не хранил ссылку на уже отданный продукт.
                          // Если Reset не нужен для повторного использования, то его можно вызвать только в конструкторе.
            return result;
        }
    }
}