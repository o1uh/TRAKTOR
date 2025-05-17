namespace Traktor.Interfaces
{
    public interface ISensors<T>
    {
        /// <summary>
        /// �������� ������ � �������.
        /// </summary>
        /// <returns>������ ������� ���������� ���� T.</returns>
        T GetData();
    }
}