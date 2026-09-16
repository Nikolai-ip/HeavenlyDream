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
        [SerializeField] private bool _shakingHorizontal;
        [SerializeField] private bool _shakingVertical;

        private Coroutine _shakingCoroutine;
        private Vector3 _p0;
        private Vector3 _p1;
        private Vector3 _p2;
        private Vector3 _p3;
        private Vector3 _desiredPosition;
        private Vector3 _target;

        public void SetFreezeAxis(bool shakingHorizontal, bool shakingVertical)
        {
            _shakingHorizontal = shakingHorizontal;
            _shakingVertical = shakingVertical;
        }

        public void StartFollowing(AirplaneSurface surface)
        {
            StopShaking();
            _shakingCoroutine = StartCoroutine(Follow(surface));
        }

        public void StopShaking()
        {
            if (_shakingCoroutine != null)
                StopCoroutine(_shakingCoroutine);
        }
        
        private IEnumerator Follow(AirplaneSurface surface)
        {
            float t = 0f;
            GenerateBezier(out _p0, out _p1, out _p2, out _p3);
            _target = surface.Center;
            Vector3 startPos = transform.position;
            transform.position = startPos;
            
            while (true)
            {
                _target = surface.Project(Camera.main.ScreenToWorldPoint(Input.mousePosition));

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
                offset = new Vector3(_shakingHorizontal? offset.x: 0, _shakingVertical? offset.y: 0)
                {
                    z = 0
                };

                //_desiredPosition = _target + offset;
                _desiredPosition = _target;
                transform.position = _desiredPosition;
                // transform.position += (_desiredPosition-transform.position)
                //                       * (1f - Mathf.Exp(-_smooth * Time.deltaTime)); 
                    
                yield return null;
            }
        }

        private Vector3 ProjectMouseOnFuselage(Transform fuselage, float size, Vector3 m)
        {
            Vector3 s = fuselage.position + fuselage.right * (size * 0.5f); 
            Vector3 e = fuselage.position - fuselage.right * (size * 0.5f);
            Vector3 se = s - e;
            Vector3 em = m - e;
            float t = Vector3.Dot(em, se)/Vector3.Dot(se, se);
            t = Mathf.Clamp01(t);
            return e + t * se;
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