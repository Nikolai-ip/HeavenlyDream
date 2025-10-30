namespace _Game.Source.Domain.Plane.Component
{
    public class AirplaneFinComponent : PlaneComponent
    {
        public float CenterOffsetX { get; }

        public AirplaneFinComponent(float centerOffsetX)
        {
            CenterOffsetX = centerOffsetX;
        }
    }
}