using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application.Airplane.Adders
{
    public class FuselageComponentController: PlaneComponentController
    {
        [Inject] private PlaneComponentContainer _planeComponentContainer;
        [field: SerializeField] public float PolishDegree { get; set; }

        public override void TryAdd()
        {
            _planeComponentContainer.AddComponent(new FuselageComponent(PolishDegree), PlaneComponentQuantityMode.Single);
        }

        public override void TryAdjustComponentParams()
        {
            if (_planeComponentContainer.TryGetComponent<FuselageComponent>(out var fuselageComponent))
            {
                fuselageComponent.PolishingDegree =  PolishDegree;
            }
        }
    }
}