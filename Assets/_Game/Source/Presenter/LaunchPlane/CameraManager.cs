using System;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace _Game.Source.Presenter.LaunchPlane
{
    public class CameraManager: MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _followCamera;
        [SerializeField] private float _switchDelay;
        private float _time;

        private async void Start()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_switchDelay));
            _followCamera.Priority = 1;
        }

   
    }
}