using System.Collections.Generic; 
using Traktor.Core;             

namespace Traktor.MementoPattern
{
    /// <summary>
    /// Опекун в паттерне Memento.
    /// Отвечает за хранение Снимков, не изменяя и не анализируя их.
    /// </summary>
    public class History
    {
        private const string SourceFilePath = "MementoPattern/History.cs";
        private readonly Stack<object> _mementos = new Stack<object>();
        public History()
        {
            Logger.Instance.Info(SourceFilePath, "History (Caretaker) создан. Готов к сохранению снимков.");
        }

        /// <summary>
        /// Добавляет снимок в историю.
        /// </summary>
        /// <param name="memento">Объект снимка (возвращенный Originator.CreateMemento()).</param>
        public void PushMemento(object memento)
        {
            if (memento != null)
            {
                _mementos.Push(memento);
                Logger.Instance.Info(SourceFilePath, $"В History добавлен новый Memento. Всего снимков: {_mementos.Count}.");
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "Попытка добавить null Memento в History.");
            }
        }

        /// <summary>
        /// Извлекает (и удаляет) последний сохраненный снимок из истории.
        /// </summary>
        /// <returns>Последний сохраненный объект Memento или null, если история пуста.</returns>
        public object PopMemento()
        {
            if (_mementos.Count > 0)
            {
                object memento = _mementos.Pop();
                Logger.Instance.Info(SourceFilePath, $"Из History извлечен Memento. Осталось снимков: {_mementos.Count}.");
                return memento;
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "Попытка извлечь Memento из пустой History. Возвращен null.");
                return null;
            }
        }

        /// <summary>
        /// Проверяет, пуста ли история снимков.
        /// </summary>
        public bool IsEmpty => _mementos.Count == 0;
    }
}