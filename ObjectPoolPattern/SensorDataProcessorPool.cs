using System.Collections.Generic;
using Traktor.Core;             

namespace Traktor.ObjectPoolPattern
{
    /// <summary>
    /// ��� �������� ��� SensorDataProcessor.
    /// ��������� ���������, ��������� � ������� ���������������� ��������.
    /// </summary>
    public class SensorDataProcessorPool
    {
        private const string SourceFilePath = "ObjectPoolPattern/SensorDataProcessorPool.cs";
        private readonly List<SensorDataProcessor> _availableProcessors = new List<SensorDataProcessor>();
        private readonly List<SensorDataProcessor> _inUseProcessors = new List<SensorDataProcessor>();
        private readonly int _maxPoolSize;
        private readonly object _lock = new object(); 

        /// <summary>
        /// �������������� ����� ��������� ���� ��������.
        /// </summary>
        /// <param name="initialSize">��������� ���������� �������� � ����.</param>
        /// <param name="maxSize">������������ ������ ����. ���� 0 ��� ������, ������ �� ��������� (�� �� �������������).</param>
        public SensorDataProcessorPool(int initialSize = 2, int maxSize = 5)
        {
            _maxPoolSize = maxSize > 0 ? maxSize : int.MaxValue; 

            Logger.Instance.Info(SourceFilePath, $"SensorDataProcessorPool ������. ��������� ������: {initialSize}, ������������ ������: {(maxSize > 0 ? maxSize.ToString() : "�� ���������")}.");

            for (int i = 0; i < initialSize; i++)
            {
                if (_availableProcessors.Count + _inUseProcessors.Count < _maxPoolSize)
                {
                    _availableProcessors.Add(new SensorDataProcessor());
                }
                else break;
            }
            Logger.Instance.Info(SourceFilePath, $"��� ���������������. �������� �����������: {_availableProcessors.Count}.");
        }

        /// <summary>
        /// �������� ��������� SensorDataProcessor �� ����.
        /// ���� � ���� ��� ��������� �������� � ������ ���� �� ������ ���������, ������� �����.
        /// ���� ��� ���� � �������� ���������, ����� ������� null ��� ������� (� ������ ������ null).
        /// </summary>
        /// <returns>��������� SensorDataProcessor ��� null, ���� �������� �� �������.</returns>
        public SensorDataProcessor AcquireProcessor()
        {
            lock (_lock) 
            {
                if (_availableProcessors.Count > 0)
                {
                    SensorDataProcessor processor = _availableProcessors[0];
                    _availableProcessors.RemoveAt(0);
                    _inUseProcessors.Add(processor);
                    Logger.Instance.Info(SourceFilePath, $"��������� (ID: {processor.Id}) ���� �� ����. ��������: {_availableProcessors.Count}, ������������: {_inUseProcessors.Count}.");
                    return processor;
                }
                else if (_inUseProcessors.Count < _maxPoolSize)
                {
                    Logger.Instance.Info(SourceFilePath, "� ���� ��� ��������� �����������. ������� ������� ����� (����� �� ���������)...");
                    SensorDataProcessor newProcessor = new SensorDataProcessor();
                    _inUseProcessors.Add(newProcessor);
                    Logger.Instance.Info(SourceFilePath, $"����� ��������� (ID: {newProcessor.Id}) ������ � �����. ��������: {_availableProcessors.Count}, ������������: {_inUseProcessors.Count}.");
                    return newProcessor;
                }
                else
                {
                    Logger.Instance.Warning(SourceFilePath, "� ���� ��� ��������� �����������, � ������������ ������ ���� ���������! �� ������� ������ ���������.");
                    return null; 
                }
            }
        }

        /// <summary>
        /// ���������� SensorDataProcessor ������� � ���.
        /// </summary>
        /// <param name="processor">������ ��� �������� � ���.</param>
        public void ReleaseProcessor(SensorDataProcessor processor)
        {
            if (processor == null)
            {
                Logger.Instance.Warning(SourceFilePath, "������� ������� null ��������� � ���.");
                return;
            }

            lock (_lock)
            {
                if (_inUseProcessors.Contains(processor))
                {
                    processor.Reset(); 
                    _inUseProcessors.Remove(processor);
                    _availableProcessors.Add(processor);
                    Logger.Instance.Info(SourceFilePath, $"��������� (ID: {processor.Id}) ��������� � ���. ��������: {_availableProcessors.Count}, ������������: {_inUseProcessors.Count}.");
                }
                else
                {
                    Logger.Instance.Warning(SourceFilePath, $"������� ������� � ��� ��������� (ID: {processor.Id}), ������� �� ��� �� ���� ���� ��� ��� ���������.");
                }
            }
        }

        public int GetAvailableCount() => _availableProcessors.Count;
        public int GetInUseCount() => _inUseProcessors.Count;
    }
}