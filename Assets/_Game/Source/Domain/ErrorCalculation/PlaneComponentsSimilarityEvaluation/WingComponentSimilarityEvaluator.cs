using System;
using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.ErrorCalculation.PlaneComponentsSimilarityEvaluation
{
    class WingComponentSimilarityEvaluator : IPlaneComponentSimilarityEvaluator<WingComponent>
    {
        private readonly WingErrorEvaluateParams _wingErrorEvaluateParams;

        public WingComponentSimilarityEvaluator(WingErrorEvaluateParams wingErrorEvaluateParams)
        {
            _wingErrorEvaluateParams = wingErrorEvaluateParams;
        }

        public float EvaluateErrorValue(WingComponent rc, WingComponent c)
        {
            float mountError = SimilarityValueUtility.CalcError(rc.CenterOffsetVertical, c.CenterOffsetVertical,
                _wingErrorEvaluateParams.MountainSimilarityGrade, _wingErrorEvaluateParams.MaxMountainDistance);
                
            float offsetXError = SimilarityValueUtility.CalcError(rc.CenterOffsetHorizontal,  c.CenterOffsetHorizontal, 
                _wingErrorEvaluateParams.OffsetXSimilarityGrade, _wingErrorEvaluateParams.MaxOffsetXDistance);
            
            float similarity = mountError +  offsetXError;
            return Math.Clamp(similarity, 0, 1);
        }

        public float EvaluateInAbsentOfComparableErrorValue() => _wingErrorEvaluateParams.AbsentOfComponentValue;
    }

    class WingErrorEvaluateParams
    {
        public float MaxMountainDistance { get; }
        public float MountainSimilarityGrade { get; }
        public float MaxOffsetXDistance { get; }
        public float OffsetXSimilarityGrade { get; }
        public float AbsentOfComponentValue { get; }

        public WingErrorEvaluateParams(float maxMountainDistance, float mountainSimilarityGrade, float maxOffsetXDistance, float offsetXSimilarityGrade, float absentOfComponentValue)
        {
            MaxMountainDistance = maxMountainDistance;
            MountainSimilarityGrade = mountainSimilarityGrade;
            MaxOffsetXDistance = maxOffsetXDistance;
            OffsetXSimilarityGrade = offsetXSimilarityGrade;
            AbsentOfComponentValue = absentOfComponentValue;
        }
    }
}