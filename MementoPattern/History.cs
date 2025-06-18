using System.Collections.Generic; 
using Traktor.Core;             

namespace Traktor.MementoPattern
{
    /// <summary>
    /// ������ � �������� Memento.
    /// �������� �� �������� �������, �� ������� � �� ���������� ��.
    /// </summary>
    public class History
    {
        private const string SourceFilePath = "MementoPattern/History.cs";
        private readonly Stack<object> _mementos = new Stack<object>();
        public History()
        {
            Logger.Instance.Info(SourceFilePath, "History (Caretaker) ������. ����� � ���������� �������.");
        }

        /// <summary>
        /// ��������� ������ � �������.
        /// </summary>
        /// <param name="memento">������ ������ (������������ Originator.CreateMemento()).</param>
        public void PushMemento(object memento)
        {
            if (memento != null)
            {
                _mementos.Push(memento);
                Logger.Instance.Info(SourceFilePath, $"� History �������� ����� Memento. ����� �������: {_mementos.Count}.");
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "������� �������� null Memento � History.");
            }
        }

        /// <summary>
        /// ��������� (� �������) ��������� ����������� ������ �� �������.
        /// </summary>
        /// <returns>��������� ����������� ������ Memento ��� null, ���� ������� �����.</returns>
        public object PopMemento()
        {
            if (_mementos.Count > 0)
            {
                object memento = _mementos.Pop();
                Logger.Instance.Info(SourceFilePath, $"�� History �������� Memento. �������� �������: {_mementos.Count}.");
                return memento;
            }
            else
            {
                Logger.Instance.Warning(SourceFilePath, "������� ������� Memento �� ������ History. ��������� null.");
                return null;
            }
        }

        /// <summary>
        /// ���������, ����� �� ������� �������.
        /// </summary>
        public bool IsEmpty => _mementos.Count == 0;
    }
}