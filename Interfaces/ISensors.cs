namespace Traktor.Interfaces
{
    public interface ISensors<T>
    {
        /// <summary>
        /// Получает данные с датчика.
        /// </summary>
        /// <returns>Данные датчика указанного типа T.</returns>
        T GetData();
    }
}