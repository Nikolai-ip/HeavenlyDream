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
    [SerializeField] private ForceMode _forceMode;
    [SerializeField] private float _sqrtApproximation;
    private Rigidbody _rb;
    [SerializeField] private Trajectory _trajectory;
    public event Action OnLaunch;
    private CancellationTokenSource _cts;
    private Random _rnd = new();

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
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
        while (!cts.IsCancellationRequested)
        {
            elapsedTime += Time.fixedDeltaTime;
            SetDirection();
            Move();
            if (elapsedTime >= _changePowerInterval)
            {
                SetFlightPower();
                elapsedTime = 0;
            }
            await UniTask.WaitForFixedUpdate(cts);
        }   
    }

    private void SetFlightPower()
    {
        float t = _rnd.RandomNormal(0.5f, _flightPowerStdDev);
        _flightPower = Mathf.Lerp(_minFlightPower, _maxFlightPower, t);
    }

    private void SetDirection()
    {
        Vector3 nextPoint = _trajectory.GetNextPoint();
        Vector3 currentPoint = _trajectory.GetCurrentPoint();
        Vector3 diff = nextPoint - transform.position;
        if (diff.sqrMagnitude < _sqrtApproximation)
            _trajectory.OnKeyPointReached();
        
        Vector3 dir = (nextPoint - currentPoint).normalized;
        
        _direction = Vector3.Lerp(_direction, dir, _changeForceVectorSpeed);
    }

    private void Move()
    {
        _rb.AddForce(_direction * _flightPower, _forceMode);
    }

    private void OnDisable()
    {
        _cts?.Dispose();
        _cts = null;
    }
}
