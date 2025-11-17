using System;
using System.Threading;
using _Game.Source;
using _Game.Source.Utilities.Extensions;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using Random = System.Random;

public class PaperAircraftMoveController : MonoBehaviour
{
    [ReadOnly] [SerializeField] private float _flightPower;
    [SerializeField] private float _maxFlightPower = 12f;
    [SerializeField] private float _changePowerInterval;
    [SerializeField] private float _flightPowerStdDev;
    [SerializeField] private float _minFlightPower = 8f;
    [SerializeField] private float _launchPower;
    [SerializeField] private ForceMode _launchForceMode;

    [Range(0, 2)] [SerializeField] private float _changeForceVectorSpeed;
    [SerializeField] private Vector3 _direction;
    private Vector3 _lerpDirection;
    private Vector3 _currentTargetPoint;
    [SerializeField] private ForceMode _forceMode;
    [SerializeField] private float _sqrtApproximation;
    private Rigidbody _rb;
    private Trajectory _currentTrajectory;
    [SerializeField] private Trajectory _trajectory;
    [SerializeField] private Trajectory _fallTrajectory;
    public event Action OnLaunch;
    private CancellationTokenSource _cts;
    private Random _rnd = new();

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _currentTrajectory =  _trajectory;
        LaunchAircraft();
    }

    private async void LaunchAircraft()
    {
        try
        {
            _flightPower = (_maxFlightPower + _minFlightPower) / 2;
            _rb.AddForce(_direction * _launchPower, _launchForceMode);
            OnLaunch?.Invoke();
            _cts = new CancellationTokenSource();
            await FlyingProcess(_cts.Token);
        }
        catch (OperationCanceledException) { }
        catch (Exception e) { Debug.LogError(e); }
    }

    private async UniTask FlyingProcess(CancellationToken cts)
    {
        float elapsedTime = 0;
        SetNewDirection();
        while (!cts.IsCancellationRequested)
        {
            elapsedTime += Time.fixedDeltaTime;
            if (IsReachTargetPoint())
                SetNewDirection();

            _lerpDirection = Vector3.Lerp(_direction, _direction, _changeForceVectorSpeed);
            Move();
            if (elapsedTime >= _changePowerInterval)
            {
                SetFlightPower();
                elapsedTime = 0;
            }
            await UniTask.WaitForFixedUpdate(cts);
        }   
    }

    private bool IsReachTargetPoint() => (_currentTargetPoint - transform.position).sqrMagnitude < _sqrtApproximation;

    private void SetFlightPower()
    {
        float t = _rnd.RandomNormal(0.5f, _flightPowerStdDev);
        _flightPower = Mathf.Lerp(_minFlightPower, _maxFlightPower, t);
    }

    private void SetNewDirection()
    {
        if (_currentTrajectory.IsTrajectoryOver())
        {
            _currentTrajectory = _fallTrajectory;
            _currentTrajectory.CreateTrajectoryPoints(transform.position);
        }
        
        if (_currentTrajectory.TryGetNextPoint(out var nextPoint))
        {
            _currentTargetPoint = nextPoint;
            _direction = (_currentTargetPoint - transform.position).normalized;
        }
    }

    private void Move()
    {
        _rb.AddForce(_lerpDirection * _flightPower, _forceMode);
    }

    private void OnDisable()
    {
        _cts?.Dispose();
        _cts = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_currentTargetPoint, 1f);
    }
}
