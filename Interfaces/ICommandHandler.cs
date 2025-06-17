using Traktor.Interfaces; 
using Traktor.Core;     

namespace Traktor.Interfaces
{
    /// <summary>
    /// ��������� ��� ����������� � ������� ������������.
    /// ������������ ��������� ���� � ������������ ������� �������.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// ������������� ���������� ����������� � �������.
        /// </summary>
        /// <param name="nextHandler">��������� ����������.</param>
        void SetNext(ICommandHandler nextHandler);

        /// <summary>
        /// �������� ���������� ������ �����.
        /// </summary>
        /// <param name="userInput">������, ��������� �������������.</param>
        /// <param name="receiver">���������� ������ (��������, ControlUnit).</param>
        /// <returns>��������� ������� ICommand, ���� ���� ���������, ����� null.</returns>
        ICommand HandleRequest(string userInput, IControlUnitCommands receiver);
    }
}