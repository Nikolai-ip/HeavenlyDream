namespace _Game.Source.Domain.Plane.Component
{
    public class WingComponent: PlaneComponent
    {
        public float CenterOffsetVertical { get; set; }

        public float CenterOffsetHorizontal { get; set; }

        public WingComponent(float centerOffsetVertical, float centerOffsetHorizontal)
        {
            CenterOffsetVertical = centerOffsetVertical;
            CenterOffsetHorizontal = centerOffsetHorizontal;
        }
    }
}