namespace Traktor.Interfaces
{
    /// <summary>
    /// Определяет контракт для взаимодействия с датчиками.
    /// Датчики могут возвращать различные типы данных.
    /// </summary>
    /// <typeparam name="T">Тип данных, возвращаемых датчиком.</typeparam>
    public interface ISensors<T>
    {
        /// <summary>
        /// Получает данные с датчика.
        /// </summary>
        /// <returns>Данные датчика указанного типа T.</returns>
        T GetData();
    }
}