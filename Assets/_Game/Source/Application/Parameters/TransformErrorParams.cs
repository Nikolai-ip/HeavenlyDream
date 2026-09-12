using System;
using UnityEngine;

namespace _Game.Source.Domain.Parameters
{
    [Serializable]
    public struct TransformErrorParams
    {
        [SerializeField] private float _errorPower;
        [SerializeField] private float maxDistanceDiff;
        [SerializeField] private float _maxRotationDiff;
        [SerializeField] private float _absentOfComponentValue;
        [SerializeField] private float _weight;

        public float ErrorPower => _errorPower;

        public float MaxDistanceDiff => maxDistanceDiff;

        public float MaxRotationDiff => _maxRotationDiff;

        public float AbsentOfComponentValue => _absentOfComponentValue;

        public float Weight => _weight;
    }
}