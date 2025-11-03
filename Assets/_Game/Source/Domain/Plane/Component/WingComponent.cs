namespace _Game.Source.Domain.Plane.Component
{
    public class WingComponent: PlaneComponent
    {
        public float CenterOffsetVertical { get; }

        public float CenterOffsetHorizontal { get; }

        public WingComponent(float centerOffsetVertical, float centerOffsetHorizontal)
        {
            CenterOffsetVertical = centerOffsetVertical;
            CenterOffsetHorizontal = centerOffsetHorizontal;
        }
    }
}