using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.Commands
{
    /// <summary>
    /// ����������� ������� ����� ��� ������������ ������ � ������� ������������.
    /// ������������� ���������� ��� ��������� ���������� ����������� � ������������� �������.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        protected ICommandHandler _nextHandler;

        public void SetNext(ICommandHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        /// <summary>
        /// ������������ ������ ��� �������� ��� ���������� ����������� � �������.
        /// </summary>
        /// <param name="userInput">������, ��������� �������������.</param>
        /// <param name="receiver">���������� ������.</param>
        /// <returns>��������� ������� ICommand, ���� ���� ���������, ����� null.</returns>
        public abstract ICommand HandleRequest(string userInput, IControlUnitCommands receiver);

        /// <summary>
        /// ��������������� ����� ��� �������� ������� ���������� �����������, ���� ������� �� ���� ��� ����������.
        /// </summary>
        protected ICommand PassToNext(string userInput, IControlUnitCommands receiver, string currentHandlerName)
        {
            if (_nextHandler != null)
            {
                Logger.Instance.Debug(currentHandlerName, $"�� ���� ���������� '{userInput}'. �������� ����������: {_nextHandler.GetType().Name}.");
                return _nextHandler.HandleRequest(userInput, receiver);
            }
            else
            {
                Logger.Instance.Debug(currentHandlerName, $"�� ���� ���������� '{userInput}'. ���������� ����������� ���.");
                return null; // ����� �������, ������� �� ����������
            }
        }
    }
}