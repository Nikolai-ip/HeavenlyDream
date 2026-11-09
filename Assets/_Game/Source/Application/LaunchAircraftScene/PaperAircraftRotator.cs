using System.Threading;
using _Game.Source.Utilities.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace _Game.Source.Application.LaunchAircraftScene
{
    public class PaperAircraftRotator: MonoBehaviour
    {
        private Rigidbody _rb;
        [SerializeField] private PlaneMoveController _moveController;
        [SerializeField] private float _maxXAngle;
        [SerializeField] private float _rotateAroundXDuration;
        [SerializeField] private float _xRotationRandomNormalOffset;
        private CancellationTokenSource _cts;

        private float _currentXAngle;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void StartRotateProcess()
        {
            _cts = new();
            RotateAcrossVelocity(_cts.Token).Forget();
            RotateAroundXProcess(_cts.Token).Forget();
        }

        public void StopRotateProcess()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private async UniTask RotateAcrossVelocity(CancellationToken cts)
        {
            while (!cts.IsCancellationRequested)
            {
                float angle = Mathf.Atan2(_rb.linearVelocity.y, _rb.linearVelocity.x) * Mathf.Rad2Deg;
                Quaternion rotByVel = Quaternion.Euler(angle + 180, -90, 0);
                Quaternion rotByX = Quaternion.Euler(0, 0, _currentXAngle);
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
                if (token.IsCancellationRequested)
                    return;
                
                elapsed += Time.deltaTime;
                float t = elapsed / _rotateAroundXDuration;
                _currentXAngle = Mathf.Lerp(startAngle, targetXAngle, t);
                await UniTask.Yield(token);
            }
            
            _currentXAngle = targetXAngle;
        }

        private void OnDisable()
        {
            _moveController.OnLaunch -= StartRotateProcess;
            _moveController.OnTrajectoryFinished -= StopRotateProcess;
            StopRotateProcess();
        }
        
    }
}