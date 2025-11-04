using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using UnityEngine;

namespace _Game.Source.Application.Airplane.Adders
{
    public class WingComponentController: PlaneComponentController
    {
        [field:SerializeField] public float CenterOffsetVertical { get; set; }

        [field:SerializeField] public float CenterOffsetHorizontal { get; set; }

        public override void TryAdd()
        {
            PlaneComponentContainer.AddComponent(new WingComponent(CenterOffsetVertical, CenterOffsetHorizontal), PlaneComponentQuantityMode.Single);
        }

        public override void TryAdjustComponentParams()
        {
            if (PlaneComponentContainer.TryGetComponent(out WingComponent wingComponent))
            {
                wingComponent.CenterOffsetVertical = CenterOffsetVertical;
                wingComponent.CenterOffsetHorizontal = CenterOffsetHorizontal;
            }
        }
    }
}