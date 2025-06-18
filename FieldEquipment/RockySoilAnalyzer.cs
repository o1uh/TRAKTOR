using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.FieldEquipment
{
    /// <summary>
    /// Конкретный Продукт A2: Анализатор для каменистой почвы.
    /// </summary>
    public class RockySoilAnalyzer : ISoilAnalyzerProduct
    {
        private const string SourceFilePath = "FieldEquipment/RockySoilAnalyzer.cs";
        private const string AnalyzerType = "Анализатор для каменистой почвы";

        public RockySoilAnalyzer()
        {
            Logger.Instance.Info(SourceFilePath, $"Создан: {AnalyzerType}.");
        }

        public void AnalyzeSoil()
        {
            Logger.Instance.Info(SourceFilePath, $"{AnalyzerType}: Глубокий анализ каменистой почвы. Оценка плотности, содержания камней...");
        }

        public string GetAnalyzerType() => AnalyzerType;
    }
}