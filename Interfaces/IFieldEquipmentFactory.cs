namespace Traktor.Interfaces
{
    /// <summary>
    /// Интерфейс Абстрактной Фабрики (AbstractFactory).
    /// Объявляет операции для создания абстрактных продуктов 
    /// (анализатора почвы и инструмента для обработки почвы).
    /// </summary>
    public interface IFieldEquipmentFactory
    {
        /// <summary>
        /// Создает экземпляр анализатора почвы.
        /// </summary>
        /// <returns>Экземпляр, реализующий ISoilAnalyzerProduct.</returns>
        ISoilAnalyzerProduct CreateSoilAnalyzer();

        /// <summary>
        /// Создает экземпляр инструмента для обработки почвы.
        /// </summary>
        /// <returns>Экземпляр, реализующий ITillageToolProduct.</returns>
        ITillageToolProduct CreateTillageTool();
    }
}