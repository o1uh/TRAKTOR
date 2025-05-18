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

        private const string LogPrefix = "[Implements/ImplementControlSystem.cs]";

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="ImplementControlSystem"/>.
        /// </summary>
        public ImplementControlSystem()
        {
            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ������� ���������� �������� ������������� ����������������.");
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
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AttachImplement: ������ ������� ������������ �� '{type}', ���� ������� ({_activeImplementType}) ������� ��������� ��������. ������� ������������� ��������.");
                return;
            }
            _activeImplementType = type;
            // ����� ���������� ����������� ������������ (�����������, �� �������)
            _currentPloughDepth = 0;
            _currentSeedingRate = 0;
            _currentSprayerIntensity = 0;
            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: AttachImplement: ���������� (�������) ������������: {_activeImplementType}. ��������� ��������.");
        }

        /// <summary>
        /// ������������� ������� ������� ��� �����.
        /// </summary>
        /// <param name="depth">�������� ������� ������� � ������ (�� ����� ���� �������������).</param>
        public void SetPloughDepth(double depth)
        {
            if (_activeImplementType != ImplementType.Plough)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SetPloughDepth: ���� �� ���������/������. ������� ������������: {_activeImplementType}.");
                return;
            }
            // ����� ��������� ��������� ���������� ���� ���� �������� �� �������,
            // �� ���� ��������� ����� ��������� ������ ��� ���������.
            // if (!_isOperationActive)
            // {
            //     Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SetPloughDepth: ���� �� ��������� ��������. �������� ����� �������� ��� ���������.");
            // }
            _currentPloughDepth = Math.Max(0, depth); // ������� �� ����� ���� �������������
            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SetPloughDepth: ����������� ������� �������: {_currentPloughDepth:F2} �.");
        }

        /// <summary>
        /// ������������� ����� ������ ��� ������.
        /// </summary>
        /// <param name="rate">�������� ����� ������ (��������, ��/�� ��� �����/����).</param>
        public void SetSeederRate(double rate)
        {
            if (_activeImplementType != ImplementType.Seeder)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SetSeederRate: ������ �� ����������/�������. ������� ������������: {_activeImplementType}.");
                return;
            }
            _currentSeedingRate = Math.Max(0, rate);
            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SetSeederRate: ����������� ����� ������: {_currentSeedingRate:F2} (�������� ������).");
        }

        /// <summary>
        /// ������������� ������������� ������������ (0-100%).
        /// </summary>
        /// <param name="intensity">�������� ������������� ������������ (0-100).</param>
        public void SetSprayerIntensity(double intensity)
        {
            if (_activeImplementType != ImplementType.Sprayer)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SetSprayerIntensity: ������������� �� ���������/������. ������� ������������: {_activeImplementType}.");
                return;
            }
            _currentSprayerIntensity = Math.Clamp(intensity, 0, 100); // ������������� �� 0 �� 100%
            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: SetSprayerIntensity: ����������� ������������� ������������: {_currentSprayerIntensity:F1}%.");
        }

        /// <summary>
        /// ���������� ���������� �������� ������� ������������ �������� �������������.
        /// </summary>
        public void ActivateOperation()
        {
            if (_activeImplementType == ImplementType.None)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ActivateOperation: ������������ �� ����������/�� �������. ��������� ����������.");
                return;
            }
            if (_isOperationActive)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ActivateOperation: �������� � ������������� ({_activeImplementType}) ��� �������.");
                return;
            }
            _isOperationActive = true;
            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: ActivateOperation: �������� � ������������� ({_activeImplementType}) ������������.");
            // ����� ����� �� ���� �������� ������: ������� �� ��������� �����, ��������� ������/�������������.
            // ��������, � ����������� �� _activeImplementType � ������������� ����������:
            // if (_activeImplementType == ImplementType.Plough) Console.WriteLine($"...���� ������ �� ������� {_currentPloughDepth:F2}�.");
        }

        /// <summary>
        /// ������������ ���������� �������� ������� �������� �������������.
        /// </summary>
        public void DeactivateOperation()
        {
            if (!_isOperationActive)
            {
                Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DeactivateOperation: �������� � ������������� ��� ��������������.");
                return;
            }
            _isOperationActive = false;
            Console.WriteLine($"{LogPrefix}-[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: DeactivateOperation: �������� � ������������� ({_activeImplementType}) ��������������.");
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