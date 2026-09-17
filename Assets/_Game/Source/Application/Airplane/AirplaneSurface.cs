using System.Collections.Generic;
using UnityEngine;

namespace _Game.Source.Application
{
    public class AirplaneSurface: MonoBehaviour
    {
        [SerializeField] private List<Transform> _attachPoints;
        private Collider _collider;
        private Camera _camera;

        public Vector3 Center => _collider.bounds.center;
        public Transform Transform => _collider.transform;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _camera = Camera.main;
        }

        public bool TryProject(Ray ray, out RaycastHit hit)
        {
            hit = default;
            float maxDistance = _camera.farClipPlane;
            return _collider.Raycast(ray, out hit, maxDistance);
        }

        public void Project(Ray ray, out Vector3 point, out Vector3 normal)
        {
            if (TryProject(ray, out RaycastHit hit))
            {
                point = hit.point;
                normal = hit.normal;
                return;
            }

            Vector3 center = _collider.bounds.center;
            float distanceAlongRay = Vector3.Dot(center - ray.origin, ray.direction);
            Vector3 cursorPoint = ray.GetPoint(Mathf.Max(0f, distanceAlongRay));

            point = _collider.ClosestPoint(cursorPoint);

            Vector3 normalDirection = cursorPoint - point;
            if (normalDirection.sqrMagnitude > Mathf.Epsilon)
            {
                normal = normalDirection.normalized;
                return;
            }

            normalDirection = point - center;
            normal = normalDirection.sqrMagnitude > Mathf.Epsilon
                ? normalDirection.normalized
                : Transform.forward;
        }

        public Transform GetAttachPoint(int attachPointIndex)
        {
            return _attachPoints[attachPointIndex];
        }
    }
}