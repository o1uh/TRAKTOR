using Traktor.Interfaces;
using System.Drawing; // ��� Bitmap


namespace Traktor.Sensors
{
    public class CameraSensor : ISensors<Bitmap> 
    {
        private int imageCounter = 0;
        private readonly int defaultWidth = 640;  // �������� ������� ��� �������� ���������
        private readonly int defaultHeight = 480;

        /// <summary>
        /// ��������� ��������� ����������� � ������.
        /// </summary>
        /// <returns>������ <see cref="Bitmap"/> � ������������.</returns>
        public Bitmap GetData()
        {
            imageCounter++;
            // � �������� ������� ����� ����� ��� ��� ������� ����� � ������.
            // ��� ������ ������� ������� �����������.
            Bitmap bmp = new Bitmap(defaultWidth, defaultHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // �������� ��������� ������ ��� ������������
                Random rnd = new Random();
                g.Clear(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));

                // ������ �����
                string text = $"���� #{imageCounter} ({DateTime.Now:HH:mm:ss})";
                using (Font font = new Font("Arial", 20))
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    g.DrawString(text, font, brush, 10, 10);
                }
            }

            Console.WriteLine($"[CameraSensor]: ������������ Bitmap {bmp.Width}x{bmp.Height}, ���� #{imageCounter}"); // ��������� �����

            return bmp;
        } 
    }
}