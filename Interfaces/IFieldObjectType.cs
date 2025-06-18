using Traktor.DataModels; 

namespace Traktor.Interfaces
{
    /// <summary>
    /// ��������� �������������� (Flyweight).
    /// ������������ ��� ������� �� ����, ������� ����� ���� ��������� ��� ���������.
    /// </summary>
    public interface IFieldObjectType
    {
        /// <summary>
        /// "����������" ��� "������������" ������ �� ����, ��������� ������� ���������.
        /// </summary>
        /// <param name="position">������� ���������: ������� ������� �� ����.</param>
        /// <param name="uniqueId">������� ���������: ���������� ������������� ����������� ���������� �� ����.</param>
        void Display(Coordinates position, string uniqueId);

        string GetIntrinsicStateDescription(); 
    }
}