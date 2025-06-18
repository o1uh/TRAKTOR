using Traktor.Interfaces;     
using Traktor.FieldEquipment; 
using Traktor.Core;           

namespace Traktor.EquipmentFactories
{
    /// <summary>
    /// ���������� ������� 2: ������� ��������� ��������� ��� ���������� �����.
    /// </summary>
    public class RockyFieldEquipmentFactory : IFieldEquipmentFactory
    {
        private const string SourceFilePath = "EquipmentFactories/RockyFieldEquipmentFactory.cs";

        public RockyFieldEquipmentFactory()
        {
            Logger.Instance.Info(SourceFilePath, "RockyFieldEquipmentFactory �������.");
        }

        public ISoilAnalyzerProduct CreateSoilAnalyzer()
        {
            Logger.Instance.Debug(SourceFilePath, "CreateSoilAnalyzer: �������� RockySoilAnalyzer...");
            return new RockySoilAnalyzer();
        }

        public ITillageToolProduct CreateTillageTool()
        {
            Logger.Instance.Debug(SourceFilePath, "CreateTillageTool: �������� HeavyDutyPlough...");
            return new HeavyDutyPlough();
        }
    }
}