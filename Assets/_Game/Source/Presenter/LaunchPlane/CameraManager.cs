using System;
using _Game.Source.Application.LaunchAircraftScene;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace _Game.Source.Presenter.LaunchPlane
{
    public class CameraManager: MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _followCamera;
        [SerializeField] private CinemachineCamera _finishCamera;
        [SerializeField] private CinemachineBrain _camera;
        [SerializeField] private Transform _finishPoint;
        [SerializeField] private float _distanceToSwitchCamera;
        [SerializeField] private Transform _airplane;
        [SerializeField] private float _switchDelay;
        [SerializeField] private PlaneMoveController _planeMoveController;
        private float _time;
        private bool _isSwitching;

        private void Awake()
        {
            var camPos = _airplane.position;
            camPos.z = -10;
            _followCamera.transform.position = camPos;
        }

        private void OnEnable()
        {
            _planeMoveController.OnLaunch += StartFollowForPlane;
        }
        
        private void OnDisable()
        {
            _planeMoveController.OnLaunch -= StartFollowForPlane;
        }

        private async void StartFollowForPlane()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_switchDelay));
            _followCamera.Priority = 1;   
        }

        private void Update()
        {
            if (_isSwitching)
                return;

            float distanceSqr = (_finishPoint.position - _airplane.position).sqrMagnitude;
            float switchDistanceSqr = _distanceToSwitchCamera * _distanceToSwitchCamera;

            if (distanceSqr >= switchDistanceSqr)
                return;

            _isSwitching = true;

            float distance = Mathf.Sqrt(distanceSqr);
            float blendDuration = Mathf.Lerp(0.1f, 2f, distance / _distanceToSwitchCamera);

            _camera.DefaultBlend.Time = blendDuration;

            _finishCamera.Priority = _followCamera.Priority + 1;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_finishPoint.position, _distanceToSwitchCamera);
        }
    }
}