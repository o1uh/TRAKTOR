using Traktor.Interfaces;
using System.Drawing; // ��� Bitmap, Graphics, Color, Font, SolidBrush
using Traktor.Core;   // ��������� ��� Logger

namespace Traktor.Sensors
{
    /// <summary>
    /// ��������� ������ ������, ��������� ����������� Bitmap.
    /// </summary>
    public class CameraSensor : ISensors<Bitmap>
    {
        private int _imageCounter = 0;
        private readonly int _defaultWidth = 640;
        private readonly int _defaultHeight = 480;
        private static readonly Random _random = new Random(); // ��� ���������� ����� ����
        private const string SourceFilePath = "Sensors/CameraSensor.cs"; // ���������� ��������� ��� ���� �����

        /// <summary>
        /// �������������� ����� ��������� ������ <see cref="CameraSensor"/>.
        /// </summary>
        public CameraSensor()
        {
            // ���������� Logger.Instance.Info ��� ����������� �������������
            Logger.Instance.Info(SourceFilePath, $"������ ������ (���������) ��������������� � ����������� {_defaultWidth}x{_defaultHeight}.");
        }

        /// <summary>
        /// ��������� ��������� ����������� � ������.
        /// </summary>
        /// <returns>������ <see cref="Bitmap"/> � �������������� ������������.</returns>
        public Bitmap GetData()
        {
            _imageCounter++;
            // � �������� ������� ����� ����� ��� ��� ������� ����� � ������.
            // ��� ������ ������� ������� �����������.
            Bitmap bmp = new Bitmap(_defaultWidth, _defaultHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // �������� ��������� ������ ��� ������������
                g.Clear(Color.FromArgb(_random.Next(256), _random.Next(256), _random.Next(256)));

                // ������ �����
                string text = $"���� #{_imageCounter} ({DateTime.Now:HH:mm:ss.fff})"; // DateTime ����� ��� ������������
                using (Font font = new Font("Arial", 20)) // ����� Arial ����� ������������� � ��������� ��������, ����� �������� �� ����� �������������
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    g.DrawString(text, font, brush, 10, 10);
                }
            }
            // ���������� Logger.Instance.Debug ��� ����������� ��������� Bitmap, �.�. ��� ����� ���� ������ ��������
            Logger.Instance.Debug(SourceFilePath, $"������������ Bitmap {bmp.Width}x{bmp.Height}, ���� #{_imageCounter}");
            return bmp;
        }
    }
}