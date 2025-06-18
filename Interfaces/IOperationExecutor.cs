namespace Traktor.Interfaces
{
    /// <summary>
    /// ��������� ����������� (Implementor) ��� �������� ����.
    /// ���������� �������������� �������� ��� ���������� ����� �����-���� ��������.
    /// </summary>
    public interface IOperationExecutor
    {
        /// <summary>
        /// �������� ���������� ���� ��������.
        /// </summary>
        /// <param name="operationName">�������� ��������.</param>
        /// <param name="stepDetails">������ �������� ����.</param>
        void StartStep(string operationName, string stepDetails);

        /// <summary>
        /// "������������" ������� ���.
        /// </summary>
        /// <param name="operationName">�������� ��������.</param>
        void ProcessStep(string operationName);

        /// <summary>
        /// ��������� ���������� ���� ��������.
        /// </summary>
        /// <param name="operationName">�������� ��������.</param>
        /// <param name="stepDetails">������ ������������ ����.</param>
        /// <returns>��������� ���������� ���� (��� ������ - ������).</returns>
        string FinishStep(string operationName, string stepDetails);
    }
}