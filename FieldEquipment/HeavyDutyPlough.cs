using Traktor.Interfaces;   
using Traktor.Core;       

namespace Traktor.FieldEquipment
{
    /// <summary>
    /// ���������� ������� B2: ���� ��� ������� ������� (��������, ���������� �����).
    /// </summary>
    public class HeavyDutyPlough : ITillageToolProduct
    {
        private const string SourceFilePath = "FieldEquipment/HeavyDutyPlough.cs";
        private const string ToolType = "���� ��� ������� �������";

        public HeavyDutyPlough()
        {
            Logger.Instance.Info(SourceFilePath, $"������: {ToolType}.");
        }
        public void PrepareSoil()
        {
            Logger.Instance.Info(SourceFilePath, $"{ToolType}: �������� ������� � ��������� ������� ��� ���������� ��� ������� �����.");
        }
        public string GetToolType() => ToolType;
    }
}