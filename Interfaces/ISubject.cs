namespace Traktor.Interfaces
{
    /// <summary>
    /// Интерфейс для Субъекта (Subject/Publisher).
    /// Определяет методы для управления подписчиками и их уведомления.
    /// </summary>
    public interface ISubject
    {
        /// <summary>
        /// Добавляет Наблюдателя (подписывает на обновления).
        /// </summary>
        /// <param name="observer">Наблюдатель для добавления.</param>
        void Attach(IObserver observer);

        /// <summary>
        /// Удаляет Наблюдателя (отписывает от обновлений).
        /// </summary>
        /// <param name="observer">Наблюдатель для удаления.</param>
        void Detach(IObserver observer);

        /// <summary>
        /// Уведомляет всех подписанных Наблюдателей об изменении.
        /// </summary>
        /// <param name="message">Сообщение или данные об изменении.</param>
        void Notify(string message);
    }
}