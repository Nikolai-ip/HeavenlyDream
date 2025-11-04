using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using UnityEngine;

namespace _Game.Source.Application.Airplane.Adders
{
    public class FinComponentController: PlaneComponentController
    {
        [field: SerializeField] public float CenterOffsetVertical { get; set; }
        public override void TryAdd()
        {
            PlaneComponentContainer.AddComponent(new AirplaneFinComponent(CenterOffsetVertical), PlaneComponentQuantityMode.Single);
        }

        public override void TryAdjustComponentParams()
        {
            if (PlaneComponentContainer.TryGetComponent(out AirplaneFinComponent finComponent))
            {
                finComponent.CenterOffsetVertical = CenterOffsetVertical;
            }
        }
    }
}