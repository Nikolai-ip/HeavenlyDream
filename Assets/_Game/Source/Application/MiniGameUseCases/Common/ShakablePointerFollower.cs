using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Source.Application.MiniGameUseCases.Common
{
    public class ShakablePointerFollower : MonoBehaviour
    {
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private float _radius = 20f;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _smooth = 0.5f;
        [SerializeField] private float _surfaceOffset = 0.02f;

        private Coroutine _shakingCoroutine;
        private Vector3 _p0;
        private Vector3 _p1;
        private Vector3 _p2;
        private Vector3 _p3;
        private Vector3 _desiredPosition;
        private Vector3 _target;
        private Camera _projectionCamera;

        private void Start()
        {
            _projectionCamera = Camera.main;
        }

        public void StartFollowing(AirplaneSurface surface, int surfaceAttachPointIndex)
        {
            StopShaking();
            _shakingCoroutine = StartCoroutine(Follow(surface, surfaceAttachPointIndex));
        }

        public void StopShaking()
        {
            if (_shakingCoroutine != null)
                StopCoroutine(_shakingCoroutine);
        }
        
        private IEnumerator Follow(AirplaneSurface surface, int attachPointIndex)
        {
            float t = 0f;
            GenerateBezier(out _p0, out _p1, out _p2, out _p3);
            _target = surface.Center;
            Transform attachPoint = surface.GetAttachPoint(attachPointIndex);
            
            while (true)
            {
                Ray pointerRay = _projectionCamera.ScreenPointToRay(Input.mousePosition);
                surface.Project(pointerRay, out _target, out _);


                t += Time.deltaTime * _speed;

                if (t >= 1f)
                {
                    t = 0f;
                    GenerateBezier(
                        out _p0,
                        out _p1,
                        out _p2,
                        out _p3);
                }
                Vector3 offset = CubicBezier(_p0, _p1, _p2, _p3, t);

                Vector3 tangent = attachPoint.forward;
                Vector3 bitangent = attachPoint.right;
                Vector3 planeNormal = attachPoint.up;
                
                Vector3 shakeOffset = tangent * offset.x + bitangent * offset.y;
                Vector3 candidatePosition = _target + shakeOffset;
                
                _desiredPosition =  attachPoint.position +
                                    Vector3.ProjectOnPlane(
                                        candidatePosition - attachPoint.position,
                                        planeNormal);

                transform.position += (_desiredPosition - transform.position)
                                      * (1f - Mathf.Exp(-_smooth * Time.deltaTime));
                    
                yield return null;
            }
        }

        private void GenerateBezier(out Vector3 p0, out Vector3 p1, out Vector3 p2, out Vector3 p3)
        {
            Vector2 start = Random.insideUnitCircle * _radius;
            Vector2 end = Random.insideUnitCircle * _radius;

            p0 = start;
            p3 = end;

            // Направление движения.
            Vector2 direction = (end - start).normalized;

            // Перпендикуляр к направлению.
            Vector2 perpendicular = new Vector2(
                -direction.y,
                direction.x);

            // Случайное искривление траектории.
            float curvature = Random.Range(
                -_radius,
                _radius);

            Vector2 controlOffset = perpendicular * curvature;

            p1 = start + direction * (_radius * 0.5f) + controlOffset;
            p2 = end - direction * (_radius * 0.5f) + controlOffset;
        }

        private Vector3 CubicBezier(
            Vector3 p0,
            Vector3 p1,
            Vector3 p2,
            Vector3 p3,
            float t)
        {
            float u = 1f - t;

            return
                u * u * u * p0 +
                3f * u * u * t * p1 +
                3f * u * t * t * p2 +
                t * t * t * p3;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            var radius = 0.25f;
            Gizmos.DrawWireSphere(_p0, radius);
            Gizmos.DrawWireSphere(_p1, radius);
            Gizmos.DrawWireSphere(_p2, radius);
            Gizmos.DrawWireSphere(_p3, radius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_desiredPosition, radius);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_target, 0.2f);
        }
    }
}