using Traktor.Core; // ��������� ��� Logger

namespace Traktor.Implements
{
    /// <summary>
    /// ������������ ����� ��������� ������������, ������� ����� ��������� �������.
    /// </summary>
    public enum ImplementType
    {
        /// <summary>
        /// ������������ �� ������� ��� �� ����������.
        /// </summary>
        None,
        /// <summary>
        /// ����.
        /// </summary>
        Plough,
        /// <summary>
        /// ������.
        /// </summary>
        Seeder,
        /// <summary>
        /// �������������.
        /// </summary>
        Sprayer
        // ����� �������� ������ ���� � �������
    }

    /// <summary>
    /// ��������� ������� ��������� ������������ ��������.
    /// ��������� ���������� ��������� ���� ������������, ����������� �� ��������� � ������������/��������������.
    /// </summary>
    public class ImplementControlSystem
    {
        private ImplementType _activeImplementType = ImplementType.None;
        private double _currentPloughDepth = 0;         // �����
        private double _currentSeedingRate = 0;       // �������� ������ (��/�� ��� �����/����)
        private double _currentSprayerIntensity = 0;  // �������� (0-100)

        private bool _isOperationActive = false; // ����, ��� ������� ������������ ������� ��������� ��������

        // private const string LogPrefix = "[Implements/ImplementControlSystem.cs]"; // ������, �.�. SourceFilePath ����� ��������������
        private const string SourceFilePath = "Implements/ImplementControlSystem.cs"; // ���������� ���������

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="ImplementControlSystem"/>.
        /// </summary>
        public ImplementControlSystem()
        {
            Logger.Instance.Info(SourceFilePath, "������� ���������� �������� ������������� ����������������.");
        }

        /// <summary>
        /// ���������� (��������) ��������� ��� ��������� ������������.
        /// ���������� ������� ������������, ���� ������� ������� ��������� ��������.
        /// </summary>
        /// <param name="type">��� ������������� ������������.</param>
        public void AttachImplement(ImplementType type)
        {
            if (_isOperationActive)
            {
                Logger.Instance.Warning(SourceFilePath, $"AttachImplement: ������ ������� ������������ �� '{type}', ���� ������� ({_activeImplementType}) ������� ��������� ��������. ������� ������������� ��������.");
                return;
            }
            _activeImplementType = type;
            // ����� ���������� ����������� ������������ (�����������, �� �������)
            _currentPloughDepth = 0;
            _currentSeedingRate = 0;
            _currentSprayerIntensity = 0;
            Logger.Instance.Info(SourceFilePath, $"AttachImplement: ���������� (�������) ������������: {_activeImplementType}. ��������� ��������.");
        }

        /// <summary>
        /// ������������� ������� ������� ��� �����.
        /// </summary>
        /// <param name="depth">�������� ������� ������� � ������ (�� ����� ���� �������������).</param>
        public void SetPloughDepth(double depth)
        {
            if (_activeImplementType != ImplementType.Plough)
            {
                Logger.Instance.Warning(SourceFilePath, $"SetPloughDepth: ���� �� ���������/������. ������� ������������: {_activeImplementType}.");
                return;
            }
            // ����� ��������� ��������� ���������� ���� ���� �������� �� �������,
            // �� ���� ��������� ����� ��������� ������ ��� ���������.
            // if (!_isOperationActive)
            // {
            //     Logger.Instance.Debug(SourceFilePath, "SetPloughDepth: ���� �� ��������� ��������. �������� ����� �������� ��� ���������.");
            // }
            _currentPloughDepth = Math.Max(0, depth); // ������� �� ����� ���� �������������
            Logger.Instance.Info(SourceFilePath, $"SetPloughDepth: ����������� ������� �������: {_currentPloughDepth:F2} �.");
        }

        /// <summary>
        /// ������������� ����� ������ ��� ������.
        /// </summary>
        /// <param name="rate">�������� ����� ������ (��������, ��/�� ��� �����/����).</param>
        public void SetSeederRate(double rate)
        {
            if (_activeImplementType != ImplementType.Seeder)
            {
                Logger.Instance.Warning(SourceFilePath, $"SetSeederRate: ������ �� ����������/�������. ������� ������������: {_activeImplementType}.");
                return;
            }
            _currentSeedingRate = Math.Max(0, rate);
            Logger.Instance.Info(SourceFilePath, $"SetSeederRate: ����������� ����� ������: {_currentSeedingRate:F2} (�������� ������).");
        }

        /// <summary>
        /// ������������� ������������� ������������ (0-100%).
        /// </summary>
        /// <param name="intensity">�������� ������������� ������������ (0-100).</param>
        public void SetSprayerIntensity(double intensity)
        {
            if (_activeImplementType != ImplementType.Sprayer)
            {
                Logger.Instance.Warning(SourceFilePath, $"SetSprayerIntensity: ������������� �� ���������/������. ������� ������������: {_activeImplementType}.");
                return;
            }
            _currentSprayerIntensity = Math.Clamp(intensity, 0, 100); // ������������� �� 0 �� 100%
            Logger.Instance.Info(SourceFilePath, $"SetSprayerIntensity: ����������� ������������� ������������: {_currentSprayerIntensity:F1}%.");
        }

        /// <summary>
        /// ���������� ���������� �������� ������� ������������ �������� �������������.
        /// </summary>
        public void ActivateOperation()
        {
            if (_activeImplementType == ImplementType.None)
            {
                Logger.Instance.Warning(SourceFilePath, "ActivateOperation: ������������ �� ����������/�� �������. ��������� ����������.");
                return;
            }
            if (_isOperationActive)
            {
                Logger.Instance.Info(SourceFilePath, $"ActivateOperation: �������� � ������������� ({_activeImplementType}) ��� �������.");
                return;
            }
            _isOperationActive = true;
            Logger.Instance.Info(SourceFilePath, $"ActivateOperation: �������� � ������������� ({_activeImplementType}) ������������.");
            // ����� ����� �� ���� �������� ������: ������� �� ��������� �����, ��������� ������/�������������.
            // ��������, � ����������� �� _activeImplementType � ������������� ����������:
            // if (_activeImplementType == ImplementType.Plough) Logger.Instance.Debug(SourceFilePath, $"...���� ������ �� ������� {_currentPloughDepth:F2}�.");
        }

        /// <summary>
        /// ������������ ���������� �������� ������� �������� �������������.
        /// </summary>
        public void DeactivateOperation()
        {
            if (!_isOperationActive)
            {
                Logger.Instance.Info(SourceFilePath, "DeactivateOperation: �������� � ������������� ��� ��������������.");
                return;
            }
            _isOperationActive = false;
            Logger.Instance.Info(SourceFilePath, $"DeactivateOperation: �������� � ������������� ({_activeImplementType}) ��������������.");
            // ����� ������������ ���������� (�������, �����) ��� ����������� �������� ����� ���� �� �����,
            // �.�. ��� ����� ������������ ��� ��������� ���������. �� ��� ������� �� ����������.
        }

        // --- ������ ��� ��������� �������� ��������� ---

        /// <summary>
        /// ���������, ������� �� � ������ ������ ���������� �������� �������� �������������.
        /// </summary>
        /// <returns>True, ���� �������� �������, ����� false.</returns>
        public bool IsOperationActive() => _isOperationActive;

        /// <summary>
        /// ���������� ��� �������� ������������� (����������) ��������� ������������.
        /// </summary>
        /// <returns>��� ������������ <see cref="ImplementType"/>.</returns>
        public ImplementType GetAttachedImplementType() => _activeImplementType;

        /// <summary>
        /// �������� ������� ������������� ������� �������.
        /// </summary>
        /// <returns>������� ������� ������� � ������, ���� ���� ���������. ����� ����� ������� 0 ��� ������ �������� �� ���������.</returns>
        public double GetCurrentPloughDepth() => _activeImplementType == ImplementType.Plough ? _currentPloughDepth : 0;

        /// <summary>
        /// �������� ������� ������������� ����� ������.
        /// </summary>
        /// <returns>������� ����� ������, ���� ������ ����������. ����� 0.</returns>
        public double GetCurrentSeedingRate() => _activeImplementType == ImplementType.Seeder ? _currentSeedingRate : 0;

        /// <summary>
        /// �������� ������� ������������� ������������� ������������.
        /// </summary>
        /// <returns>������� ������������� ������������ � ���������, ���� ������������� ���������. ����� 0.</returns>
        public double GetCurrentSprayerIntensity() => _activeImplementType == ImplementType.Sprayer ? _currentSprayerIntensity : 0;
    }
}