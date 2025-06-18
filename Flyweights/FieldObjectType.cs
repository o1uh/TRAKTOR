using Traktor.Interfaces; 
using Traktor.Core;       
using Traktor.DataModels; 

namespace Traktor.Flyweights
{
    /// <summary>
    /// Конкретный Приспособленец (ConcreteFlyweight).
    /// Хранит внутреннее (разделяемое) состояние - например, тип текстуры, цвет, базовое имя.
    /// </summary>
    public class FieldObjectType : IFieldObjectType
    {
        private const string SourceFilePath = "Flyweights/FieldObjectType.cs";

        private readonly string _typeName;        
        private readonly string _textureName;     
        private readonly ConsoleColor _displayColor; 

        /// <summary>
        /// Конструктор обычно приватный или internal, чтобы создание шло только через фабрику.
        /// Для макета сделаем public, но с логированием, подчеркивающим создание НОВОГО экземпляра.
        /// </summary>
        public FieldObjectType(string typeName, string textureName, ConsoleColor displayColor)
        {
            _typeName = typeName;
            _textureName = textureName;
            _displayColor = displayColor;
            Logger.Instance.Info(SourceFilePath, $"СОЗДАН НОВЫЙ экземпляр FieldObjectType (Приспособленец): Тип='{_typeName}', Текстура='{_textureName}', Цвет='{_displayColor}'.");
        }

        /// <summary>
        /// "Отображает" объект, используя внутреннее и внешнее состояние.
        /// </summary>
        /// <param name="position">Внешнее состояние: позиция.</param>
        /// <param name="uniqueId">Внешнее состояние: уникальный ID экземпляра на поле.</param>
        public void Display(Coordinates position, string uniqueId)
        {
            string logMessage = $"Отображение (ID: {uniqueId}): Тип='{_typeName}' (Текстура: '{_textureName}', Цвет: {_displayColor}) в позиции {position}.";
            Logger.Instance.Info(SourceFilePath, logMessage);
        }

        public string GetIntrinsicStateDescription()
        {
            return $"Тип='{_typeName}', Текстура='{_textureName}', Цвет='{_displayColor}'";
        }
    }
}