// Interfaces/ISensors.cs
namespace TractorAutopilot.Interfaces
{
    public interface ISensors<T>
    {
        /// <summary>
        /// �������� ������ � �������.
        /// </summary>
        /// <returns>������ � �������.</returns>
        T GetData();
    }
}