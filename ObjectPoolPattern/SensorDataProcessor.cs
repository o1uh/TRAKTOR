using System; 
using Traktor.Core;

namespace Traktor.ObjectPoolPattern
{
    /// <summary>
    /// ���������������� ������: ���������� ������ ��������.
    /// ��������� ������������ ������, ������� ������� ����������������.
    /// </summary>
    public class SensorDataProcessor
    {
        private const string SourceFilePath = "ObjectPoolPattern/SensorDataProcessor.cs";
        public Guid Id { get; } 
        private bool _isInUse;

        public SensorDataProcessor()
        {
            Id = Guid.NewGuid();
            _isInUse = false; 
            Logger.Instance.Info(SourceFilePath, $"������ ����� SensorDataProcessor. ID: {Id}. �������� ������� �������������...");
            System.Threading.Thread.Sleep(100); 
        }

        /// <summary>
        /// ��������� ��������� ������.
        /// </summary>
        /// <param name="data">������ ��� ���������.</param>
        public void Process(string data)
        {
            if (_isInUse)
            {
                Logger.Instance.Warning(SourceFilePath, $"SensorDataProcessor (ID: {Id}): ������� ������������ ������, ������� ��� ������������!");
            }
            _isInUse = true;
            Logger.Instance.Info(SourceFilePath, $"SensorDataProcessor (ID: {Id}): ������ ��������� ������ '{data}'...");
            System.Threading.Thread.Sleep(50);
            Logger.Instance.Info(SourceFilePath, $"SensorDataProcessor (ID: {Id}): ��������� ������ '{data}' ���������.");
        }

        /// <summary>
        /// "����������" ��������� ������� ��� ���������� � ���������� �������������.
        /// ���������� ��� ����������� ������� � ���.
        /// </summary>
        public void Reset()
        {
            _isInUse = false;
            Logger.Instance.Info(SourceFilePath, $"SensorDataProcessor (ID: {Id}): ��������� ��������. ����� � ���������� �������������.");
        }

        public bool IsInUse() => _isInUse;
    }
}