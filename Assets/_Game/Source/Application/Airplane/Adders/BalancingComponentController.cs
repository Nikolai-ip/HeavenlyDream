using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Source.Application.Airplane.Adders
{
    public class BalancingComponentAdjuster: PlaneComponentAdjuster
    {
        [field: SerializeField] [field:ReadOnly] public float WeightAmount { get; set; }
        
        public override void TryAdd()
        {
            PlaneComponentId = PlaneComponentContainer.AddComponent(new BalancingWeightComponent(WeightAmount));
        }
        
    }
}