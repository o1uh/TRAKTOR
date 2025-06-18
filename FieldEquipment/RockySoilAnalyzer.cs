using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.FieldEquipment
{
    /// <summary>
    /// ���������� ������� A2: ���������� ��� ���������� �����.
    /// </summary>
    public class RockySoilAnalyzer : ISoilAnalyzerProduct
    {
        private const string SourceFilePath = "FieldEquipment/RockySoilAnalyzer.cs";
        private const string AnalyzerType = "���������� ��� ���������� �����";

        public RockySoilAnalyzer()
        {
            Logger.Instance.Info(SourceFilePath, $"������: {AnalyzerType}.");
        }

        public void AnalyzeSoil()
        {
            Logger.Instance.Info(SourceFilePath, $"{AnalyzerType}: �������� ������ ���������� �����. ������ ���������, ���������� ������...");
        }

        public string GetAnalyzerType() => AnalyzerType;
    }
}