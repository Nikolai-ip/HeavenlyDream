using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using UnityEngine;

namespace _Game.Source.Application.Airplane.Adders
{
    public class BalancingComponentAdjuster: PlaneComponentAdjuster
    {
        [field: SerializeField] public float WeightAmount { get; set; }
        
        public override void TryAdd()
        {
            PlaneComponentContainer.AddComponent(new BalancingWeightComponent(WeightAmount), PlaneComponentQuantityMode.Single);
        }
        
    }
}