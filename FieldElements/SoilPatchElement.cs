using Traktor.Interfaces; 
using Traktor.Core;       
using Traktor.DataModels; 

namespace Traktor.FieldElements
{
    /// <summary>
    /// Конкретный элемент поля: Участок почвы.
    /// </summary>
    public class SoilPatchElement : IFieldElement
    {
        private const string SourceFilePath = "FieldElements/SoilPatchElement.cs";
        public double Moisture { get; private set; } // Влажность
        public string SoilType { get; private set; } // Тип почвы, например "Песчаная", "Глинистая"

        public SoilPatchElement(string soilType, double moisture)
        {
            SoilType = soilType ?? "Неизвестный тип почвы";
            Moisture = moisture;
            Logger.Instance.Debug(SourceFilePath, $"Создан SoilPatchElement: Тип='{SoilType}', Влажность={Moisture}%.");
        }

        public void Accept(IVisitor visitor)
        {
            Logger.Instance.Debug(SourceFilePath, $"SoilPatchElement '{SoilType}' вызывает Accept у Visitor '{visitor.GetType().Name}'.");
            visitor.VisitSoilPatchElement(this);
        }

        public string GetDescription()
        {
            return $"Участок почвы: Тип={SoilType}, Влажность={Moisture}%";
        }
    }
}