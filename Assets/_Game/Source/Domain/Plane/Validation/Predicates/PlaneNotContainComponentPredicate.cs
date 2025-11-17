using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.Plane.Validation.Predicates
{
    public class PlaneNotContainComponentPredicate<TPlaneComponent>:
        PlaneContainComponentPredicate<TPlaneComponent> where TPlaneComponent: PlaneComponent
    {
        public PlaneNotContainComponentPredicate(PlaneComponentContainer plane) : base(plane)
        { }
        public override bool Evaluate() => !PlaneContainComponent();
    }
}