using System.ComponentModel;
using _Game.Source.Domain.Plane;

namespace _Game.Source.Domain.ErrorCalculation
{
    public interface IPlaneComponentSimilarityEvaluator<in TPlaneComponent>
    {
        public float EvaluateErrorValue(TPlaneComponent @ref, TPlaneComponent comp);
        public float EvaluateInAbsentOfComparableErrorValue();
    }
}