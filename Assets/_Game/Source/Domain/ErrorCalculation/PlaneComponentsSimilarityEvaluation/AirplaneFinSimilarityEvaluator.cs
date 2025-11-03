using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.ErrorCalculation.PlaneComponentsSimilarityEvaluation
{
    public class AirplaneFinSimilarityEvaluator: IPlaneComponentSimilarityEvaluator<AirplaneFinComponent>
    {
        private readonly float _maxCenterOffsetXDistance;
        private readonly float _centerOffsetXSimilarityGrade;
        private readonly float _absentOfComponentValue;

        public AirplaneFinSimilarityEvaluator(float maxCenterOffsetXDistance, float centerOffsetXSimilarityGrade, float absentOfComponentValue)
        {
            _maxCenterOffsetXDistance = maxCenterOffsetXDistance;
            _centerOffsetXSimilarityGrade = centerOffsetXSimilarityGrade;
            _absentOfComponentValue = absentOfComponentValue;
        }

        public float EvaluateErrorValue(AirplaneFinComponent rc, AirplaneFinComponent c)
        {
            float error = SimilarityValueUtility.CalcError(rc.CenterOffsetVertical,  c.CenterOffsetVertical, 
                _centerOffsetXSimilarityGrade, _maxCenterOffsetXDistance);
            
            return error;
        }

        public float EvaluateInAbsentOfComparableErrorValue()
        {
            return _absentOfComponentValue;
        }
    }
}