using Traktor.FieldElements;

namespace Traktor.Interfaces
{
    /// <summary>
    /// Интерфейс для Посетителя. Определяет операции для каждого типа Конкретного Элемента.
    /// </summary>
    public interface IVisitor
    {
        /// <summary>
        /// Посещает элемент типа ObstacleElement.
        /// </summary>
        /// <param name="obstacleElement">Посещаемый элемент препятствия.</param>
        void VisitObstacleElement(ObstacleElement obstacleElement);

        /// <summary>
        /// Посещает элемент типа SoilPatchElement.
        /// </summary>
        /// <param name="soilPatchElement">Посещаемый элемент участка почвы.</param>
        void VisitSoilPatchElement(SoilPatchElement soilPatchElement);
    }
}