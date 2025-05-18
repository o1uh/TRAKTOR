using System;

namespace Traktor.Implements // Папка Implements
{
    // Перечисление для типов навесного оборудования, если их несколько
    public enum ImplementType
    {
        None,
        Plough,  // Плуг
        Seeder,  // Сеялка
        Sprayer  // Опрыскиватель
        // ... другие типы ...
    }

    public class ImplementControlSystem
    {
        private ImplementType _activeImplement = ImplementType.None;
        private double _currentDepth = 0;       // метры
        private double _currentSeedingRate = 0; // кг/га или семян/метр
        private double _currentSprayingIntensity = 0; // л/га или % открытия форсунок

        private bool _isImplementActive = false;

        public ImplementControlSystem()
        {
            Console.WriteLine("[ImplementControlSystem]: Система управления навесным оборудованием инициализирована.");
        }

        // Метод для "подключения" или выбора типа оборудования
        public void AttachImplement(ImplementType type)
        {
            if (_isImplementActive)
            {
                Console.WriteLine($"[ImplementControlSystem]: Нельзя сменить оборудование ({type}), пока текущее ({_activeImplement}) активно. Сначала деактивируйте.");
                return;
            }
            _activeImplement = type;
            Console.WriteLine($"[ImplementControlSystem]: Подключено оборудование: {_activeImplement}.");
        }

        public void SetPloughDepth(double depth)
        {
            if (_activeImplement != ImplementType.Plough)
            {
                Console.WriteLine("[ImplementControlSystem]: SetPloughDepth - Плуг не подключен или не выбран.");
                return;
            }
            if (!_isImplementActive)
            {
                Console.WriteLine("[ImplementControlSystem]: SetPloughDepth - Плуг не активен. Сначала активируйте оборудование.");
                return;
            }
            _currentDepth = Math.Max(0, depth); // Глубина не может быть отрицательной
            Console.WriteLine($"[ImplementControlSystem]: Установлена глубина вспашки: {_currentDepth:F2} м.");
        }

        public void SetSeederRate(double rate)
        {
            if (_activeImplement != ImplementType.Seeder)
            {
                Console.WriteLine("[ImplementControlSystem]: SetSeederRate - Сеялка не подключена или не выбрана.");
                return;
            }
            if (!_isImplementActive)
            {
                Console.WriteLine("[ImplementControlSystem]: SetSeederRate - Сеялка не активна. Сначала активируйте оборудование.");
                return;
            }
            _currentSeedingRate = Math.Max(0, rate);
            Console.WriteLine($"[ImplementControlSystem]: Установлена норма высева: {_currentSeedingRate:F2} (условных единиц).");
        }

        public void SetSprayerIntensity(double intensity)
        {
            if (_activeImplement != ImplementType.Sprayer)
            {
                Console.WriteLine("[ImplementControlSystem]: SetSprayerIntensity - Опрыскиватель не подключен или не выбран.");
                return;
            }
            if (!_isImplementActive)
            {
                Console.WriteLine("[ImplementControlSystem]: SetSprayerIntensity - Опрыскиватель не активен. Сначала активируйте оборудование.");
                return;
            }
            _currentSprayingIntensity = Math.Clamp(intensity, 0, 100); // Интенсивность от 0 до 100%
            Console.WriteLine($"[ImplementControlSystem]: Установлена интенсивность опрыскивания: {_currentSprayingIntensity:F1}%.");
        }

        public void ActivateImplement()
        {
            if (_activeImplement == ImplementType.None)
            {
                Console.WriteLine("[ImplementControlSystem]: ActivateImplement - Оборудование не подключено/не выбрано.");
                return;
            }
            if (_isImplementActive)
            {
                Console.WriteLine($"[ImplementControlSystem]: Оборудование ({_activeImplement}) уже активно.");
                return;
            }
            _isImplementActive = true;
            Console.WriteLine($"[ImplementControlSystem]: Оборудование ({_activeImplement}) активировано.");
            // Здесь могла бы быть логика отправки команд на гидравлику и т.д.
        }

        public void DeactivateImplement()
        {
            if (!_isImplementActive)
            {
                Console.WriteLine("[ImplementControlSystem]: Оборудование уже деактивировано.");
                return;
            }
            _isImplementActive = false;
            Console.WriteLine($"[ImplementControlSystem]: Оборудование ({_activeImplement}) деактивировано.");
            // Сброс параметров при деактивации может быть опциональным
            // _currentDepth = 0;
            // _currentSeedingRate = 0;
            // _currentSprayingIntensity = 0;
        }

        // Вспомогательные методы для получения текущего состояния (могут понадобиться ControlUnit)
        public bool IsImplementActive() => _isImplementActive;
        public ImplementType GetActiveImplementType() => _activeImplement;
        public double GetCurrentPloughDepth() => _activeImplement == ImplementType.Plough && _isImplementActive ? _currentDepth : -1; // -1 как индикатор неактуальности
        // ... аналогичные геттеры для других параметров ...
    }
}