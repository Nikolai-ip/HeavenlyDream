using System;
using _Game.Source.Domain.Plane.Component;
using UnityEngine;

namespace _Game.Source.Domain.ErrorCalculation.PlaneComponentsSimilarityEvaluation
{
    public class TransformSimilarityEvaluator : IPlaneComponentSimilarityEvaluator<PlaneComponent>
    {
        private readonly float _errorPower;
        private readonly float _maxDistanceDiff;
        private readonly float _absentOfComponentValue;
        private float _maxRotationDiff;
        

        public TransformSimilarityEvaluator(float errorPower, float maxDistanceDiff, float maxRotationDiff, float absentOfComponentValue)
        {
            _errorPower = errorPower;
            _maxDistanceDiff = maxDistanceDiff;
            _maxRotationDiff = maxRotationDiff;
            _absentOfComponentValue = absentOfComponentValue;
        }

        public float EvaluateErrorValue(PlaneComponent @ref, PlaneComponent comp)
        {
            float positionError = Get3DimensionError(
                new ValueTuple<float, float>(@ref.LocalPosition.x, comp.LocalPosition.x),
                new ValueTuple<float, float>(@ref.LocalPosition.y, comp.LocalPosition.y),
                new ValueTuple<float, float>(@ref.LocalPosition.z, comp.LocalPosition.z),
                _maxDistanceDiff);

            Vector3 refRot = @ref.LocalRotation.eulerAngles;
            Vector3 compRot = comp.LocalRotation.eulerAngles;
            
            float rotationError = Get3DimensionError(
                new ValueTuple<float, float>(@refRot.x, compRot.x),
                new ValueTuple<float, float>(refRot.y, compRot.y),
                new ValueTuple<float, float>(refRot.z, compRot.z),
                _maxRotationDiff);
            
            float totalError = positionError + rotationError;
            return Math.Clamp(totalError, 0, 1);
        }

        private float Get3DimensionError((float,float) a, (float,float) b, (float,float) c, float max)
        {
            float aError = SimilarityValueUtility.CalcError(a.Item1,
                a.Item2, _errorPower, max);
            float bError = SimilarityValueUtility.CalcError(b.Item1,
                b.Item2, _errorPower, max);
            float cError = SimilarityValueUtility.CalcError(c.Item1,
                c.Item2, _errorPower, max);
            
            return (aError + bError + cError) / 3;
        }

        public float EvaluateInAbsentOfComparableErrorValue()
        {
            return _absentOfComponentValue;
        }
    }
}