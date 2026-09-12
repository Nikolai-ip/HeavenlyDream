using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.ErrorCalculation.PlaneComponentsSimilarityEvaluation
{
    public class BalancingWeightSimilarityEvaluator : IPlaneComponentSimilarityEvaluator<BalancingWeightComponent>
    {
        private readonly float _maxWeightAmount;
        private readonly float _weightGrade;
        private readonly float _absentOfComponentValue;

        public BalancingWeightSimilarityEvaluator(float maxWeightAmount, float weightGrade,
            float absentOfComponentValue)
        {
            _maxWeightAmount = maxWeightAmount;
            _weightGrade = weightGrade;
            _absentOfComponentValue = absentOfComponentValue;
        }

        public float EvaluateErrorValue(BalancingWeightComponent @ref, BalancingWeightComponent comp)
        {
            ;
            var error =
                SimilarityValueUtility.CalcError(@ref.WeightAmount, comp.WeightAmount,
                    _weightGrade, _maxWeightAmount);
            return error;
        }

        public float EvaluateInAbsentOfComparableErrorValue()
        {
            return _absentOfComponentValue;
        }
    }
}