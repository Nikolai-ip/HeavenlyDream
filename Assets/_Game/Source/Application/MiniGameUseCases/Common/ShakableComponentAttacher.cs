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
        [SerializeField] private int _surfaceAttachPointIndex;
        [SerializeField] private float _yaw;
        [SerializeField] private RotationType _relativeToSurface;

        private AirplaneModelView _airplaneModelView;

        private bool _fixProcess;
        private AirplaneComponentType _componentType;
        private Transform _attachPoint;
        private AirplaneSurface _surface;
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
            _surface =  _airplaneModelView.GetSurface(
                ComponentsDependencyResolver.GetAirplaneSurfaceFor(_componentType));
            _attachPoint = _surface.GetAttachPoint(_surfaceAttachPointIndex);
            _handShaker.WarmUp(_surface, _relativeToSurface, _yaw);
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
                
                _handShaker.StartFollowing(
                    _surface,
                    _relativeToSurface,
                    _yaw, 
                    _surfaceAttachPointIndex);
                
                while (true)
                {
                    if (Input.GetMouseButtonDown(0) && IsNearToAttachPoint())
                        break;   
                    
                    yield return null;
                }
                _handShaker.StopFollowing();
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
        

        private bool IsNearToAttachPoint()
        {
            return Vector3.Distance(_attachPoint.position, transform.position) <= _distanceToAbleFix;
        }
    }
}
