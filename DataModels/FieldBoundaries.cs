namespace Traktor.DataModels 
{
    public class FieldBoundaries
    {
        public List<Coordinates> Vertices { get; private set; }

        public FieldBoundaries(List<Coordinates> vertices)
        {
            Vertices = vertices ?? new List<Coordinates>();
        }
    }
}