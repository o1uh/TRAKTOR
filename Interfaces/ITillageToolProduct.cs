namespace Traktor.Interfaces
{
    /// <summary>
    /// Абстрактный Продукт B: Интерфейс для инструмента обработки почвы.
    /// </summary>
    public interface ITillageToolProduct
    {
        void PrepareSoil();
        string GetToolType();
    }
}