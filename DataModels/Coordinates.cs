namespace Traktor.DataModels
{
    /// <summary>
    /// ������������ �������������� ����������.
    /// </summary>
    public struct Coordinates
    {
        /// <summary>
        /// ������.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// �������.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// �������������� ����� ��������� ��������� <see cref="Coordinates"/>.
        /// </summary>
        /// <param name="latitude">������.</param>
        /// <param name="longitude">�������.</param>
        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// ���������� ��������� ������������� ���������.
        /// </summary>
        /// <returns>������ � ������� � ��������.</returns>
        public override string ToString()
        {
            return $"������: {Latitude:F6}, �������: {Longitude:F6}";
        }
    }
}