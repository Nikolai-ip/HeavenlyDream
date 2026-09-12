using UnityEngine;

namespace _Game.Source.Domain.Plane.Component
{
    public class FuselageComponent:  PlaneComponent
    {
        public float PolishingDegree { get; set; }

        public FuselageComponent(float polishingDegree): base(Vector3.zero, Quaternion.identity, AirplaneComponentType.Fuselage)
        {
            PolishingDegree = polishingDegree;
        }
    }
}