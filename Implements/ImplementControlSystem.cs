using Traktor.Core; // Добавлено для Logger

namespace Traktor.Implements
{
    /// <summary>
    /// Перечисление типов навесного оборудования, которым может управлять система.
    /// </summary>
    public enum ImplementType
    {
        /// <summary>
        /// Оборудование не выбрано или не подключено.
        /// </summary>
        None,
        /// <summary>
        /// Плуг.
        /// </summary>
        Plough,
        /// <summary>
        /// Сеялка.
        /// </summary>
        Seeder,
        /// <summary>
        /// Опрыскиватель.
        /// </summary>
        Sprayer
        // Можно добавить другие типы в будущем
    }

    /// <summary>
    /// Управляет работой навесного оборудования трактора.
    /// Позволяет подключать различные типы оборудования, настраивать их параметры и активировать/деактивировать.
    /// </summary>
    public class ImplementControlSystem
    {
        private ImplementType _activeImplementType = ImplementType.None;
        private double _currentPloughDepth = 0;         // метры
        private double _currentSeedingRate = 0;       // условных единиц (кг/га или семян/метр)
        private double _currentSprayerIntensity = 0;  // проценты (0-100)

        private bool _isOperationActive = false; // Флаг, что текущее оборудование активно выполняет операцию

        // private const string LogPrefix = "[Implements/ImplementControlSystem.cs]"; // Убрано, т.к. SourceFilePath будет использоваться
        private const string SourceFilePath = "Implements/ImplementControlSystem.cs"; // Определяем константу

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ImplementControlSystem"/>.
        /// </summary>
        public ImplementControlSystem()
        {
            Logger.Instance.Info(SourceFilePath, "Система управления навесным оборудованием инициализирована.");
        }

        /// <summary>
        /// Подключает (выбирает) указанный тип навесного оборудования.
        /// Невозможно сменить оборудование, если текущее активно выполняет операцию.
        /// </summary>
        /// <param name="type">Тип подключаемого оборудования.</param>
        public void AttachImplement(ImplementType type)
        {
            if (_isOperationActive)
            {
                Logger.Instance.Warning(SourceFilePath, $"AttachImplement: Нельзя сменить оборудование на '{type}', пока текущее ({_activeImplementType}) активно выполняет операцию. Сначала деактивируйте операцию.");
                return;
            }
            _activeImplementType = type;
            // Сброс параметров предыдущего оборудования (опционально, но логично)
            _currentPloughDepth = 0;
            _currentSeedingRate = 0;
            _currentSprayerIntensity = 0;
            Logger.Instance.Info(SourceFilePath, $"AttachImplement: Подключено (выбрано) оборудование: {_activeImplementType}. Параметры сброшены.");
        }

        /// <summary>
        /// Устанавливает глубину вспашки для плуга.
        /// </summary>
        /// <param name="depth">Желаемая глубина вспашки в метрах (не может быть отрицательной).</param>
        public void SetPloughDepth(double depth)
        {
            if (_activeImplementType != ImplementType.Plough)
            {
                Logger.Instance.Warning(SourceFilePath, $"SetPloughDepth: Плуг не подключен/выбран. Текущее оборудование: {_activeImplementType}.");
                return;
            }
            // Можно разрешить установку параметров даже если операция не активна,
            // но сами параметры будут применены только при активации.
            // if (!_isOperationActive)
            // {
            //     Logger.Instance.Debug(SourceFilePath, "SetPloughDepth: Плуг не выполняет операцию. Параметр будет применен при активации.");
            // }
            _currentPloughDepth = Math.Max(0, depth); // Глубина не может быть отрицательной
            Logger.Instance.Info(SourceFilePath, $"SetPloughDepth: Установлена глубина вспашки: {_currentPloughDepth:F2} м.");
        }

        /// <summary>
        /// Устанавливает норму высева для сеялки.
        /// </summary>
        /// <param name="rate">Желаемая норма высева (например, кг/га или семян/метр).</param>
        public void SetSeederRate(double rate)
        {
            if (_activeImplementType != ImplementType.Seeder)
            {
                Logger.Instance.Warning(SourceFilePath, $"SetSeederRate: Сеялка не подключена/выбрана. Текущее оборудование: {_activeImplementType}.");
                return;
            }
            _currentSeedingRate = Math.Max(0, rate);
            Logger.Instance.Info(SourceFilePath, $"SetSeederRate: Установлена норма высева: {_currentSeedingRate:F2} (условных единиц).");
        }

        /// <summary>
        /// Устанавливает интенсивность опрыскивания (0-100%).
        /// </summary>
        /// <param name="intensity">Желаемая интенсивность опрыскивания (0-100).</param>
        public void SetSprayerIntensity(double intensity)
        {
            if (_activeImplementType != ImplementType.Sprayer)
            {
                Logger.Instance.Warning(SourceFilePath, $"SetSprayerIntensity: Опрыскиватель не подключен/выбран. Текущее оборудование: {_activeImplementType}.");
                return;
            }
            _currentSprayerIntensity = Math.Clamp(intensity, 0, 100); // Интенсивность от 0 до 100%
            Logger.Instance.Info(SourceFilePath, $"SetSprayerIntensity: Установлена интенсивность опрыскивания: {_currentSprayerIntensity:F1}%.");
        }

        /// <summary>
        /// Активирует выполнение операции текущим подключенным навесным оборудованием.
        /// </summary>
        public void ActivateOperation()
        {
            if (_activeImplementType == ImplementType.None)
            {
                Logger.Instance.Warning(SourceFilePath, "ActivateOperation: Оборудование не подключено/не выбрано. Активация невозможна.");
                return;
            }
            if (_isOperationActive)
            {
                Logger.Instance.Info(SourceFilePath, $"ActivateOperation: Операция с оборудованием ({_activeImplementType}) уже активна.");
                return;
            }
            _isOperationActive = true;
            Logger.Instance.Info(SourceFilePath, $"ActivateOperation: Операция с оборудованием ({_activeImplementType}) АКТИВИРОВАНА.");
            // Здесь могла бы быть реальная логика: команда на опускание плуга, включение сеялки/опрыскивателя.
            // Например, в зависимости от _activeImplementType и установленных параметров:
            // if (_activeImplementType == ImplementType.Plough) Logger.Instance.Debug(SourceFilePath, $"...Плуг опущен на глубину {_currentPloughDepth:F2}м.");
        }

        /// <summary>
        /// Деактивирует выполнение операции текущим навесным оборудованием.
        /// </summary>
        public void DeactivateOperation()
        {
            if (!_isOperationActive)
            {
                Logger.Instance.Info(SourceFilePath, "DeactivateOperation: Операция с оборудованием уже деактивирована.");
                return;
            }
            _isOperationActive = false;
            Logger.Instance.Info(SourceFilePath, $"DeactivateOperation: Операция с оборудованием ({_activeImplementType}) ДЕАКТИВИРОВАНА.");
            // Сброс операционных параметров (глубины, нормы) при деактивации операции может быть не нужен,
            // т.к. они могут понадобиться при следующей активации. Но это зависит от требований.
        }

        // --- Методы для получения текущего состояния ---

        /// <summary>
        /// Проверяет, активно ли в данный момент выполнение операции навесным оборудованием.
        /// </summary>
        /// <returns>True, если операция активна, иначе false.</returns>
        public bool IsOperationActive() => _isOperationActive;

        /// <summary>
        /// Возвращает тип текущего подключенного (выбранного) навесного оборудования.
        /// </summary>
        /// <returns>Тип оборудования <see cref="ImplementType"/>.</returns>
        public ImplementType GetAttachedImplementType() => _activeImplementType;

        /// <summary>
        /// Получает текущую установленную глубину вспашки.
        /// </summary>
        /// <returns>Текущая глубина вспашки в метрах, если плуг подключен. Иначе может вернуть 0 или другое значение по умолчанию.</returns>
        public double GetCurrentPloughDepth() => _activeImplementType == ImplementType.Plough ? _currentPloughDepth : 0;

        /// <summary>
        /// Получает текущую установленную норму высева.
        /// </summary>
        /// <returns>Текущая норма высева, если сеялка подключена. Иначе 0.</returns>
        public double GetCurrentSeedingRate() => _activeImplementType == ImplementType.Seeder ? _currentSeedingRate : 0;

        /// <summary>
        /// Получает текущую установленную интенсивность опрыскивания.
        /// </summary>
        /// <returns>Текущая интенсивность опрыскивания в процентах, если опрыскиватель подключен. Иначе 0.</returns>
        public double GetCurrentSprayerIntensity() => _activeImplementType == ImplementType.Sprayer ? _currentSprayerIntensity : 0;
    }
}