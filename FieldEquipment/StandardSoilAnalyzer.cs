using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.FieldEquipment
{
    /// <summary>
    /// ���������� ������� A1: ����������� ���������� �����.
    /// </summary>
    public class StandardSoilAnalyzer : ISoilAnalyzerProduct
    {
        private const string SourceFilePath = "FieldEquipment/StandardSoilAnalyzer.cs";
        private const string AnalyzerType = "����������� ���������� �����";

        public StandardSoilAnalyzer()
        {
            Logger.Instance.Info(SourceFilePath, $"������: {AnalyzerType}.");
        }

        public void AnalyzeSoil()
        {
            Logger.Instance.Info(SourceFilePath, $"{AnalyzerType}: ������ ������������ ���� �����. �������� ���������, pH, ����������� �������...");
        }

        public string GetAnalyzerType() => AnalyzerType;
    }
}