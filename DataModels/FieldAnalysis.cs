namespace TractorAutopilot.DataModels
{
    /// <summary>
    /// Содержит результаты анализа состояния поля.
    /// </summary>
    public class FieldAnalysis
    {
        /// <summary>
        /// Получает или задает описание состояния почвы.
        /// </summary>
        public string SoilCondition { get; set; } = "Норма";

        /// <summary>
        /// Получает или задает описание здоровья растений.
        /// </summary>
        public string PlantHealth { get; set; } = "Хорошее";

        // Можно добавить другие параметры анализа

        /// <summary>
        /// Возвращает строковое представление анализа поля.
        /// </summary>
        /// <returns>Строка, описывающая результаты анализа.</returns>
        public override string ToString() => $"Состояние почвы: {SoilCondition}, Здоровье растений: {PlantHealth}.";
    }
}