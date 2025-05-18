using System.Collections; // ��� IEnumerable
using Traktor.DataModels; // ��� Coordinates
using Traktor.Core;       // ��� Logger

namespace Traktor.Navigation // ��� Traktor.DataModels, ��� Traktor.Core, ��� ��� ������� ��� ������
{
    /// <summary>
    /// ������������ ������� �������� ��� ��������� ���������,
    /// ������������ ������� �������� ����� ���������� IEnumerable<Coordinates>.
    /// </summary>
    public class TractorRoute : IEnumerable<Coordinates>
    {
        private readonly List<Coordinates> _points;
        private const string SourceFilePath = "Navigation/TractorRoute.cs"; // ������� ���������� ����

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="TractorRoute"/>.
        /// </summary>
        /// <param name="points">������ ���������, ������������ �������.</param>
        public TractorRoute(List<Coordinates> points)
        {
            _points = points ?? new List<Coordinates>(); // ���� null, ������� ������ ������
            Logger.Instance.Info(SourceFilePath, $"TractorRoute ������. ���������� ����� � ��������: {_points.Count}.");
        }

        /// <summary>
        /// ���������� �������������, ������� ������������ �������� �� ��������� ���������.
        /// </summary>
        /// <returns>������������� ��� ��������� <see cref="Coordinates"/>.</returns>
        public IEnumerator<Coordinates> GetEnumerator()
        {
            Logger.Instance.Debug(SourceFilePath, "GetEnumerator<Coordinates>() ������. ������ �� �������� �� ������ ��������.");
            // ���������� ��������� ��������� ������������ ������.
            // � ����� ������� ������ ����� ����� �� ���� ��������� ������ ��������.
            int pointCounter = 0;
            foreach (var point in _points)
            {
                pointCounter++;
                Logger.Instance.Debug(SourceFilePath, $"��������: ������������ ����� #{pointCounter}: {point}");
                yield return point; // 'yield return' ������ ����� ���������
            }
            Logger.Instance.Debug(SourceFilePath, $"��������: ��� {_points.Count} ����� ���� ���������.");
        }

        /// <summary>
        /// ���������� �������������, ������� ������������ �������� �� ���������. (��� ����������������� IEnumerable)
        /// </summary>
        /// <returns>�������������, ������� ����� ������������ ��� �������� �� ���������.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            Logger.Instance.Debug(SourceFilePath, "IEnumerable.GetEnumerator() ������ (����������������). ������������� ��������������� GetEnumerator.");
            return GetEnumerator();
        }

        // �������������� ����� ��� ������������, ��� ��� �� ������ ������
        public int GetPointCount()
        {
            return _points.Count;
        }
    }
}