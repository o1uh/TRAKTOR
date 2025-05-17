namespace TractorAutopilot.DataModels
{
    /// <summary>
    /// �������� ���������� ������� ��������� ����.
    /// </summary>
    public class FieldAnalysis
    {
        /// <summary>
        /// �������� ��� ������ �������� ��������� �����.
        /// </summary>
        public string SoilCondition { get; set; } = "�����";

        /// <summary>
        /// �������� ��� ������ �������� �������� ��������.
        /// </summary>
        public string PlantHealth { get; set; } = "�������";

        // ����� �������� ������ ��������� �������

        /// <summary>
        /// ���������� ��������� ������������� ������� ����.
        /// </summary>
        /// <returns>������, ����������� ���������� �������.</returns>
        public override string ToString() => $"��������� �����: {SoilCondition}, �������� ��������: {PlantHealth}.";
    }
}