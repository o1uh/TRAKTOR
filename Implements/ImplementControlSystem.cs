using System;

namespace Traktor.Implements // ����� Implements
{
    // ������������ ��� ����� ��������� ������������, ���� �� ���������
    public enum ImplementType
    {
        None,
        Plough,  // ����
        Seeder,  // ������
        Sprayer  // �������������
        // ... ������ ���� ...
    }

    public class ImplementControlSystem
    {
        private ImplementType _activeImplement = ImplementType.None;
        private double _currentDepth = 0;       // �����
        private double _currentSeedingRate = 0; // ��/�� ��� �����/����
        private double _currentSprayingIntensity = 0; // �/�� ��� % �������� ��������

        private bool _isImplementActive = false;

        public ImplementControlSystem()
        {
            Console.WriteLine("[ImplementControlSystem]: ������� ���������� �������� ������������� ����������������.");
        }

        // ����� ��� "�����������" ��� ������ ���� ������������
        public void AttachImplement(ImplementType type)
        {
            if (_isImplementActive)
            {
                Console.WriteLine($"[ImplementControlSystem]: ������ ������� ������������ ({type}), ���� ������� ({_activeImplement}) �������. ������� �������������.");
                return;
            }
            _activeImplement = type;
            Console.WriteLine($"[ImplementControlSystem]: ���������� ������������: {_activeImplement}.");
        }

        public void SetPloughDepth(double depth)
        {
            if (_activeImplement != ImplementType.Plough)
            {
                Console.WriteLine("[ImplementControlSystem]: SetPloughDepth - ���� �� ��������� ��� �� ������.");
                return;
            }
            if (!_isImplementActive)
            {
                Console.WriteLine("[ImplementControlSystem]: SetPloughDepth - ���� �� �������. ������� ����������� ������������.");
                return;
            }
            _currentDepth = Math.Max(0, depth); // ������� �� ����� ���� �������������
            Console.WriteLine($"[ImplementControlSystem]: ����������� ������� �������: {_currentDepth:F2} �.");
        }

        public void SetSeederRate(double rate)
        {
            if (_activeImplement != ImplementType.Seeder)
            {
                Console.WriteLine("[ImplementControlSystem]: SetSeederRate - ������ �� ���������� ��� �� �������.");
                return;
            }
            if (!_isImplementActive)
            {
                Console.WriteLine("[ImplementControlSystem]: SetSeederRate - ������ �� �������. ������� ����������� ������������.");
                return;
            }
            _currentSeedingRate = Math.Max(0, rate);
            Console.WriteLine($"[ImplementControlSystem]: ����������� ����� ������: {_currentSeedingRate:F2} (�������� ������).");
        }

        public void SetSprayerIntensity(double intensity)
        {
            if (_activeImplement != ImplementType.Sprayer)
            {
                Console.WriteLine("[ImplementControlSystem]: SetSprayerIntensity - ������������� �� ��������� ��� �� ������.");
                return;
            }
            if (!_isImplementActive)
            {
                Console.WriteLine("[ImplementControlSystem]: SetSprayerIntensity - ������������� �� �������. ������� ����������� ������������.");
                return;
            }
            _currentSprayingIntensity = Math.Clamp(intensity, 0, 100); // ������������� �� 0 �� 100%
            Console.WriteLine($"[ImplementControlSystem]: ����������� ������������� ������������: {_currentSprayingIntensity:F1}%.");
        }

        public void ActivateImplement()
        {
            if (_activeImplement == ImplementType.None)
            {
                Console.WriteLine("[ImplementControlSystem]: ActivateImplement - ������������ �� ����������/�� �������.");
                return;
            }
            if (_isImplementActive)
            {
                Console.WriteLine($"[ImplementControlSystem]: ������������ ({_activeImplement}) ��� �������.");
                return;
            }
            _isImplementActive = true;
            Console.WriteLine($"[ImplementControlSystem]: ������������ ({_activeImplement}) ������������.");
            // ����� ����� �� ���� ������ �������� ������ �� ���������� � �.�.
        }

        public void DeactivateImplement()
        {
            if (!_isImplementActive)
            {
                Console.WriteLine("[ImplementControlSystem]: ������������ ��� ��������������.");
                return;
            }
            _isImplementActive = false;
            Console.WriteLine($"[ImplementControlSystem]: ������������ ({_activeImplement}) ��������������.");
            // ����� ���������� ��� ����������� ����� ���� ������������
            // _currentDepth = 0;
            // _currentSeedingRate = 0;
            // _currentSprayingIntensity = 0;
        }

        // ��������������� ������ ��� ��������� �������� ��������� (����� ������������ ControlUnit)
        public bool IsImplementActive() => _isImplementActive;
        public ImplementType GetActiveImplementType() => _activeImplement;
        public double GetCurrentPloughDepth() => _activeImplement == ImplementType.Plough && _isImplementActive ? _currentDepth : -1; // -1 ��� ��������� ��������������
        // ... ����������� ������� ��� ������ ���������� ...
    }
}