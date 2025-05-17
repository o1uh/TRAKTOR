using Traktor.DataModels;           // Для Coordinates и FieldBoundaries

namespace Traktor.Interfaces
{
    public interface INavigationSystem
    {
        /// <summary>
        /// Получает текущие координаты трактора.
        /// </summary>
        /// <returns>Структура <see cref="Coordinates"/> с текущими координатами.</returns>
        Coordinates GetPosition();

        /// <summary>
        /// Рассчитывает маршрут как последовательность точек (координат).
        /// </summary>
        /// <param name="startPosition">Начальная точка маршрута (текущие координаты трактора).</param>
        /// <param name="targetPosition">Конечная точка маршрута.</param>
        /// <param name="boundaries">Опционально, границы поля, в пределах которых должен быть построен маршрут.</param>
        /// <returns>Список <see cref="Coordinates"/>, представляющий маршрут, или null/пустой список, если маршрут не может быть построен.</returns>
        List<Coordinates> CalculateRoute(Coordinates startPosition, Coordinates targetPosition, FieldBoundaries boundaries = null);

        /// <summary>
        /// Корректирует существующий маршрут на основе информации об обнаруженных препятствиях.
        /// Возвращает новый скорректированный маршрут.
        /// </summary>
        /// <param name="currentRoute">Текущий маршрут, который нужно скорректировать.</param>
        /// <param name="detectedObstacles">Список обнаруженных препятствий.</param>
        /// <returns>Новый список <see cref="Coordinates"/>, представляющий скорректированный маршрут, или null/пустой список, если корректировка не удалась или не требуется.</returns>
        List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles);

        /// <summary>
        /// Останавливает процесс навигации.
        /// </summary>
        void StopNavigation();
    }
}