namespace Traktor.Interfaces
{
    /// <summary>
    /// Интерфейс для всех команд, инкапсулирующих действие.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Выполняет действие, инкапсулированное командой.
        /// </summary>
        void Execute();
    }
}