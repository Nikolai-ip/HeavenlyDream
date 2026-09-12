using UnityEngine;

namespace _Game.Source.Domain.Plane.Component
{
    public class PlaneComponent
    {
        public Vector3 LocalPosition;
        public Quaternion LocalRotation;
        public AirplaneComponentType Type { get; }

        public PlaneComponent(Vector3 localPosition, Quaternion localRotation, AirplaneComponentType type)
        {
            LocalPosition = localPosition;
            LocalRotation = localRotation;
            Type = type;
        }
    }
}