using Traktor.Core; // Для Logger
using System.Text;  // Для StringBuilder

namespace Traktor.Builders
{
    /// <summary>
    /// Перечисление типов двигателей для трактора.
    /// </summary>
    public enum EngineType { Diesel, Electric, Hybrid }

    /// <summary>
    /// Перечисление типов трансмиссии для трактора.
    /// </summary>
    public enum TransmissionType { Manual, Automatic, CVT }

    /// <summary>
    /// Представляет сложный объект "Конфигурация Трактора",
    /// который будет конструироваться с помощью паттерна Строитель.
    /// </summary>
    public class TractorConfiguration
    {
        private const string SourceFilePath = "Builders/TractorConfiguration.cs";

        /// <summary>
        /// Получает или задает (внутренне) название модели трактора.
        /// </summary>
        public string ModelName { get; internal set; }

        /// <summary>
        /// Получает или задает (внутренне) тип двигателя.
        /// </summary>
        public EngineType Engine { get; internal set; }

        /// <summary>
        /// Получает или задает (внутренне) тип трансмиссии.
        /// По умолчанию: Manual.
        /// </summary>
        public TransmissionType Transmission { get; internal set; } = TransmissionType.Manual;

        /// <summary>
        /// Получает или задает (внутренне) наличие кондиционера.
        /// По умолчанию: false.
        /// </summary>
        public bool HasAirConditioning { get; internal set; } = false;

        /// <summary>
        /// Получает или задает (внутренне) наличие GPS модуля.
        /// По умолчанию: false.
        /// </summary>
        public bool HasGPSModule { get; internal set; } = false;

        /// <summary>
        /// Получает или задает (внутренне) мощность двигателя в лошадиных силах.
        /// По умолчанию: 100.
        /// </summary>
        public int HorsePower { get; internal set; } = 100;

        /// <summary>
        /// Внутренний конструктор. Используется строителем для создания экземпляра.
        /// </summary>
        /// <param name="modelName">Название модели.</param>
        /// <param name="engineType">Тип двигателя.</param>
        internal TractorConfiguration(string modelName, EngineType engineType)
        {
            ModelName = modelName ?? "UnknownModel";
            Engine = engineType;
        }

        /// <summary>
        /// Отображает (логирует) текущую конфигурацию трактора.
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