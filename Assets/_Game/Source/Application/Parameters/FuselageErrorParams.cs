using System;
using UnityEngine;

namespace _Game.Source.Domain.Parameters
{
    [Serializable]
    public struct FuselageErrorParams
    {
        [SerializeField] private float _maxPolishingDegreeValue;
        [SerializeField] private float _evaluateSimilarityFuncDegree;
        [SerializeField] private float _absentOfComponentValue;
        [SerializeField] private float _weight;

        public float MaxPolishingDegreeValue => _maxPolishingDegreeValue;

        public float EvaluateSimilarityFuncDegree => _evaluateSimilarityFuncDegree;

        public float AbsentOfComponentValue => _absentOfComponentValue;

        public float Weight => _weight;
    }
}