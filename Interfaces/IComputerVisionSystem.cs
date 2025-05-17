using Traktor.DataModels;

namespace Traktor.Interfaces
{
    public interface IComputerVisionSystem
    {
        /// <summary>
        /// Обнаруживает препятствия на поле.
        /// </summary>
        /// <param name="sensorInput">Опциональные "сырые" данные с сенсора.</param>
        /// <returns>
        /// Список <see cref="ObstacleData"/> обнаруженных препятствий.
        /// Пустой список, если препятствий нет.
        /// </returns>
        List<ObstacleData> DetectObstacles(object sensorInput = null);

        /// <summary>
        /// Анализирует состояние выбранных точек/участков поля.
        /// </summary>
        /// <param name="targetCoordinates">Список координат точек/участков для анализа. Если null или пуст, может анализировать текущую зону видимости.</param>
        /// <param name="sensorInput">Опциональные "сырые" данные с сенсора.</param>
        /// <returns>
        /// Список <see cref="FieldPointAnalysis"/> с результатами анализа для каждой запрошенной точки/участка.
        /// Может вернуть пустой список, если анализ не дал результатов.
        /// </returns>
        List<FieldPointAnalysis> AnalyzeFieldPoints(List<Coordinates> targetCoordinates = null, object sensorInput = null);
    }
}
