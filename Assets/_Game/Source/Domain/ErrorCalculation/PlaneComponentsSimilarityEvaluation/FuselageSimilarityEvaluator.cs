using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.ErrorCalculation.PlaneComponentsSimilarityEvaluation
{
    public class FuselageSimilarityEvaluator: IPlaneComponentSimilarityEvaluator<FuselageComponent>
    {
        private readonly float _maxPolishingDegreeValue;
		private readonly float _errorDegree;
        private readonly float _absentOfComponentValue;

        public FuselageSimilarityEvaluator(float maxPolishingDegreeValue, float errorDegree, float absentOfComponentValue)
        {
            _maxPolishingDegreeValue = maxPolishingDegreeValue;
            _errorDegree = errorDegree;
            _absentOfComponentValue = absentOfComponentValue;
        }

        public float EvaluateErrorValue(FuselageComponent @ref, FuselageComponent comp)
        {
            float error = SimilarityValueUtility.CalcError(@ref.PolishingDegree, comp.PolishingDegree, 
                _errorDegree, _maxPolishingDegreeValue);
            return error;
        }

        public float EvaluateInAbsentOfComparableErrorValue()
        {
            return _absentOfComponentValue;
        }
    }
}