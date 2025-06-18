using System;
using Traktor.Core;       
using Traktor.Mocks;      
using Traktor.DataModels; 
using Traktor.Implements; 

namespace Traktor.Facades
{
    /// <summary>
    /// �����, ��������������� ���������� ��������� ��� ���������� ��������� ���������� ���������� ��������.
    /// �������� ��������� �������������� � ControlUnit � ��� ������������.
    /// </summary>
    public class TractorAutopilotFacade
    {
        private const string SourceFilePath = "Facades/TractorAutopilotFacade.cs";
        private readonly MockControlUnit _mockControlUnit; 

        /// <summary>
        /// �������������� ����� ��������� ������.
        /// </summary>
        /// <param name="controlUnit">��������� MockControlUnit (��� ControlUnit), ������� ����� ��������� �����.</param>
        public TractorAutopilotFacade(MockControlUnit controlUnit)
        {
            _mockControlUnit = controlUnit ?? throw new ArgumentNullException(nameof(controlUnit));
            Logger.Instance.Info(SourceFilePath, $"TractorAutopilotFacade ������. ����������: {_mockControlUnit.GetType().FullName}.");
        }

        /// <summary>
        /// ���������� ����� ��� ������� ������ �������� ��������� ����.
        /// </summary>
        /// <param name="fieldId">������������� ���� (��� �������, �� ������������ � MockControlUnit).</param>
        /// <param name="startLatitude">��������� ������ ��� ��������.</param>
        /// <param name="startLongitude">��������� ������� ��� ��������.</param>
        /// <param name="implement">��� ��������� ������������.</param>
        /// <returns>True, ���� ������� �� ������ ���� ������� ����������, ����� false.</returns>
        public bool StartFullFieldOperation(string fieldId, double startLatitude, double startLongitude, ImplementType implement)
        {
            Logger.Instance.Info(SourceFilePath, $"Facade: ������ �� ������ ��������� ���� '{fieldId}' � ������������� '{implement}' �� ����� ({startLatitude}, {startLongitude}).");

            var target = new Coordinates(startLatitude, startLongitude);
            
            Logger.Instance.Debug(SourceFilePath, "Facade: ������������� ������� �� ������ � MockControlUnit...");
            _mockControlUnit.RequestStart(target, implement, null);

            Logger.Instance.Info(SourceFilePath, $"Facade: ������� �� ������ ������ ��������� ���� '{fieldId}' ����������.");
            return true; 
        }

        /// <summary>
        /// ���������� ����� ��� ��������� ���� ��������.
        /// </summary>
        public void StopAllOperations()
        {
            Logger.Instance.Info(SourceFilePath, "Facade: ������ �� ��������� ���� ��������.");
            Logger.Instance.Debug(SourceFilePath, "Facade: ������������� ������� �� ��������� � MockControlUnit...");
            _mockControlUnit.RequestStop();
            Logger.Instance.Info(SourceFilePath, "Facade: ������� �� ��������� ���� �������� ����������.");
        }

        /// <summary>
        /// ���������� ����� ��� ��������� �������� �������.
        /// </summary>
        /// <returns>������ � ����� ������� ��������.</returns>
        public string GetQuickStatus()
        {
            string currentStatus = "������ �� �������� ����� MockControlUnit � ������ (������� ��������� MockControlUnit ��� ����������)";

            Logger.Instance.Info(SourceFilePath, $"Facade: ������ �� ������� ������. ����������: '{currentStatus}'.");
            return currentStatus;
        }
    }
}