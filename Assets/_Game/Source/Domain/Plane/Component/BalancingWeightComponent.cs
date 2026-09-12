using UnityEngine;

namespace _Game.Source.Domain.Plane.Component
{
    public class BalancingWeightComponent: PlaneComponent
    {
        public float WeightAmount { get; set; }

        public BalancingWeightComponent(float weightAmount) : base(Vector3.zero, Quaternion.identity, AirplaneComponentType.Balancing)
        {
            WeightAmount = weightAmount;
        }
    }
}