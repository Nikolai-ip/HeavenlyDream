using System;
using UnityEngine;

namespace _Game.Source.Domain.Parameters
{
    [Serializable]
    public struct StabilizerErrorParams
    {
        [SerializeField] private float _maxCenterOffsetHorizontal;
        [SerializeField] private float _evaluateSimilarityFuncDegree;
        [SerializeField] private float _absentOfComponentValue;
        [SerializeField] private float _weight;

        public float MaxCenterOffsetHorizontal => _maxCenterOffsetHorizontal;

        public float EvaluateSimilarityFuncDegree => _evaluateSimilarityFuncDegree;

        public float AbsentOfComponentValue => _absentOfComponentValue;

        public float Weight => _weight;
    }
}