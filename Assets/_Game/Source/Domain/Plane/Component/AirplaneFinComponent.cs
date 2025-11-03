namespace _Game.Source.Domain.Plane.Component
{
    public class AirplaneFinComponent : PlaneComponent
    {
        public float CenterOffsetVertical { get; }

        public AirplaneFinComponent(float centerOffsetVertical)
        {
            CenterOffsetVertical = centerOffsetVertical;
        }
    }
}