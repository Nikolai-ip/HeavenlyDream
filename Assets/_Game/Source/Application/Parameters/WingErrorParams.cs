using System;
using UnityEngine;

namespace _Game.Source.Domain.Parameters
{
    [Serializable]
    public struct WingErrorParams
    {
        [SerializeField] private float _wingMountainSpaceSize ;
        [SerializeField] private float _offsetVerticalSimilarityFuncDegree;
        [SerializeField] private float _maxVerticalDistance;
        [SerializeField] private float _maxHorizontalDistance;
        [SerializeField] private float _offsetHorizontalSimilarityDegree;
        [SerializeField] private float _absentOfComponentValue;
        [SerializeField] private float _weight;

        public float WingMountainSpaceSize => _wingMountainSpaceSize;

        public float OffsetVerticalSimilarityFuncDegree => _offsetVerticalSimilarityFuncDegree;

        public float MaxVerticalDistance => _maxVerticalDistance;

        public float OffsetHorizontalSimilarityDegree => _offsetHorizontalSimilarityDegree;

        public float AbsentOfComponentValue => _absentOfComponentValue;

        public float MaxHorizontalDistance => _maxHorizontalDistance;

        public float Weight => _weight;
    }
}