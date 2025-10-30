namespace _Game.Source.Domain.Plane.Component
{
    public class StabilizerComponent: PlaneComponent
    {
        public float CenterOffsetX { get; }

        public StabilizerComponent(float centerOffsetX)
        {
            CenterOffsetX = centerOffsetX;
        }
    }
}