using Traktor.Core;       // Для Logger
using Traktor.Interfaces; // Для ITaskComponent

namespace Traktor.TaskComponents 
{
    /// <summary>
    /// Представляет простую (листовую) задачу, которая не может содержать подзадач.
    /// </summary>
    public class SimpleTask : ITaskComponent 
    {
        private readonly string _name;
        private const string SourceFilePath = "TaskComponents/SimpleTask.cs";

        /// <summary>
        /// Инициализирует новый экземпляр простой задачи.
        /// </summary>
        /// <param name="name">Имя задачи.</param>
        public SimpleTask(string name)
        {
            _name = name ?? "Безымянная простая задача";
            Logger.Instance.Info(SourceFilePath, $"SimpleTask '{_name}' создан.");
        }

        public string GetName()
        {
            return _name;
        }

        public void Execute(string indent = "")
        {
            Logger.Instance.Info(SourceFilePath, $"{indent}Выполнение простой задачи: '{_name}'.");
        }
    }
}