using Traktor.DataModels; 

namespace Traktor.Interfaces
{
    /// <summary>
    /// Интерфейс Приспособленца (Flyweight).
    /// Представляет тип объекта на поле, который может быть отображен или обработан.
    /// </summary>
    public interface IFieldObjectType
    {
        /// <summary>
        /// "Отображает" или "обрабатывает" объект на поле, используя внешнее состояние.
        /// </summary>
        /// <param name="position">Внешнее состояние: позиция объекта на поле.</param>
        /// <param name="uniqueId">Внешнее состояние: уникальный идентификатор конкретного экземпляра на поле.</param>
        void Display(Coordinates position, string uniqueId);

        string GetIntrinsicStateDescription(); 
    }
}