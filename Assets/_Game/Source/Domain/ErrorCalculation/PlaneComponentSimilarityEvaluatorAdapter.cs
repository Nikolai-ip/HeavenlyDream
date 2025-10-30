using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.ErrorCalculation
{
    class PlaneComponentSimilarityEvaluatorAdapter<T>: IPlaneComponentSimilarityEvaluator<PlaneComponent> where T: PlaneComponent
    {
        private readonly IPlaneComponentSimilarityEvaluator<T> _inner;

        public PlaneComponentSimilarityEvaluatorAdapter(IPlaneComponentSimilarityEvaluator<T> inner)
        {
            _inner = inner;
        }

        public float EvaluateErrorValue(PlaneComponent referenceComponent, PlaneComponent comparableComponent)
        {
            return _inner.EvaluateErrorValue((T) referenceComponent, (T) comparableComponent);
        }

        public float EvaluateInAbsentOfComparableErrorValue() => _inner.EvaluateInAbsentOfComparableErrorValue();
    }
}