// Interfaces/ISensors.cs
namespace TractorAutopilot.Interfaces
{
    public interface ISensors<T>
    {
        /// <summary>
        /// Получает данные с датчика.
        /// </summary>
        /// <returns>Данные с датчика.</returns>
        T GetData();
    }
}