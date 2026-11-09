using System;
using System.Linq;
using System.Threading;
using _Game.Source.Utilities.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Random = System.Random;

namespace _Game.Source.Application.LaunchAircraftScene
{
    public class PlaneMoveController : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private float _flightPower;
        [SerializeField] private float _maxFlightPower = 12f;
        [SerializeField] private float _minFlightPower = 8f;
        [SerializeField] private float _changePowerInterval;
        [SerializeField] private float _flightPowerStdDev;
        [SerializeField] private float _launchPower;
        [SerializeField] private ForceMode _launchForceMode;
        [Range(0, 2)] [SerializeField] private float _changeForceVectorSpeed;
        [SerializeField] private Vector3 _direction;
        [SerializeField] private Trajectory _trajectory;
        [SerializeField] private ManualTrajectory _finishTrajectory;

        [SerializeField] private float _landingDescentSpeed = 1.5f;
        [SerializeField] private float _landingDuration = 3f;
        [SerializeField] private float _groundRollDeceleration = 2f;
        [SerializeField] private float _landingRotationSpeed = 180f;
        [SerializeField] private Ease _reducePowerFunc;

        private bool _isGrounded;
        private Vector3 _groundNormal = Vector3.up;

        private Vector3 _lerpDirection;
        private Vector3 _currentTargetPoint;
        [SerializeField] private ForceMode _forceMode;
        private Rigidbody _rb;
        public event Action OnLaunch;
        private CancellationTokenSource _cts;
        private readonly Random _rnd = new();
        public event Action OnTrajectoryFinished;

        public void LaunchPlane()
        {
            _rb = GetComponent<Rigidbody>();
            _cts = new CancellationTokenSource();
            _finishTrajectory.CreateTrajectoryPoints(_trajectory.TrajectoryPoints.Last());
            LaunchAircraft(_cts.Token).Forget();
        }

        private async UniTask LaunchAircraft(CancellationToken cts)
        {
            try
            {
                _flightPower = (_maxFlightPower + _minFlightPower) / 2;
                _rb.AddForce(_direction * _launchPower, _launchForceMode);
                OnLaunch?.Invoke();
                Debug.Log("Start fly follow by " + _trajectory.name);
                await FlyingProcess(cts, _trajectory);
                Debug.Log("Start fly follow by " + _finishTrajectory.name);
                await FlyingProcess(cts, _finishTrajectory);
                if (cts.IsCancellationRequested) return;
                Debug.Log("Start falling");
                await ReducePower(cts);
            }
            catch (OperationCanceledException) { }
            catch (Exception e) { Debug.LogError(e); }
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
                return;

            _isGrounded = true;
            OnTrajectoryFinished?.Invoke();

            if (collision.contactCount > 0)
                _groundNormal = collision.GetContact(0).normal;
        }

        private async UniTask FlyingProcess(CancellationToken cts, Trajectory trajectory)
        {
            float elapsedTime = 0;
            TrySetNewDirection(trajectory);
            while (!cts.IsCancellationRequested)
            {
                elapsedTime += Time.fixedDeltaTime;
                if (IsReachTargetPoint() && !TrySetNewDirection(trajectory))
                {
                    return;
                }

                _lerpDirection = Vector3.Lerp(_lerpDirection, _direction, _changeForceVectorSpeed);
                Move(_lerpDirection,  _flightPower, _forceMode);
                if (elapsedTime >= _changePowerInterval)
                {
                    SetFlightPower();
                    elapsedTime = 0;
                }
                await UniTask.WaitForFixedUpdate(cts);
            }   
        }

        private bool IsReachTargetPoint()
        {
            return _currentTargetPoint.x > transform.position.x;
        }

        private bool TrySetNewDirection(Trajectory trajectory)
        {
            if (trajectory.TryGetNextPoint(out var nextPoint))
            {
                _currentTargetPoint = nextPoint;
                _direction = (_currentTargetPoint - transform.position).normalized;
                return true;
            }
            return false;
        }

        private void SetFlightPower()
        {
            float t = _rnd.RandomNormal(0.5f, _flightPowerStdDev);
            _flightPower = Mathf.Lerp(_minFlightPower, _maxFlightPower, t);
        }


        private void Move(Vector3 dir, float flightPower, ForceMode forceMode)
        {
            _rb.AddForce(dir * flightPower, forceMode);
        }

        private async UniTask ReducePower(CancellationToken cts)
        {
            _isGrounded = false;
            _rb.useGravity = false;

            float elapsed = 0f;
            float startPower = _flightPower;

            Debug.Log("Start velocity " + startPower);

            while (!_isGrounded)
            {
                cts.ThrowIfCancellationRequested();

                elapsed += Time.fixedDeltaTime;

                float t = Mathf.Clamp01(elapsed / _landingDuration);
                float easedT = DOVirtual.EasedValue(0f, 1f, t, _reducePowerFunc);

                float targetPower = Mathf.Lerp(
                    startPower,
                    startPower * 0.5f,
                    easedT);
            
                float targetY = -_landingDescentSpeed * easedT;

                Vector3 direction = new(-1, targetY, 0f);
                Debug.Log("Set velocity to " + direction);
                Move(direction.normalized, targetPower, _forceMode);

                await UniTask.WaitForFixedUpdate(cts);
            }

            if (cts.IsCancellationRequested)
                return;
        
            Debug.Log("Enable Gravity");
            _rb.useGravity = true;

            while (!_isGrounded)
            {
                cts.ThrowIfCancellationRequested();
                await UniTask.WaitForFixedUpdate(cts);
            }

            await GroundRoll(cts);
        }
    
        private async UniTask GroundRoll(CancellationToken cts)
        {
            _rb.useGravity = true;

            while (_rb.linearVelocity.sqrMagnitude > 0.01f)
            {
                cts.ThrowIfCancellationRequested();

                // Убираем движение внутрь поверхности.
                Vector3 velocity = _rb.linearVelocity;
                Vector3 groundVelocity = Vector3.ProjectOnPlane(
                    velocity,
                    _groundNormal);

                _rb.linearVelocity = Vector3.MoveTowards(
                    velocity,
                    groundVelocity.normalized *
                    Mathf.Max(0f, groundVelocity.magnitude -
                                  _groundRollDeceleration * Time.fixedDeltaTime),
                    _groundRollDeceleration * Time.fixedDeltaTime);

                // Выравниваем верхнюю сторону самолёта по поверхности.
                Quaternion targetRotation =
                    Quaternion.FromToRotation(transform.up, _groundNormal) *
                    transform.rotation;

                _rb.MoveRotation(Quaternion.RotateTowards(
                    _rb.rotation,
                    targetRotation,
                    _landingRotationSpeed * Time.fixedDeltaTime));

                _rb.angularVelocity = Vector3.zero;

                await UniTask.WaitForFixedUpdate(cts);
            }

            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        private void OnDisable()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_currentTargetPoint, 1f);
        }
    }
}
