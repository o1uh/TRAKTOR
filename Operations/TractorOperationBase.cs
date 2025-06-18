using System; 
using Traktor.Interfaces; 
using Traktor.Core;     

namespace Traktor.Operations
{
    /// <summary>
    /// ���������� � �������� ����.
    /// ���������� ��������������� ��������� ��� �������� ��������
    /// � ������ ������ �� ������ ����������� (IOperationExecutor).
    /// </summary>
    public abstract class TractorOperationBase
    {
        protected readonly IOperationExecutor _executor; // ������ �� �����������
        protected readonly string _operationName;

        /// <summary>
        /// �������������� ����� ��������� ������� ��������.
        /// </summary>
        /// <param name="operationName">�������� ��������.</param>
        /// <param name="executor">����������, ������� ����� ��������� �������������� ����.</param>
        protected TractorOperationBase(string operationName, IOperationExecutor executor)
        {
            _operationName = operationName ?? "������������� ��������";
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            Logger.Instance.Info(GetSourceFilePath(), $"������� �������� '{_operationName}' � ������������ ���� '{_executor.GetType().Name}'.");
        }

        /// <summary>
        /// �����, ������� ������ ���� ���������� ������������ ��� �����������
        /// ������������� ������ ���������� �������� ����� ������ _executor.
        /// </summary>
        public abstract void ExecuteOperation();

        /// <summary>
        /// ��������������� �����, ����� ���������� ����� ������� ���� SourceFilePath ��� �������.
        /// </summary>
        protected abstract string GetSourceFilePath();
    }
}