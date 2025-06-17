using Traktor.Interfaces; 
using Traktor.Core;       
using Traktor.DataModels; 

namespace Traktor.FieldElements
{
    /// <summary>
    /// ���������� ������� ����: �����������.
    /// </summary>
    public class ObstacleElement : IFieldElement
    {
        private const string SourceFilePath = "FieldElements/ObstacleElement.cs";
        public string ObstacleType { get; private set; }
        public Coordinates Position { get; private set; }

        public ObstacleElement(string obstacleType, Coordinates position)
        {
            ObstacleType = obstacleType ?? "����������� �����������";
            Position = position;
            Logger.Instance.Debug(SourceFilePath, $"������ ObstacleElement: ���='{ObstacleType}', �������={Position}.");
        }

        public void Accept(IVisitor visitor)
        {
            Logger.Instance.Debug(SourceFilePath, $"ObstacleElement '{ObstacleType}' �������� Accept � Visitor '{visitor.GetType().Name}'.");
            visitor.VisitObstacleElement(this);
            // visitor.Visit(this);
        }

        public string GetDescription()
        {
            return $"�����������: {ObstacleType} � {Position}";
        }
    }
}