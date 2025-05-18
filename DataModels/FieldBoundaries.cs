namespace Traktor.DataModels
{
    /// <summary>
    /// Представляет границы поля, определенные набором вершин (координат).
    /// Предполагается, что вершины задают многоугольник.
    /// </summary>
    public class FieldBoundaries
    {
        /// <summary>
        /// Список вершин, определяющих границы поля.
        /// </summary>
        public List<Coordinates> Vertices { get; private set; } 

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FieldBoundaries"/>.
        /// </summary>
        /// <param name="vertices">Список координат, представляющих вершины границ поля.
        /// Если передано null, будет создан пустой список.</param>
        public FieldBoundaries(List<Coordinates> vertices)
        {
            Vertices = vertices ?? new List<Coordinates>();
        }
        // Можно добавить методы, если потребуется, например, для проверки, находится ли точка внутри границ.
    }
}