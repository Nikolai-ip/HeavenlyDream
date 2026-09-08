using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Source.Application.Airplane.Adders
{
    public class WingComponentAdjuster: PlaneComponentAdjuster
    {
        [field:SerializeField] [field:ReadOnly] public float CenterOffsetVertical { get; set; }
        [field:SerializeField] [field:ReadOnly] public float CenterOffsetHorizontal { get; set; }
        [field:SerializeField] [field:ReadOnly] public Vector3 LocalPosition { get; set; }

        public override void TryAdd()
        {
            PlaneComponentContainer.AddComponent(new WingComponent(CenterOffsetVertical, CenterOffsetHorizontal), PlaneComponentQuantityMode.Single);
        }
    }
}