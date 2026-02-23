using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using _Game.Source;
using _Game.Source.Application;
using _Game.Source.Utilities.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Random = System.Random;

public class PaperAircraftMoveController : MonoBehaviour
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
    [SerializeField] private float _reducePowerAfterFinishDuration;

    private Vector3 _lerpDirection;
    private Vector3 _currentTargetPoint;
    [SerializeField] private ForceMode _forceMode;
    private Rigidbody _rb;
    private Trajectory _currentTrajectory;
    public event Action OnLaunch;
    private CancellationTokenSource _cts;
    private readonly Random _rnd = new();
    public event Action OnTrajectoryFinished;

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
            OnTrajectoryOver();
            StartCoroutine(ReducePower());
        }
        catch (OperationCanceledException) { }
        catch (Exception e) { Debug.LogError(e); }
    }

    private IEnumerator ReducePower()
    {
        float currentVelocityX = _rb.linearVelocity.x;
        
        var tween = DOTween.To(() => currentVelocityX, (v) =>
        {
            currentVelocityX = v;
            _rb.linearVelocity = new Vector3(currentVelocityX, _rb.linearVelocity.y, 0f);
        },0, _reducePowerAfterFinishDuration);
        yield return tween.WaitForCompletion();
    }

    private void OnTrajectoryOver()
    {
        _rb.useGravity = true;
        OnTrajectoryFinished?.Invoke();
    }

    private async UniTask FlyingProcess(CancellationToken cts)
    {
        float elapsedTime = 0;
        TrySetNewDirection();
        while (!cts.IsCancellationRequested)
        {
            elapsedTime += Time.fixedDeltaTime;
            if (IsReachTargetPoint())
            {
                var isNextPointExist = TrySetNewDirection();
                if (!isNextPointExist)
                    return;
            }

            _lerpDirection = Vector3.Lerp(_direction, _direction, _changeForceVectorSpeed);
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

    private bool TrySetNewDirection()
    {
        if (_currentTrajectory.TryGetNextPoint(out var nextPoint))
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
