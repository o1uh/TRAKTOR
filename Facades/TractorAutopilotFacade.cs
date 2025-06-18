using System;
using Traktor.Core;       
using Traktor.Mocks;      
using Traktor.DataModels; 
using Traktor.Implements; 

namespace Traktor.Facades
{
    /// <summary>
    /// Фасад, предоставляющий упрощенный интерфейс для управления основными операциями автопилота трактора.
    /// Скрывает сложность взаимодействия с ControlUnit и его подсистемами.
    /// </summary>
    public class TractorAutopilotFacade
    {
        private const string SourceFilePath = "Facades/TractorAutopilotFacade.cs";
        private readonly MockControlUnit _mockControlUnit; 

        /// <summary>
        /// Инициализирует новый экземпляр фасада.
        /// </summary>
        /// <param name="controlUnit">Экземпляр MockControlUnit (или ControlUnit), которым будет управлять фасад.</param>
        public TractorAutopilotFacade(MockControlUnit controlUnit)
        {
            _mockControlUnit = controlUnit ?? throw new ArgumentNullException(nameof(controlUnit));
            Logger.Instance.Info(SourceFilePath, $"TractorAutopilotFacade создан. Использует: {_mockControlUnit.GetType().FullName}.");
        }

        /// <summary>
        /// Упрощенный метод для запуска полной операции обработки поля.
        /// </summary>
        /// <param name="fieldId">Идентификатор поля (для примера, не используется в MockControlUnit).</param>
        /// <param name="startLatitude">Начальная широта для маршрута.</param>
        /// <param name="startLongitude">Начальная долгота для маршрута.</param>
        /// <param name="implement">Тип навесного оборудования.</param>
        /// <returns>True, если команда на запуск была успешно отправлена, иначе false.</returns>
        public bool StartFullFieldOperation(string fieldId, double startLatitude, double startLongitude, ImplementType implement)
        {
            Logger.Instance.Info(SourceFilePath, $"Facade: Запрос на полную обработку поля '{fieldId}' с оборудованием '{implement}' от точки ({startLatitude}, {startLongitude}).");

            var target = new Coordinates(startLatitude, startLongitude);
            
            Logger.Instance.Debug(SourceFilePath, "Facade: Делегирование запроса на запуск в MockControlUnit...");
            _mockControlUnit.RequestStart(target, implement, null);

            Logger.Instance.Info(SourceFilePath, $"Facade: Команда на запуск полной обработки поля '{fieldId}' отправлена.");
            return true; 
        }

        /// <summary>
        /// Упрощенный метод для остановки всех операций.
        /// </summary>
        public void StopAllOperations()
        {
            Logger.Instance.Info(SourceFilePath, "Facade: Запрос на остановку всех операций.");
            Logger.Instance.Debug(SourceFilePath, "Facade: Делегирование запроса на остановку в MockControlUnit...");
            _mockControlUnit.RequestStop();
            Logger.Instance.Info(SourceFilePath, "Facade: Команда на остановку всех операций отправлена.");
        }

        /// <summary>
        /// Упрощенный метод для получения базового статуса.
        /// </summary>
        /// <returns>Строка с очень кратким статусом.</returns>
        public string GetQuickStatus()
        {
            string currentStatus = "Статус не доступен через MockControlUnit в фасаде (требует доработки MockControlUnit или интерфейса)";

            Logger.Instance.Info(SourceFilePath, $"Facade: Запрос на быстрый статус. Возвращаем: '{currentStatus}'.");
            return currentStatus;
        }
    }
}