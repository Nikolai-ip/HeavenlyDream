using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using UnityEngine;

namespace _Game.Source.Application.Airplane.Adders
{
    public class FinComponentAdjuster: PlaneComponentAdjuster
    {
        [field: SerializeField] public float CenterOffsetVertical { get; set; }
        public override void TryAdd()
        {
            PlaneComponentContainer.AddComponent(new AirplaneFinComponent(CenterOffsetVertical), PlaneComponentQuantityMode.Single);
        }
    }
}