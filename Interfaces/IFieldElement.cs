namespace Traktor.Interfaces
{
    /// <summary>
    /// Интерфейс для элементов, которые могут быть "посещены" Посетителем.
    /// </summary>
    public interface IFieldElement
    {
        /// <summary>
        /// Принимает Посетителя.
        /// </summary>
        /// <param name="visitor">Объект Посетителя.</param>
        void Accept(IVisitor visitor);

        /// <summary>
        /// Вспомогательный метод для получения описания элемента (для логирования).
        /// </summary>
        string GetDescription();
    }
}