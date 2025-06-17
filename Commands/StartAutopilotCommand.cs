using System; 
using Traktor.Core;       
using Traktor.Interfaces; 
using Traktor.DataModels; 
using Traktor.Implements; 

namespace Traktor.Commands
{
    /// <summary>
    /// ������� ��� ������� �������� ����������.
    /// </summary>
    public class StartAutopilotCommand : ICommand
    {
        private const string SourceFilePath = "Commands/StartAutopilotCommand.cs";
        private readonly IControlUnitCommands _receiver; // ���������� (Receiver)
        private readonly Coordinates _targetPosition;
        private readonly FieldBoundaries _fieldBoundaries; // ����� ���� null
        private readonly ImplementType _implementType;

        /// <summary>
        /// �������������� ����� ��������� ������� ������� ����������.
        /// </summary>
        /// <param name="controlUnit">��������� ControlUnit, ������� ����� ��������� ��������.</param>
        /// <param name="targetPosition">������� �������.</param>
        /// <param name="implementType">��� ������������� ������������.</param>
        /// <param name="fieldBoundaries">������� ���� (�����������).</param>
        public StartAutopilotCommand(IControlUnitCommands receiver, Coordinates targetPosition, ImplementType implementType, FieldBoundaries fieldBoundaries = null)
        {
            _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
            _targetPosition = targetPosition; // ������������, ��� Coordinates - ���������, �� ����� ���� null
            _implementType = implementType;
            _fieldBoundaries = fieldBoundaries; // ����� ���� null
            Logger.Instance.Debug(SourceFilePath, $"StartAutopilotCommand �������. ����: {_targetPosition}, ������������: {_implementType}, �������: {(_fieldBoundaries == null ? "�� ������" : "������")}.");
        }

        /// <summary>
        /// ��������� ������� ������� ����������.
        /// </summary>
        public void Execute()
        {
            Logger.Instance.Info(SourceFilePath, $"���������� StartAutopilotCommand: ������ ���������� � ���� {_targetPosition} � ������������� {_implementType}.");
            try
            {
                _receiver.StartOperation(_targetPosition, _fieldBoundaries, _implementType);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"������ ��� ���������� StartAutopilotCommand: {ex.Message}", ex);
            }
        }
    }
}