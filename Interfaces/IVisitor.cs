using Traktor.FieldElements;

namespace Traktor.Interfaces
{
    /// <summary>
    /// ��������� ��� ����������. ���������� �������� ��� ������� ���� ����������� ��������.
    /// </summary>
    public interface IVisitor
    {
        /// <summary>
        /// �������� ������� ���� ObstacleElement.
        /// </summary>
        /// <param name="obstacleElement">���������� ������� �����������.</param>
        void VisitObstacleElement(ObstacleElement obstacleElement);

        /// <summary>
        /// �������� ������� ���� SoilPatchElement.
        /// </summary>
        /// <param name="soilPatchElement">���������� ������� ������� �����.</param>
        void VisitSoilPatchElement(SoilPatchElement soilPatchElement);
    }
}