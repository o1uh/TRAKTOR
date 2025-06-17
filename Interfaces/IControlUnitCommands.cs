using Traktor.DataModels; 
using Traktor.Implements; 

namespace Traktor.Interfaces
{
    /// <summary>
    /// ���������� �������, ������� ControlUnit ������ ����� ��������� 
    /// ��� ������������� � �������� Command.
    /// </summary>
    public interface IControlUnitCommands
    {
        /// <summary>
        /// ��������� �������� ����������.
        /// </summary>
        void StartOperation(Coordinates targetPosition, FieldBoundaries fieldBoundaries, ImplementType implementType);

        /// <summary>
        /// ������������� �������� ����������.
        /// </summary>
        void StopOperation();
    }
}