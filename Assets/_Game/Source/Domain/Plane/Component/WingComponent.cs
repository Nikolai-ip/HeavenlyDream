namespace _Game.Source.Domain.Plane.Component
{
    public class WingComponent: PlaneComponent
    {
        public float MountingDistance { get; }

        public float CenterOffsetX { get; }

        public WingComponent(float mountingDistance, float centerOffsetX)
        {
            MountingDistance = mountingDistance;
            CenterOffsetX = centerOffsetX;
        }
    }
}