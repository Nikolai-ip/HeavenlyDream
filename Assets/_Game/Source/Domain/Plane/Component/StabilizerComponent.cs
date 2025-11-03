namespace _Game.Source.Domain.Plane.Component
{
    public class StabilizerComponent: PlaneComponent
    {
        public float CenterOffsetHorizontal { get; }

        public StabilizerComponent(float centerOffsetHorizontal)
        {
            CenterOffsetHorizontal = centerOffsetHorizontal;
        }
    }
}