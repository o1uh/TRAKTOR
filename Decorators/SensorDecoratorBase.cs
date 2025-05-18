using Traktor.Interfaces; // Для ISensors<T>

namespace Traktor.Decorators
{
    /// <summary>
    /// Абстрактный базовый класс для декораторов сенсоров.
    /// Реализует ISensors<T> и делегирует вызовы обернутому сенсору.
    /// </summary>
    /// <typeparam name="T">Тип данных, возвращаемых сенсором.</typeparam>
    public abstract class SensorDecoratorBase<T> : ISensors<T>
    {
        protected readonly ISensors<T> _wrappedSensor;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SensorDecoratorBase{T}"/>.
        /// </summary>
        /// <param name="wrappedSensor">Сенсор, который будет декорирован.</param>
        /// <exception cref="ArgumentNullException">Если wrappedSensor равен null.</exception>
        protected SensorDecoratorBase(ISensors<T> wrappedSensor)
        {
            _wrappedSensor = wrappedSensor ?? throw new ArgumentNullException(nameof(wrappedSensor));
            // Логирование создания будет в конкретных декораторах,
            // так как там будет известно имя конкретного декоратора и его SourceFilePath.
        }

        /// <summary>
        /// Получает данные от обернутого сенсора.
        /// Может быть переопределен в производных классах для добавления поведения.
        /// </summary>
        /// <returns>Данные от сенсора.</returns>
        public virtual T GetData()
        {
            // В базовом классе просто делегируем.
            // Конкретные декораторы добавят логику до/после этого вызова.
            return _wrappedSensor.GetData();
        }
    }
}