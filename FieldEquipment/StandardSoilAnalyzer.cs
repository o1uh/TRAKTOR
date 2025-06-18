using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.FieldEquipment
{
    /// <summary>
    /// Конкретный Продукт A1: Стандартный анализатор почвы.
    /// </summary>
    public class StandardSoilAnalyzer : ISoilAnalyzerProduct
    {
        private const string SourceFilePath = "FieldEquipment/StandardSoilAnalyzer.cs";
        private const string AnalyzerType = "Стандартный анализатор почвы";

        public StandardSoilAnalyzer()
        {
            Logger.Instance.Info(SourceFilePath, $"Создан: {AnalyzerType}.");
        }

        public void AnalyzeSoil()
        {
            Logger.Instance.Info(SourceFilePath, $"{AnalyzerType}: Анализ стандартного типа почвы. Проверка влажности, pH, питательных веществ...");
        }

        public string GetAnalyzerType() => AnalyzerType;
    }
}