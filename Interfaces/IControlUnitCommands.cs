using Traktor.DataModels; 
using Traktor.Implements; 

namespace Traktor.Interfaces
{
    /// <summary>
    /// ќпредел€ет команды, которые ControlUnit должен уметь выполн€ть 
    /// дл€ использовани€ в паттерне Command.
    /// </summary>
    public interface IControlUnitCommands
    {
        /// <summary>
        /// «апускает операцию автопилота.
        /// </summary>
        void StartOperation(Coordinates targetPosition, FieldBoundaries fieldBoundaries, ImplementType implementType);

        /// <summary>
        /// ќстанавливает операцию автопилота.
        /// </summary>
        void StopOperation();
    }
}