using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.Plane.Validation.Predicates
{
    public class PlaneNotContain: PlaneContainComponentPredicate
    {
        public PlaneNotContain(PlaneComponentContainer plane, AirplaneComponentType type) : base(plane, type)
        { }
        public override bool Evaluate() => !PlaneContainComponent();
    }
}