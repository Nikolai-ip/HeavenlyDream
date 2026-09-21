using System;
using System.Collections;
using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using _Game.Source.Infrastructure.Input.Pointer;
using _Game.Source.Presenter;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.Common
{
    public class ShakableComponentAttacher: ComponentAttacher
    {
        [SerializeField] private PointerInteractionListenerBase _interactionListener;
        [SerializeField] private ShakablePointerFollower _handShaker;
        [SerializeField] private float _distanceToAbleFix;
        [SerializeField] private bool _invertedDirection;
        [SerializeField] private DirectionType _relativeToSurface;
        [SerializeField] private int _surfaceAttachPointIndex;
        [SerializeField] private float _yawOffsetDegrees;
        [SerializeField] private Vector3 _localRotation;

        private AirplaneModelView _airplaneModelView;

        private bool _fixProcess;
        private AirplaneComponentType _componentType;
        public override event Action<Vector3, Quaternion> OnComponentAttached; 

        private void OnEnable()
        {
            _interactionListener.onPointerUp.AddListener(StartFixComponentProcess);
        }
        private void OnDisable()
        {
            _interactionListener.onPointerUp.RemoveListener(StartFixComponentProcess);
        }
        
        public override void Construct(AirplaneModelView airplaneModelView, AirplaneComponentType componentType, Vector3 spawnPosition)
        {
            _airplaneModelView = airplaneModelView;
            _componentType = componentType;
            transform.position = spawnPosition;
            ApplyAttachOrientation(GetTargetSurface());
        }

        private void StartFixComponentProcess()
        {
            if (_fixProcess)
                return;
            
            _fixProcess = true;

            StartCoroutine(AttachProcess((resultLocalPos, rotation) =>
            {
                OnComponentAttached?.Invoke(resultLocalPos, rotation);
            }));
        }

        private IEnumerator AttachProcess(Action<Vector3, Quaternion> callBack)
        {
            try
            {
                yield return null; //Wait for input handled

                AirplaneSurface surface = GetTargetSurface();
                Transform attachPoint = surface.GetAttachPoint(_surfaceAttachPointIndex);
                if (attachPoint == null)
                    yield break;
                
                _handShaker.StartFollowing(
                    surface,
                    _surfaceAttachPointIndex,
                    _relativeToSurface,
                    _invertedDirection,
                    _localRotation,
                    _yawOffsetDegrees);
                while (true)
                {
                    if (Input.GetMouseButtonDown(0) && IsNearToAttachPoint(attachPoint))
                        break;   
                    yield return null;
                }
                _handShaker.StopShaking();
                Vector3 resultLocalPos = _airplaneModelView.transform.InverseTransformPoint(transform.position);
                Quaternion resultLocalRot =
                    Quaternion.Inverse(_airplaneModelView.transform.rotation) * transform.rotation;
                callBack?.Invoke(resultLocalPos, resultLocalRot);
            }
            finally
            {
                _fixProcess = false;
            }
        }

        private AirplaneSurface GetTargetSurface()
        {
            return _airplaneModelView.GetSurface(
                ComponentsDependencyResolver.GetAirplaneSurfaceFor(_componentType));
        }

        private void ApplyAttachOrientation(AirplaneSurface surface)
        {
            Transform attachPoint = surface.GetAttachPoint(_surfaceAttachPointIndex);
            if (attachPoint == null)
                return;

            transform.rotation = AttachOrientation.GetWorldRotation(
                attachPoint,
                _relativeToSurface,
                _invertedDirection,
                yawDegrees: _yawOffsetDegrees) * Quaternion.Euler(_localRotation);
        }

        private bool IsNearToAttachPoint(Transform attachPoint)
        {
            return Vector3.Distance(attachPoint.position, transform.position) <= _distanceToAbleFix;
        }
    }
}
