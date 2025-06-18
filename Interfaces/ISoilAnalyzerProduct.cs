namespace Traktor.Interfaces
{
    /// <summary>
    /// Абстрактный Продукт A: Интерфейс для анализатора почвы.
    /// </summary>
    public interface ISoilAnalyzerProduct
    {
        void AnalyzeSoil();
        string GetAnalyzerType();
    }
}