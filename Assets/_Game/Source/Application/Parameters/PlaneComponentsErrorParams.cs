using System;
using UnityEngine;

namespace _Game.Source.Domain.Parameters
{
    [Serializable]
    public class PlaneComponentsErrorParams
    {
        [field: SerializeField] public FuselageErrorParams FuselageErrorParams { get; private set; }
        [field: SerializeField] public WingErrorParams WingErrorParams { get; private set; }
        [field: SerializeField] public StabilizerErrorParams StabilizerErrorParams { get; private set; }
        [field: SerializeField] public FinErrorParams FinErrorParams { get; private set; }
        [field: SerializeField] public BalancingWeightErrorParams BalancingWeightErrorParams { get; private set; }
    }
}