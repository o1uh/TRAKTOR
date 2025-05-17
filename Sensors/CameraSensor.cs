using Traktor.Interfaces;
using System.Drawing; // Для Bitmap


namespace Traktor.Sensors
{
    public class CameraSensor : ISensors<Bitmap> 
    {
        private int imageCounter = 0;
        private readonly int defaultWidth = 640;  // Уменьшим размеры для простоты генерации
        private readonly int defaultHeight = 480;

        /// <summary>
        /// Имитирует получение изображения с камеры.
        /// </summary>
        /// <returns>Объект <see cref="Bitmap"/> с изображением.</returns>
        public Bitmap GetData()
        {
            imageCounter++;
            // В реальной системе здесь будет код для захвата кадра с камеры.
            // Для макета создаем простое изображение.
            Bitmap bmp = new Bitmap(defaultWidth, defaultHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Заливаем случайным цветом для демонстрации
                Random rnd = new Random();
                g.Clear(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));

                // Рисуем текст
                string text = $"Кадр #{imageCounter} ({DateTime.Now:HH:mm:ss})";
                using (Font font = new Font("Arial", 20))
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    g.DrawString(text, font, brush, 10, 10);
                }
            }

            Console.WriteLine($"[CameraSensor]: Сгенерирован Bitmap {bmp.Width}x{bmp.Height}, Кадр #{imageCounter}"); // Временный вывод

            return bmp;
        } 
    }
}