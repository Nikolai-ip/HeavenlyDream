using System;
using System.Threading;
using _Game.Source.Utilities.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace _Game.Source.Application
{
    public class PaperAircraftRotator: MonoBehaviour
    {
        private Rigidbody _rb;
        [SerializeField] private PaperAircraftMoveController _moveController;
        [SerializeField] private float _maxXAngle;
        [SerializeField] private float _rotateAroundXDuration;
        [SerializeField] private float _xRotationRandomNormalOffset;
        private CancellationTokenSource _cts;

        private float _currentXAngle;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
        private void OnEnable()
        {
            _moveController.OnLaunch += StartRotateProcess;
            _moveController.OnTrajectoryFinished += StopRotateProcess;
        }

        private void StartRotateProcess()
        {
            _cts = new();
            RotatePaperPlane(_cts.Token).Forget();
            RotateAroundXProcess(_cts.Token).Forget();
        }

        private async UniTask RotatePaperPlane(CancellationToken cts)
        {
            while (!cts.IsCancellationRequested)
            {
                float angle = Mathf.Atan2(_rb.linearVelocity.y, _rb.linearVelocity.x) * Mathf.Rad2Deg;
                Quaternion rotByVel = Quaternion.Euler(0, 0, angle + 180);
                Quaternion rotByX = Quaternion.Euler(_currentXAngle, 0, 0);

                transform.rotation = rotByVel * rotByX;
                await UniTask.WaitForFixedUpdate(cts);
            }
        }

        private async UniTask RotateAroundXProcess(CancellationToken cts)
        {
            Random rnd = new();
            while (!cts.IsCancellationRequested)
            {
                float angleX = rnd.RandomNormal(_xRotationRandomNormalOffset) * _maxXAngle; 
                await RotateAroundX(angleX, cts);
            }
        }

        private async UniTask RotateAroundX(float targetXAngle, CancellationToken token)
        {
            float startAngle = _currentXAngle;
            float elapsed = 0f;

            while (elapsed < _rotateAroundXDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / _rotateAroundXDuration;
                _currentXAngle = Mathf.Lerp(startAngle, targetXAngle, t);
                await UniTask.Yield(token);
            }
            
            _currentXAngle = targetXAngle;
        }

        private void StopRotateProcess()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private void OnDisable()
        {
            _moveController.OnLaunch -= StartRotateProcess;
            _moveController.OnTrajectoryFinished -= StopRotateProcess;
            StopRotateProcess();
        }
        
    }
}