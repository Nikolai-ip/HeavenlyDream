using System;
using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Source.Application.Airplane.Adders
{
    public class FuselageComponentAdjuster: PlaneComponentAdjuster
    {
        [field: SerializeField] [field:ReadOnly] public float PolishDegree { get; set; }

        public override void TryAdd()
        {
            PlaneComponentId = PlaneComponentContainer.AddComponent(new FuselageComponent(PolishDegree));
        }

        public override void TryAdjustComponentParams()
        {
            if (PlaneComponentContainer.TryGetComponent<FuselageComponent>(PlaneComponentId, out var fuselageComponent))
            {
                fuselageComponent.PolishingDegree =  PolishDegree;
            }
        }
    }
}