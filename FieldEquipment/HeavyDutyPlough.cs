using Traktor.Interfaces;   
using Traktor.Core;       

namespace Traktor.FieldEquipment
{
    /// <summary>
    /// Конкретный Продукт B2: Плуг для тяжелых условий (например, каменистой почвы).
    /// </summary>
    public class HeavyDutyPlough : ITillageToolProduct
    {
        private const string SourceFilePath = "FieldEquipment/HeavyDutyPlough.cs";
        private const string ToolType = "Плуг для тяжелых условий";

        public HeavyDutyPlough()
        {
            Logger.Instance.Info(SourceFilePath, $"Создан: {ToolType}.");
        }
        public void PrepareSoil()
        {
            Logger.Instance.Info(SourceFilePath, $"{ToolType}: Глубокая вспашка с усиленным лемехом для каменистой или плотной почвы.");
        }
        public string GetToolType() => ToolType;
    }
}