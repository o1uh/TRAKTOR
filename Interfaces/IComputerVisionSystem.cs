// Interfaces/IComputerVisionSystem.cs
namespace TractorAutopilot.Interfaces
{
    public interface IComputerVisionSystem
    {
        /// <summary>
        /// Обнаруживает препятствия в поле зрения.
        /// </summary>
        /// <returns>Массив координат препятствий.</returns>
        Obstacle[] DetectObstacles();

        /// <summary>
        /// Анализирует состояние поля (растения, почва и т.д.).
        /// </summary>
        /// <returns>Информация о состоянии поля.</returns>
        FieldAnalysisData AnalyzeField();
    }

    // Вспомогательные структуры для представления данных
    public struct Obstacle
    {
        public double X;
        public double Y;
        public double Z;
    }

    public struct FieldAnalysisData
    {
        public double AveragePlantHeight;
        public double SoilMoisture;
        // Другие параметры анализа поля
    }
}