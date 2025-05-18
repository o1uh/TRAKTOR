using Traktor.Core;       // Для Logger
using Traktor.Interfaces; // Для ITaskComponent

namespace Traktor.TaskComponents
{
    /// <summary>
    /// Представляет составную (композитную) задачу, которая может содержать другие задачи.
    /// </summary>
    public class ComplexTask : ITaskComponent 
    {
        private readonly string _name;
        private readonly List<ITaskComponent> _subTasks = new List<ITaskComponent>();
        private const string SourceFilePath = "TaskComponents/ComplexTask.cs";

        public ComplexTask(string name)
        {
            _name = name ?? "Безымянная сложная задача";
            Logger.Instance.Info(SourceFilePath, $"ComplexTask '{_name}' создан.");
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
                Logger.Instance.Info(SourceFilePath, $"ComplexTask '{_name}': Добавлена подзадача '{task.GetName()}'.");
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, $"ComplexTask '{_name}': Попытка добавить null как подзадачу.");
            }
        }

        public void RemoveSubTask(ITaskComponent task)
        {
            if (task != null && _subTasks.Remove(task))
            {
                Logger.Instance.Info(SourceFilePath, $"ComplexTask '{_name}': Удалена подзадача '{task.GetName()}'.");
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, $"ComplexTask '{_name}': Попытка удалить подзадачу, которой нет в списке, или null.");
            }
        }

        public void Execute(string indent = "")
        {
            Logger.Instance.Info(SourceFilePath, $"{indent}Начало выполнения составной задачи: '{_name}' (содержит {_subTasks.Count} подзадач).");
            string subIndent = indent + "  ";
            foreach (var task in _subTasks)
            {
                task.Execute(subIndent);
            }
            Logger.Instance.Info(SourceFilePath, $"{indent}Завершение выполнения составной задачи: '{_name}'.");
        }
    }
}