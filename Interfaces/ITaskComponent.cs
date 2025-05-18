namespace Traktor.Interfaces 
{
    /// <summary>
    /// Общий интерфейс для компонентов задачи (как для простых, так и для составных).
    /// Определяет операции, общие для всех компонентов.
    /// </summary>
    public interface ITaskComponent
    {
        /// <summary>
        /// "Выполняет" задачу. В макете это будет логирование.
        /// </summary>
        /// <param name="indent">Строка отступа для визуализации иерархии в логах.</param>
        void Execute(string indent = "");

        /// <summary>
        /// Возвращает имя задачи.
        /// </summary>
        /// <returns>Имя задачи.</returns>
        string GetName();
    }
}