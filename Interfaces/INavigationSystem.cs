using Traktor.DataModels;

namespace Traktor.Interfaces
{
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
        /// <param name="boundaries">Опциональные границы поля.</param>
        /// <param name="precisionPoints">Количество промежуточных точек для генерации на сегментах маршрута.</param>
        /// <returns>Список <see cref="Coordinates"/>, представляющий маршрут. Может вернуть пустой список или null, если маршрут не построен.</returns>
        List<Coordinates> CalculateRoute(Coordinates targetPosition, FieldBoundaries boundaries = null, int precisionPoints = 3);

        /// <summary>
        /// Корректирует существующий маршрут на основе информации об обнаруженных препятствиях.
        /// </summary>
        /// <param name="currentRoute">Текущий маршрут для корректировки.</param>
        /// <param name="detectedObstacles">Список обнаруженных препятствий.</param>
        /// <returns>
        /// Новый (скорректированный) список <see cref="Coordinates"/>.
        /// Если корректировка невозможна, возвращает null.
        /// Если корректировка не требуется, может вернуть копию currentRoute.
        /// </returns>
        List<Coordinates> AdjustRoute(List<Coordinates> currentRoute, List<ObstacleData> detectedObstacles);

        /// <summary>
        /// Активирует навигационную систему, выставляет ее в указанной начальной позиции
        /// и инициирует первоначальный расчет маршрута.
        /// </summary>
        /// <param name="systemInitialВыставкаPosition">Начальная позиция для выставки системы и первого расчета маршрута.</param>
        /// <param name="initialTargetPosition">Конечная точка для первого расчета маршрута.</param>
        /// <param name="initialBoundaries">Опциональные границы поля для первого расчета.</param>
        /// <param name="initialPrecisionPoints">Точность для первого расчета.</param>
        /// <returns>Первоначально рассчитанный маршрут или null/пустой список, если запуск/расчет не удался.</returns>
        List<Coordinates> StartNavigation(Coordinates systemInitialВыставкаPosition, Coordinates initialTargetPosition, FieldBoundaries initialBoundaries = null, int initialPrecisionPoints = 3);

        /// <summary>
        /// Деактивирует навигационную систему (переводит в "спящий режим" или выключает модуль).
        /// </summary>
        void StopNavigation();

        /// <summary>
        /// Вспомогательный метод для макета: позволяет извне установить текущую симулированную позицию.
        /// </summary>
        void UpdateSimulatedPosition(Coordinates newPosition);
    }
}