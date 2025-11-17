using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using UnityEngine;

namespace _Game.Source.Application.Airplane.Adders
{
    public class StabilizerComponentController: PlaneComponentController
    {
        [field: SerializeField] public float CenterOffsetHorizontal { get; set; }
        public override void TryAdd()
        {
            PlaneComponentContainer.AddComponent(new StabilizerComponent(CenterOffsetHorizontal), PlaneComponentQuantityMode.Single);
        }
    }
}