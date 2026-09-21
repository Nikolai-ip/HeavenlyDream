using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Source.Application
{
    public class AirplaneSurface: MonoBehaviour
    {
        [SerializeField] private List<Transform> _attachPoints;
        private BoxCollider _collider;
        public Vector3 Center => _collider.bounds.center;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }
        public Transform GetAttachPoint(int index) => _attachPoints[index];

        public Vector3 ProjectRay(Ray ray)
        {
            Vector3 size = _collider.size;
            Vector3 center = _collider.center;
            var lowCorner = _collider.transform.TransformPoint(center - new Vector3(0, size.y) / 2);
            var upCorner = _collider.transform.TransformPoint(center + new Vector3(0, size.y) / 2);      
            float heightHalf = (upCorner - lowCorner).magnitude / 2;
            Plane plane = new Plane(_collider.transform.up, _collider.transform.position + _collider.transform.up * heightHalf);
            plane.Raycast(ray, out float t);
            var resultPoint = _collider.ClosestPoint(ray.origin +  ray.direction * t);
            return resultPoint;
        }

        public Quaternion AroundAxis(RotationType rotationType, float yaw)
        {
            Quaternion res;
            switch (rotationType)
            {
                case RotationType.Forward:
                    res = GetRotation(transform.forward, transform.up, yaw);
                    break;
                case RotationType.Up:
                    res = GetRotation(transform.up, transform.forward, yaw);
                    break;
                default:
                    throw new ArgumentException("Failed handle rotation with type " + rotationType);
            }
            return res;
        }

        private Quaternion GetRotation(Vector3 a, Vector3 b, float yaw)
        {
            Quaternion rotation = Quaternion.LookRotation(a, b);
            return rotation * Quaternion.AngleAxis(yaw, a);
        }

        public Vector3 GetTangent() => _collider.transform.forward;

        public Vector3 Bitangent() => _collider.transform.right;

        public Vector3 Normal() => _collider.transform.up;
    }
}