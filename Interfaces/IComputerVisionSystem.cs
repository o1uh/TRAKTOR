using Traktor.DataModels;

namespace Traktor.Interfaces
{
    public interface IComputerVisionSystem
    {
        /// <summary>
        /// ќбнаруживает физические преп€тстви€ на поле.
        /// —истема сама отвечает за получение данных со своих сенсоров.
        /// </summary>
        /// <param name="currentTractorPosition">“екущее положение трактора.</param>
        /// <returns>—писок обнаруженных преп€тствий <see cref="ObstacleData"/>.</returns>
        List<ObstacleData> DetectObstacles(Coordinates currentTractorPosition);

        /// <summary>
        /// јнализирует видимую зону пол€ дл€ обнаружени€ специфических особенностей (например, опасных сорн€ков).
        /// —истема сама отвечает за получение данных со своих сенсоров.
        /// </summary>
        /// <param name="currentTractorPosition">“екущее положение трактора (дл€ прив€зки найденных особенностей).</param>
        /// <returns>—писок обнаруженных особенностей пол€ <see cref="FieldFeatureData"/>.</returns>
        List<FieldFeatureData> AnalyzeFieldFeatures(Coordinates currentTractorPosition); // ”бран areaOfInterest
    }
}
