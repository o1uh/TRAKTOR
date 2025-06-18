using Traktor.Interfaces;     
using Traktor.FieldEquipment; 
using Traktor.Core;           

namespace Traktor.EquipmentFactories
{
    /// <summary>
    /// Конкретная Фабрика 1: Создает семейство продуктов для стандартных полей.
    /// </summary>
    public class StandardFieldEquipmentFactory : IFieldEquipmentFactory
    {
        private const string SourceFilePath = "EquipmentFactories/StandardFieldEquipmentFactory.cs";

        public StandardFieldEquipmentFactory()
        {
            Logger.Instance.Info(SourceFilePath, "StandardFieldEquipmentFactory создана.");
        }

        public ISoilAnalyzerProduct CreateSoilAnalyzer()
        {
            Logger.Instance.Debug(SourceFilePath, "CreateSoilAnalyzer: Создание StandardSoilAnalyzer...");
            return new StandardSoilAnalyzer();
        }

        public ITillageToolProduct CreateTillageTool()
        {
            Logger.Instance.Debug(SourceFilePath, "CreateTillageTool: Создание StandardPlough...");
            return new StandardPlough();
        }
    }
}