using Traktor.Interfaces; 
using Traktor.Core;       
using Traktor.DataModels; 

namespace Traktor.Flyweights
{
    /// <summary>
    /// ���������� �������������� (ConcreteFlyweight).
    /// ������ ���������� (�����������) ��������� - ��������, ��� ��������, ����, ������� ���.
    /// </summary>
    public class FieldObjectType : IFieldObjectType
    {
        private const string SourceFilePath = "Flyweights/FieldObjectType.cs";

        private readonly string _typeName;        
        private readonly string _textureName;     
        private readonly ConsoleColor _displayColor; 

        /// <summary>
        /// ����������� ������ ��������� ��� internal, ����� �������� ��� ������ ����� �������.
        /// ��� ������ ������� public, �� � ������������, �������������� �������� ������ ����������.
        /// </summary>
        public FieldObjectType(string typeName, string textureName, ConsoleColor displayColor)
        {
            _typeName = typeName;
            _textureName = textureName;
            _displayColor = displayColor;
            Logger.Instance.Info(SourceFilePath, $"������ ����� ��������� FieldObjectType (��������������): ���='{_typeName}', ��������='{_textureName}', ����='{_displayColor}'.");
        }

        /// <summary>
        /// "����������" ������, ��������� ���������� � ������� ���������.
        /// </summary>
        /// <param name="position">������� ���������: �������.</param>
        /// <param name="uniqueId">������� ���������: ���������� ID ���������� �� ����.</param>
        public void Display(Coordinates position, string uniqueId)
        {
            string logMessage = $"����������� (ID: {uniqueId}): ���='{_typeName}' (��������: '{_textureName}', ����: {_displayColor}) � ������� {position}.";
            Logger.Instance.Info(SourceFilePath, logMessage);
        }

        public string GetIntrinsicStateDescription()
        {
            return $"���='{_typeName}', ��������='{_textureName}', ����='{_displayColor}'";
        }
    }
}