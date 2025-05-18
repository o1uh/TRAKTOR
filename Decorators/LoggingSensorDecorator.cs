using Traktor.Interfaces; // ��� ISensors<T>
using Traktor.Core;       // ��� Logger

namespace Traktor.Decorators
{
    /// <summary>
    /// ���������� ���������, ����������� ����������� �� � ����� ������ GetData()
    /// � ���������� �������.
    /// </summary>
    /// <typeparam name="T">��� ������, ������������ ��������.</typeparam>
    public class LoggingSensorDecorator<T> : SensorDecoratorBase<T>
    {
        private const string SourceFilePath = "Decorators/LoggingSensorDecorator.cs"; // ���� ��� �������

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="LoggingSensorDecorator{T}"/>.
        /// </summary>
        /// <param name="wrappedSensor">������, ������� ����� ����������� ������������.</param>
        public LoggingSensorDecorator(ISensors<T> wrappedSensor) : base(wrappedSensor)
        {
            // _wrappedSensor ��� �������� �� null � ������� ������������.
            Logger.Instance.Info(SourceFilePath, $"LoggingSensorDecorator<{typeof(T).Name}> ������. ����������� ������ ����: '{_wrappedSensor.GetType().FullName}'.");
        }

        /// <summary>
        /// �������� ������ �� ���������� �������, �������� �����������.
        /// </summary>
        /// <returns>������ �� �������.</returns>
        public override T GetData()
        {
            Logger.Instance.Debug(SourceFilePath, $"LoggingSensorDecorator<{typeof(T).Name}>: ����� ������� GetData() � ���������� ������� ({_wrappedSensor.GetType().Name}).");

            T data = default(T); // �������������� ��������� �� ���������
            try
            {
                // �������� ����� GetData() �������� ������, �������, � ���� �������,
                // ������� GetData() � _wrappedSensor.
                data = base.GetData();

                Logger.Instance.Debug(SourceFilePath, $"LoggingSensorDecorator<{typeof(T).Name}>: ����� ������ GetData() � ���������� �������. �������� ������: [{data}].");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(SourceFilePath, $"LoggingSensorDecorator<{typeof(T).Name}>: ������ ��� ������ GetData() � ���������� ������� ({_wrappedSensor.GetType().Name}): {ex.Message}", ex);
                throw; // ������������� ����������, ����� �� �������� ��������� ��� ������� ����������
            }
            return data;
        }
    }
}