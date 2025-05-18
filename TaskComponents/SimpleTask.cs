using Traktor.Core;       // ��� Logger
using Traktor.Interfaces; // ��� ITaskComponent

namespace Traktor.TaskComponents 
{
    /// <summary>
    /// ������������ ������� (��������) ������, ������� �� ����� ��������� ��������.
    /// </summary>
    public class SimpleTask : ITaskComponent 
    {
        private readonly string _name;
        private const string SourceFilePath = "TaskComponents/SimpleTask.cs";

        /// <summary>
        /// �������������� ����� ��������� ������� ������.
        /// </summary>
        /// <param name="name">��� ������.</param>
        public SimpleTask(string name)
        {
            _name = name ?? "���������� ������� ������";
            Logger.Instance.Info(SourceFilePath, $"SimpleTask '{_name}' ������.");
        }

        public string GetName()
        {
            return _name;
        }

        public void Execute(string indent = "")
        {
            Logger.Instance.Info(SourceFilePath, $"{indent}���������� ������� ������: '{_name}'.");
        }
    }
}