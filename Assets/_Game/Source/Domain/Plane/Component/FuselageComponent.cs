namespace _Game.Source.Domain.Plane.Component
{
    public class FuselageComponent:  PlaneComponent
    {
        public float PolishingDegree { get; set; }

        public FuselageComponent(float polishingDegree)
        {
            PolishingDegree = polishingDegree;
        }
    }
}