using Traktor.Interfaces; // ��� ISensors<T>

namespace Traktor.Decorators
{
    /// <summary>
    /// ����������� ������� ����� ��� ����������� ��������.
    /// ��������� ISensors<T> � ���������� ������ ���������� �������.
    /// </summary>
    /// <typeparam name="T">��� ������, ������������ ��������.</typeparam>
    public abstract class SensorDecoratorBase<T> : ISensors<T>
    {
        protected readonly ISensors<T> _wrappedSensor;

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="SensorDecoratorBase{T}"/>.
        /// </summary>
        /// <param name="wrappedSensor">������, ������� ����� �����������.</param>
        /// <exception cref="ArgumentNullException">���� wrappedSensor ����� null.</exception>
        protected SensorDecoratorBase(ISensors<T> wrappedSensor)
        {
            _wrappedSensor = wrappedSensor ?? throw new ArgumentNullException(nameof(wrappedSensor));
            // ����������� �������� ����� � ���������� �����������,
            // ��� ��� ��� ����� �������� ��� ����������� ���������� � ��� SourceFilePath.
        }

        /// <summary>
        /// �������� ������ �� ���������� �������.
        /// ����� ���� ������������� � ����������� ������� ��� ���������� ���������.
        /// </summary>
        /// <returns>������ �� �������.</returns>
        public virtual T GetData()
        {
            // � ������� ������ ������ ����������.
            // ���������� ���������� ������� ������ ��/����� ����� ������.
            return _wrappedSensor.GetData();
        }
    }
}