using Traktor.Interfaces;
using System.Drawing; // Для Bitmap, Graphics, Color, Font, SolidBrush
using Traktor.Core;   // Добавлено для Logger

namespace Traktor.Sensors
{
    /// <summary>
    /// Имитирует работу камеры, генерируя изображения Bitmap.
    /// </summary>
    public class CameraSensor : ISensors<Bitmap>
    {
        private int _imageCounter = 0;
        private readonly int _defaultWidth = 640;
        private readonly int _defaultHeight = 480;
        private static readonly Random _random = new Random(); // Для случайного цвета фона
        private const string SourceFilePath = "Sensors/CameraSensor.cs"; // Определяем константу для пути файла

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CameraSensor"/>.
        /// </summary>
        public CameraSensor()
        {
            // Используем Logger.Instance.Info для логирования инициализации
            Logger.Instance.Info(SourceFilePath, $"Датчик камеры (симуляция) инициализирован с разрешением {_defaultWidth}x{_defaultHeight}.");
        }

        /// <summary>
        /// Имитирует получение изображения с камеры.
        /// </summary>
        /// <returns>Объект <see cref="Bitmap"/> с симулированным изображением.</returns>
        public Bitmap GetData()
        {
            _imageCounter++;
            // В реальной системе здесь будет код для захвата кадра с камеры.
            // Для макета создаем простое изображение.
            Bitmap bmp = new Bitmap(_defaultWidth, _defaultHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Заливаем случайным цветом для демонстрации
                g.Clear(Color.FromArgb(_random.Next(256), _random.Next(256), _random.Next(256)));

                // Рисуем текст
                string text = $"Кадр #{_imageCounter} ({DateTime.Now:HH:mm:ss.fff})"; // DateTime здесь уже используется
                using (Font font = new Font("Arial", 20)) // Шрифт Arial может отсутствовать в некоторых системах, можно заменить на более универсальный
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    g.DrawString(text, font, brush, 10, 10);
                }
            }
            // Используем Logger.Instance.Debug для логирования генерации Bitmap, т.к. это может быть частым событием
            Logger.Instance.Debug(SourceFilePath, $"Сгенерирован Bitmap {bmp.Width}x{bmp.Height}, Кадр #{_imageCounter}");
            return bmp;
        }
    }
}