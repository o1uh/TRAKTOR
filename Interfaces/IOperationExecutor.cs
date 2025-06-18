namespace Traktor.Interfaces
{
    /// <summary>
    /// Интерфейс Реализатора (Implementor) для паттерна Мост.
    /// Определяет низкоуровневые операции для выполнения шагов какой-либо операции.
    /// </summary>
    public interface IOperationExecutor
    {
        /// <summary>
        /// Начинает выполнение шага операции.
        /// </summary>
        /// <param name="operationName">Название операции.</param>
        /// <param name="stepDetails">Детали текущего шага.</param>
        void StartStep(string operationName, string stepDetails);

        /// <summary>
        /// "Обрабатывает" текущий шаг.
        /// </summary>
        /// <param name="operationName">Название операции.</param>
        void ProcessStep(string operationName);

        /// <summary>
        /// Завершает выполнение шага операции.
        /// </summary>
        /// <param name="operationName">Название операции.</param>
        /// <param name="stepDetails">Детали завершенного шага.</param>
        /// <returns>Результат выполнения шага (для макета - строка).</returns>
        string FinishStep(string operationName, string stepDetails);
    }
}