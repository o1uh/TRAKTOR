using System.Collections.Generic;

namespace TractorAutopilot.DataModels
{
    /// <summary>
    /// ������������ ������� ��������.
    /// </summary>
    public class Route
    {
        /// <summary>
        /// �������� ��� ������ ������ ������� ����� ��������.
        /// </summary>
        public List<Vector2> Waypoints { get; set; } = new List<Vector2>();

        // ����� �������� ������ ��������, ��������, ������������� ��������, ��� � �.�.

        /// <summary>
        /// ���������� ��������� ������������� ��������.
        /// </summary>
        /// <returns>������, ����������� �������.</returns>
        public override string ToString() => $"������� � {Waypoints.Count} �������� �������.";
    }
}