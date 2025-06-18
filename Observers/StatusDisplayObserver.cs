using System; 
using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.Observers
{
    /// <summary>
    /// Конкретный Наблюдатель, который "отображает" (логирует) статус или сообщения от Субъекта.
    /// </summary>
    public class StatusDisplayObserver : IObserver
    {
        private const string SourceFilePath = "Observers/StatusDisplayObserver.cs";
        private readonly string _observerName;

        /// <summary>
        /// Инициализирует новый экземпляр наблюдателя.
        /// </summary>
        /// <param name="name">Имя наблюдателя (для идентификации в логах).</param>
        public StatusDisplayObserver(string name)
        {
            _observerName = name ?? throw new ArgumentNullException(nameof(name));
            Logger.Instance.Info(SourceFilePath, $"Наблюдатель '{_observerName}' создан.");
        }

        /// <summary>
        /// Метод, вызываемый Субъектом при обновлении.
        /// Логирует полученное сообщение.
        /// </summary>
        /// <param name="subject">Ссылка на Субъекта (в данном макете не используется напрямую в логике Update).</param>
        /// <param name="message">Сообщение от Субъекта.</param>
        public void Update(ISubject subject, string message)
        {
            string subjectName = subject != null ? subject.GetType().Name : "Неизвестный субъект";
            Logger.Instance.Info(SourceFilePath, $"Наблюдатель '{_observerName}': Получено уведомление от '{subjectName}'. Сообщение: '{message}'");
            // Здесь могла бы быть логика обновления UI или другие действия
        }
    }
}