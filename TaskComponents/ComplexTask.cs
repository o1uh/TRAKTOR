using Traktor.Core;       // ��� Logger
using Traktor.Interfaces; // ��� ITaskComponent

namespace Traktor.TaskComponents
{
    /// <summary>
    /// ������������ ��������� (�����������) ������, ������� ����� ��������� ������ ������.
    /// </summary>
    public class ComplexTask : ITaskComponent 
    {
        private readonly string _name;
        private readonly List<ITaskComponent> _subTasks = new List<ITaskComponent>();
        private const string SourceFilePath = "TaskComponents/ComplexTask.cs";

        public ComplexTask(string name)
        {
            _name = name ?? "���������� ������� ������";
            Logger.Instance.Info(SourceFilePath, $"ComplexTask '{_name}' ������.");
        }

        public string GetName()
        {
            return _name;
        }

        public void AddSubTask(ITaskComponent task)
        {
            if (task != null)
            {
                _subTasks.Add(task);
                Logger.Instance.Info(SourceFilePath, $"ComplexTask '{_name}': ��������� ��������� '{task.GetName()}'.");
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, $"ComplexTask '{_name}': ������� �������� null ��� ���������.");
            }
        }

        public void RemoveSubTask(ITaskComponent task)
        {
            if (task != null && _subTasks.Remove(task))
            {
                Logger.Instance.Info(SourceFilePath, $"ComplexTask '{_name}': ������� ��������� '{task.GetName()}'.");
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, $"ComplexTask '{_name}': ������� ������� ���������, ������� ��� � ������, ��� null.");
            }
        }

        public void Execute(string indent = "")
        {
            Logger.Instance.Info(SourceFilePath, $"{indent}������ ���������� ��������� ������: '{_name}' (�������� {_subTasks.Count} ��������).");
            string subIndent = indent + "  ";
            foreach (var task in _subTasks)
            {
                task.Execute(subIndent);
            }
            Logger.Instance.Info(SourceFilePath, $"{indent}���������� ���������� ��������� ������: '{_name}'.");
        }
    }
}