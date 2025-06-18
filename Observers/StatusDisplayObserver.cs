using System; 
using Traktor.Interfaces; 
using Traktor.Core;       

namespace Traktor.Observers
{
    /// <summary>
    /// ���������� �����������, ������� "����������" (��������) ������ ��� ��������� �� ��������.
    /// </summary>
    public class StatusDisplayObserver : IObserver
    {
        private const string SourceFilePath = "Observers/StatusDisplayObserver.cs";
        private readonly string _observerName;

        /// <summary>
        /// �������������� ����� ��������� �����������.
        /// </summary>
        /// <param name="name">��� ����������� (��� ������������� � �����).</param>
        public StatusDisplayObserver(string name)
        {
            _observerName = name ?? throw new ArgumentNullException(nameof(name));
            Logger.Instance.Info(SourceFilePath, $"����������� '{_observerName}' ������.");
        }

        /// <summary>
        /// �����, ���������� ��������� ��� ����������.
        /// �������� ���������� ���������.
        /// </summary>
        /// <param name="subject">������ �� �������� (� ������ ������ �� ������������ �������� � ������ Update).</param>
        /// <param name="message">��������� �� ��������.</param>
        public void Update(ISubject subject, string message)
        {
            string subjectName = subject != null ? subject.GetType().Name : "����������� �������";
            Logger.Instance.Info(SourceFilePath, $"����������� '{_observerName}': �������� ����������� �� '{subjectName}'. ���������: '{message}'");
            // ����� ����� �� ���� ������ ���������� UI ��� ������ ��������
        }
    }
}