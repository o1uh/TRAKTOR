using System.Collections.Generic; // ��� Dictionary
using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.Flyweights
{
    /// <summary>
    /// ������� ��������������� (FlyweightFactory).
    /// ������� � ��������� ����� �������� FieldObjectType.
    /// </summary>
    public class FieldObjectTypeFactory
    {
        private const string SourceFilePath = "Flyweights/FieldObjectTypeFactory.cs";
        private readonly Dictionary<string, IFieldObjectType> _flyweights = new Dictionary<string, IFieldObjectType>();

        public FieldObjectTypeFactory()
        {
            Logger.Instance.Info(SourceFilePath, "FieldObjectTypeFactory �������.");
        }

        /// <summary>
        /// �������� ��� ������� �������������� (FieldObjectType) � ���������� ����������� �����������.
        /// </summary>
        /// <param name="typeName">���� ��� ������������� ���� (��������, "������", "�������").</param>
        /// <param name="textureName">��� �������� ��� ����� ����.</param>
        /// <param name="displayColor">���� ��� ����������� ����� ����.</param>
        /// <returns>��������� IFieldObjectType (��������, �����������).</returns>
        public IFieldObjectType GetFlyweight(string typeName, string textureName, ConsoleColor displayColor)
        {
            string key = typeName;

            if (_flyweights.TryGetValue(key, out IFieldObjectType flyweight))
            {
                Logger.Instance.Info(SourceFilePath, $"��������� ������������ �������������� ��� ����� '{key}'. ���������� ���������: {flyweight.GetIntrinsicStateDescription()}");
            }
            else
            {
                Logger.Instance.Info(SourceFilePath, $"�������������� ��� ����� '{key}' �� ������. �������� ������...");
                flyweight = new FieldObjectType(typeName, textureName, displayColor);
                _flyweights.Add(key, flyweight);
                Logger.Instance.Info(SourceFilePath, $"����� �������������� ��� ����� '{key}' �������� � ���. ����� � ����: {_flyweights.Count}.");
            }
            return flyweight;
        }

        /// <summary>
        /// ���������� ���������� ���������� ��������������� (����� ��������) � ����.
        /// </summary>
        public int GetFlyweightsCount()
        {
            int count = _flyweights.Count;
            Logger.Instance.Debug(SourceFilePath, $"������� ���������� ���������� ��������������� � ����: {count}.");
            return count;
        }
    }
}