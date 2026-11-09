using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Source.Application.LaunchAircraftScene
{
    public class PlaneView: MonoBehaviour
    {
        [SerializeField] private PaperAircraftRotator _rotator;
        [SerializeField] private List<TrailRenderer> _trails;
        [SerializeField] private PlaneMoveController _moveController;

        private void Start()
        {
            DeactivateLaunchViewState();
        }

        private void OnEnable()
        {
            _moveController.OnLaunch += ActivateLaunchViewState;
            _moveController.OnTrajectoryFinished += DeactivateLaunchViewState;
        }

        private void OnDisable()
        {
            _moveController.OnLaunch -= ActivateLaunchViewState;
            _moveController.OnTrajectoryFinished -= DeactivateLaunchViewState;
        }

        private void ActivateLaunchViewState()
        {
            _rotator.StartRotateProcess();
            _trails.ForEach(t => t.enabled = true);
        }

        private void DeactivateLaunchViewState()
        {
            _rotator.StopRotateProcess();            
            _trails.ForEach(t => t.enabled = false);
        }
    }
}