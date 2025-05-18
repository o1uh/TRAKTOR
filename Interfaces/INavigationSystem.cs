using Traktor.DataModels;      // Для Coordinates, FieldBoundaries, ObstacleData

namespace Traktor.Interfaces
{
    /// <summary>
    /// Определяет контракт для системы навигации трактора.
    /// </summary>
    public interface INavigationSystem
    {
        /// <summary>
        /// Получает текущее местоположение трактора от навигационной системы.
        /// </summary>
        /// <returns>Текущие координаты <see cref="Coordinates"/>.</returns>
        Coordinates GetPosition();

        /// <summary>
        /// Рассчитывает маршрут от текущего внутреннего положения системы до целевой точки.
        /// </summary>
        /// <param name="targetPosition">Конечная точка маршрута.</param>
        /// <param name="boundaries">Опциональные границы поля, в пределах которых должен быть построен маршрут.</param>
        /// <param name="precisionPoints">
        /// Количество промежуточных точек для генерации на прямолинейных сегментах маршрута.
        /// Увеличивает детализацию маршрута.
        /// </param>
        /// <returns>
        /// Список <see cref="Coordinates"/>, представляющий рассчитанный маршрут.
        /// Может вернуть пустой список или null, если маршрут не может быть построен.
        /// </returns>
        List<Coordinates> CalculateRoute(Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3);

        /// <summary>
        /// Корректирует существующий маршрут на основе информации об обнаруженных препятствиях.
        /// </summary>
        /// <param name="currentRoute">Текущий маршрут (список <see cref="Coordinates"/>) для корректировки.</param>
        /// <param name="detectedObstacles">Список обнаруженных препятствий <see cref="ObstacleData"/>.</param>
        /// <returns>
        /// Новый (скорректированный) список <see cref="Coordinates"/>.
        /// Если корректировка невозможна, может вернуть null или пустой список.
        /// Если корректировка не требуется, может вернуть копию <paramref name="currentRoute"/> или сам <paramref name="currentRoute"/>.
        /// </returns>
        List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles);

        /// <summary>
        /// Активирует навигационную систему.
        /// После вызова этого метода система готова к работе, но маршрут еще не рассчитан (если не использовалась перегрузка).
        /// </summary>
        void StartNavigation();

        /// <summary>
        /// Активирует навигационную систему и инициирует первоначальный расчет маршрута
        /// от текущего положения системы до указанной цели.
        /// </summary>
        /// <param name="initialTargetPosition">Конечная точка для первоначального расчета маршрута.</param>
        /// <param name="initialBoundaries">Опциональные границы поля для первоначального расчета.</param>
        /// <param name="initialPrecisionPoints">Точность детализации для первоначального расчета маршрута.</param>
        /// <returns>
        /// Первоначально рассчитанный маршрут (список <see cref="Coordinates"/>).
        /// Может вернуть пустой список или null, если запуск/расчет не удался.
        /// </returns>
        List<Coordinates> StartNavigation(Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3);

        /// <summary>
        /// Деактивирует навигационную систему (например, переводит модуль в "спящий режим" или отключает его).
        /// </summary>
        void StopNavigation();

        /// <summary>
        /// Вспомогательный метод для симуляции и тестирования:
        /// Позволяет извне установить или обновить текущую симулированную позицию навигационной системы.
        /// </summary>
        /// <param name="newPosition">Новые симулированные координаты <see cref="Coordinates"/>.</param>
        void UpdateSimulatedPosition(Coordinates newPosition);
    }
}