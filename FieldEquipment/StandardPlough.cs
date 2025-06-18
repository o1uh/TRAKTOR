using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.FieldEquipment
{
    /// <summary>
    /// Конкретный Продукт B1: Стандартный плуг.
    /// </summary>
    public class StandardPlough : ITillageToolProduct
    {
        private const string SourceFilePath = "FieldEquipment/StandardPlough.cs";
        private const string ToolType = "Стандартный плуг";

        public StandardPlough()
        {
            Logger.Instance.Info(SourceFilePath, $"Создан: {ToolType}.");
        }

        public void PrepareSoil()
        {
            Logger.Instance.Info(SourceFilePath, $"{ToolType}: Вспашка стандартной почвы на заданную глубину.");
        }
        public string GetToolType() => ToolType;
    }
}