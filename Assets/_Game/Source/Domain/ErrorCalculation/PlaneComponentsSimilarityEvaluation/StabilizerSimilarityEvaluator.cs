using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.ErrorCalculation.PlaneComponentsSimilarityEvaluation
{
    public class StabilizerSimilarityEvaluator: IPlaneComponentSimilarityEvaluator<StabilizerComponent>
    {
        private readonly float _maxCenterOffsetX;
        private readonly float _errorPower;
        private readonly float _absentOfComponentValue;

        public StabilizerSimilarityEvaluator(float maxCenterOffsetX, float errorPower, float absentOfComponentValue)
        {
            _absentOfComponentValue = absentOfComponentValue;
            _maxCenterOffsetX = maxCenterOffsetX;
            _errorPower = errorPower;
        }

        public float EvaluateErrorValue(StabilizerComponent rc, StabilizerComponent c)
        {
            float error =
                SimilarityValueUtility.CalcError(rc.CenterOffsetHorizontal, c.CenterOffsetHorizontal, _errorPower, _maxCenterOffsetX);
            return error;
        }

        public float EvaluateInAbsentOfComparableErrorValue()
        {
            return _absentOfComponentValue;
        }
    }
}