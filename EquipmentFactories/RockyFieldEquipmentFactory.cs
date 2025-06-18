using Traktor.Interfaces;     
using Traktor.FieldEquipment; 
using Traktor.Core;           

namespace Traktor.EquipmentFactories
{
    /// <summary>
    /// Конкретная Фабрика 2: Создает семейство продуктов для каменистых полей.
    /// </summary>
    public class RockyFieldEquipmentFactory : IFieldEquipmentFactory
    {
        private const string SourceFilePath = "EquipmentFactories/RockyFieldEquipmentFactory.cs";

        public RockyFieldEquipmentFactory()
        {
            Logger.Instance.Info(SourceFilePath, "RockyFieldEquipmentFactory создана.");
        }

        public ISoilAnalyzerProduct CreateSoilAnalyzer()
        {
            Logger.Instance.Debug(SourceFilePath, "CreateSoilAnalyzer: Создание RockySoilAnalyzer...");
            return new RockySoilAnalyzer();
        }

        public ITillageToolProduct CreateTillageTool()
        {
            Logger.Instance.Debug(SourceFilePath, "CreateTillageTool: Создание HeavyDutyPlough...");
            return new HeavyDutyPlough();
        }
    }
}