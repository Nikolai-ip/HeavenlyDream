using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application.Airplane.Adders
{
    public class FuselageComponentController: PlaneComponentController
    {

        [field: SerializeField] public float PolishDegree { get; set; }

        public override void TryAdd()
        {
            PlaneComponentContainer.AddComponent(new FuselageComponent(PolishDegree), PlaneComponentQuantityMode.Single);
        }

        public override void TryAdjustComponentParams()
        {
            if (PlaneComponentContainer.TryGetComponent<FuselageComponent>(out var fuselageComponent))
            {
                fuselageComponent.PolishingDegree =  PolishDegree;
            }
        }
    }
}