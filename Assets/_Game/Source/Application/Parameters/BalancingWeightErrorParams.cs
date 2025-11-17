using System;
using UnityEngine;

namespace _Game.Source.Domain.Parameters
{
    [Serializable]
    public class BalancingWeightErrorParams
    {
        [SerializeField] private float _maxWeightAmount;
        [SerializeField] private float _evaluateSimilarityFuncDegree = 2;
        [SerializeField] private float _absentOfComponentValue = 0.2f;
        [SerializeField] private float _weight = 1f;

        public float MaxWeightAmount => _maxWeightAmount;

        public float EvaluateSimilarityFuncDegree => _evaluateSimilarityFuncDegree;

        public float AbsentOfComponentValue => _absentOfComponentValue;

        public float Weight => _weight;
    }
}