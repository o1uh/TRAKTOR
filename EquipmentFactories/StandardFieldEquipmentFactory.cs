using Traktor.Interfaces;     
using Traktor.FieldEquipment; 
using Traktor.Core;           

namespace Traktor.EquipmentFactories
{
    /// <summary>
    /// ���������� ������� 1: ������� ��������� ��������� ��� ����������� �����.
    /// </summary>
    public class StandardFieldEquipmentFactory : IFieldEquipmentFactory
    {
        private const string SourceFilePath = "EquipmentFactories/StandardFieldEquipmentFactory.cs";

        public StandardFieldEquipmentFactory()
        {
            Logger.Instance.Info(SourceFilePath, "StandardFieldEquipmentFactory �������.");
        }

        public ISoilAnalyzerProduct CreateSoilAnalyzer()
        {
            Logger.Instance.Debug(SourceFilePath, "CreateSoilAnalyzer: �������� StandardSoilAnalyzer...");
            return new StandardSoilAnalyzer();
        }

        public ITillageToolProduct CreateTillageTool()
        {
            Logger.Instance.Debug(SourceFilePath, "CreateTillageTool: �������� StandardPlough...");
            return new StandardPlough();
        }
    }
}