namespace Traktor.Interfaces
{
    /// <summary>
    /// ����������� ������� A: ��������� ��� ����������� �����.
    /// </summary>
    public interface ISoilAnalyzerProduct
    {
        void AnalyzeSoil();
        string GetAnalyzerType();
    }
}