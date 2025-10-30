
using System;

namespace _Game.Source.Domain.ErrorCalculation.PlaneComponentsSimilarityEvaluation
{
    public static class SimilarityValueUtility
    {
        public static float CalcError(float aRef, float a, float errorPower, float aMax)
        {
            float error = (float)Math.Pow(Math.Abs(aRef - a), errorPower) / aMax;
            error = Math.Clamp(error, 0, 1);
            return error;
        }
    }
}