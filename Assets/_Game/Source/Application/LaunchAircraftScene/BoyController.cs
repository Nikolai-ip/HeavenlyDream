using System;
using UnityEngine;

namespace _Game.Source.Application.LaunchAircraftScene
{
    public class BoyController: MonoBehaviour
    {
        [SerializeField] private Animator _boyAnimator;
        [SerializeField] private PlaneMoveController _planeMoveController;
        [SerializeField] private Transform _launchPoint;

        private void Awake()
        {
            _planeMoveController.transform.parent = _launchPoint;
            _planeMoveController.transform.localPosition = Vector3.zero;
        }

        public void LaunchPlane()
        {
            _boyAnimator.SetTrigger("Launch");
        }

        public void OnLaunchAnimEvent()
        {
            _planeMoveController.transform.parent = null;
            _planeMoveController.LaunchPlane();
        }
    }
}