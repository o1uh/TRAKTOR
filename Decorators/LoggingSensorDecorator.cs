using Traktor.Interfaces; // Для ISensors<T>
using Traktor.Core;       // Для Logger

namespace Traktor.Decorators
{
    /// <summary>
    /// Конкретный декоратор, добавляющий логирование до и после вызова GetData()
    /// у обернутого сенсора.
    /// </summary>
    /// <typeparam name="T">Тип данных, возвращаемых сенсором.</typeparam>
    public class LoggingSensorDecorator<T> : SensorDecoratorBase<T>
    {
        private const string SourceFilePath = "Decorators/LoggingSensorDecorator.cs"; // Путь для логгера

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="LoggingSensorDecorator{T}"/>.
        /// </summary>
        /// <param name="wrappedSensor">Сенсор, который будет декорирован логированием.</param>
        public LoggingSensorDecorator(ISensors<T> wrappedSensor) : base(wrappedSensor)
        {
            // _wrappedSensor уже проверен на null в базовом конструкторе.
            Logger.Instance.Info(SourceFilePath, $"LoggingSensorDecorator<{typeof(T).Name}> создан. Оборачивает сенсор типа: '{_wrappedSensor.GetType().FullName}'.");
        }

        /// <summary>
        /// Получает данные от обернутого сенсора, добавляя логирование.
        /// </summary>
        /// <returns>Данные от сенсора.</returns>
        public override T GetData()
        {
            Logger.Instance.Debug(SourceFilePath, $"LoggingSensorDecorator<{typeof(T).Name}>: Перед вызовом GetData() у обернутого сенсора ({_wrappedSensor.GetType().Name}).");

            T data = default(T); // Инициализируем значением по умолчанию
            try
            {
                // Вызываем метод GetData() базового класса, который, в свою очередь,
                // вызовет GetData() у _wrappedSensor.
                data = base.GetData();

                Logger.Instance.Debug(SourceFilePath, $"LoggingSensorDecorator<{typeof(T).Name}>: После вызова GetData() у обернутого сенсора. Получены данные: [{data}].");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"LoggingSensorDecorator<{typeof(T).Name}>: Ошибка при вызове GetData() у обернутого сенсора ({_wrappedSensor.GetType().Name}): {ex.Message}", ex);
                throw; // Перебрасываем исключение, чтобы не изменять поведение для клиента декоратора
            }
            return data;
        }
    }
}