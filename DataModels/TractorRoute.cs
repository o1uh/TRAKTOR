using System.Collections; // Для IEnumerable
using Traktor.DataModels; // Для Coordinates
using Traktor.Core;       // Для Logger

namespace Traktor.Navigation // Или Traktor.DataModels, или Traktor.Core, как вам удобнее для макета
{
    /// <summary>
    /// Представляет маршрут трактора как коллекцию координат,
    /// демонстрируя паттерн Итератор через реализацию IEnumerable<Coordinates>.
    /// </summary>
    public class TractorRoute : IEnumerable<Coordinates>
    {
        private readonly List<Coordinates> _points;
        private const string SourceFilePath = "Navigation/TractorRoute.cs"; // Укажите актуальный путь

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TractorRoute"/>.
        /// </summary>
        /// <param name="points">Список координат, составляющих маршрут.</param>
        public TractorRoute(List<Coordinates> points)
        {
            _points = points ?? new List<Coordinates>(); // Если null, создаем пустой список
            Logger.Instance.Info(SourceFilePath, $"TractorRoute создан. Количество точек в маршруте: {_points.Count}.");
        }

        /// <summary>
        /// Возвращает перечислитель, который осуществляет итерацию по коллекции координат.
        /// </summary>
        /// <returns>Перечислитель для коллекции <see cref="Coordinates"/>.</returns>
        public IEnumerator<Coordinates> GetEnumerator()
        {
            Logger.Instance.Debug(SourceFilePath, "GetEnumerator<Coordinates>() вызван. Запрос на итерацию по точкам маршрута.");
            // Делегируем получение итератора стандартному списку.
            // В более сложном случае здесь могла бы быть кастомная логика итерации.
            int pointCounter = 0;
            foreach (var point in _points)
            {
                pointCounter++;
                Logger.Instance.Debug(SourceFilePath, $"Итератор: Возвращается точка #{pointCounter}: {point}");
                yield return point; // 'yield return' делает магию итератора
            }
            Logger.Instance.Debug(SourceFilePath, $"Итератор: Все {_points.Count} точек были перебраны.");
        }

        /// <summary>
        /// Возвращает перечислитель, который осуществляет итерацию по коллекции. (Для нетипизированного IEnumerable)
        /// </summary>
        /// <returns>Перечислитель, который можно использовать для итерации по коллекции.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            Logger.Instance.Debug(SourceFilePath, "IEnumerable.GetEnumerator() вызван (нетипизированный). Делегирование типизированному GetEnumerator.");
            return GetEnumerator();
        }

        // Дополнительный метод для демонстрации, что это не просто список
        public int GetPointCount()
        {
            return _points.Count;
        }
    }
}