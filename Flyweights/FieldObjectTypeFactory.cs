using System.Collections.Generic; // Для Dictionary
using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.Flyweights
{
    /// <summary>
    /// Фабрика Приспособленцев (FlyweightFactory).
    /// Создает и управляет пулом объектов FieldObjectType.
    /// </summary>
    public class FieldObjectTypeFactory
    {
        private const string SourceFilePath = "Flyweights/FieldObjectTypeFactory.cs";
        private readonly Dictionary<string, IFieldObjectType> _flyweights = new Dictionary<string, IFieldObjectType>();

        public FieldObjectTypeFactory()
        {
            Logger.Instance.Info(SourceFilePath, "FieldObjectTypeFactory создана.");
        }

        /// <summary>
        /// Получает или создает Приспособленца (FieldObjectType) с указанными внутренними параметрами.
        /// </summary>
        /// <param name="typeName">Ключ для идентификации типа (например, "Камень", "Пшеница").</param>
        /// <param name="textureName">Имя текстуры для этого типа.</param>
        /// <param name="displayColor">Цвет для отображения этого типа.</param>
        /// <returns>Экземпляр IFieldObjectType (возможно, разделяемый).</returns>
        public IFieldObjectType GetFlyweight(string typeName, string textureName, ConsoleColor displayColor)
        {
            string key = typeName;

            if (_flyweights.TryGetValue(key, out IFieldObjectType flyweight))
            {
                Logger.Instance.Info(SourceFilePath, $"Возвращен СУЩЕСТВУЮЩИЙ приспособленец для ключа '{key}'. Внутреннее состояние: {flyweight.GetIntrinsicStateDescription()}");
            }
            else
            {
                Logger.Instance.Info(SourceFilePath, $"Приспособленец для ключа '{key}' не найден. Создание НОВОГО...");
                flyweight = new FieldObjectType(typeName, textureName, displayColor);
                _flyweights.Add(key, flyweight);
                Logger.Instance.Info(SourceFilePath, $"Новый приспособленец для ключа '{key}' добавлен в пул. Всего в пуле: {_flyweights.Count}.");
            }
            return flyweight;
        }

        /// <summary>
        /// Возвращает количество уникальных приспособленцев (типов объектов) в пуле.
        /// </summary>
        public int GetFlyweightsCount()
        {
            int count = _flyweights.Count;
            Logger.Instance.Debug(SourceFilePath, $"Текущее количество уникальных приспособленцев в пуле: {count}.");
            return count;
        }
    }
}