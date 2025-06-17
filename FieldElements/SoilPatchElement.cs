using Traktor.Interfaces; 
using Traktor.Core;       
using Traktor.DataModels; 

namespace Traktor.FieldElements
{
    /// <summary>
    /// ���������� ������� ����: ������� �����.
    /// </summary>
    public class SoilPatchElement : IFieldElement
    {
        private const string SourceFilePath = "FieldElements/SoilPatchElement.cs";
        public double Moisture { get; private set; } // ���������
        public string SoilType { get; private set; } // ��� �����, �������� "��������", "���������"

        public SoilPatchElement(string soilType, double moisture)
        {
            SoilType = soilType ?? "����������� ��� �����";
            Moisture = moisture;
            Logger.Instance.Debug(SourceFilePath, $"������ SoilPatchElement: ���='{SoilType}', ���������={Moisture}%.");
        }

        public void Accept(IVisitor visitor)
        {
            Logger.Instance.Debug(SourceFilePath, $"SoilPatchElement '{SoilType}' �������� Accept � Visitor '{visitor.GetType().Name}'.");
            visitor.VisitSoilPatchElement(this);
        }

        public string GetDescription()
        {
            return $"������� �����: ���={SoilType}, ���������={Moisture}%";
        }
    }
}