using Traktor.Interfaces; 
using Traktor.Core;       
using Traktor.DataModels; 

namespace Traktor.FieldElements
{
    /// <summary>
    /// Конкретный элемент поля: Препятствие.
    /// </summary>
    public class ObstacleElement : IFieldElement
    {
        private const string SourceFilePath = "FieldElements/ObstacleElement.cs";
        public string ObstacleType { get; private set; }
        public Coordinates Position { get; private set; }

        public ObstacleElement(string obstacleType, Coordinates position)
        {
            ObstacleType = obstacleType ?? "Неизвестное препятствие";
            Position = position;
            Logger.Instance.Debug(SourceFilePath, $"Создан ObstacleElement: Тип='{ObstacleType}', Позиция={Position}.");
        }

        public void Accept(IVisitor visitor)
        {
            Logger.Instance.Debug(SourceFilePath, $"ObstacleElement '{ObstacleType}' вызывает Accept у Visitor '{visitor.GetType().Name}'.");
            visitor.VisitObstacleElement(this);
            // visitor.Visit(this);
        }

        public string GetDescription()
        {
            return $"Препятствие: {ObstacleType} в {Position}";
        }
    }
}