namespace Traktor.Interfaces
{
    /// <summary>
    /// ��������� ����������� ������� (AbstractFactory).
    /// ��������� �������� ��� �������� ����������� ��������� 
    /// (����������� ����� � ����������� ��� ��������� �����).
    /// </summary>
    public interface IFieldEquipmentFactory
    {
        /// <summary>
        /// ������� ��������� ����������� �����.
        /// </summary>
        /// <returns>���������, ����������� ISoilAnalyzerProduct.</returns>
        ISoilAnalyzerProduct CreateSoilAnalyzer();

        /// <summary>
        /// ������� ��������� ����������� ��� ��������� �����.
        /// </summary>
        /// <returns>���������, ����������� ITillageToolProduct.</returns>
        ITillageToolProduct CreateTillageTool();
    }
}