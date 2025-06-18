using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.FieldEquipment
{
    /// <summary>
    /// ���������� ������� B1: ����������� ����.
    /// </summary>
    public class StandardPlough : ITillageToolProduct
    {
        private const string SourceFilePath = "FieldEquipment/StandardPlough.cs";
        private const string ToolType = "����������� ����";

        public StandardPlough()
        {
            Logger.Instance.Info(SourceFilePath, $"������: {ToolType}.");
        }

        public void PrepareSoil()
        {
            Logger.Instance.Info(SourceFilePath, $"{ToolType}: ������� ����������� ����� �� �������� �������.");
        }
        public string GetToolType() => ToolType;
    }
}