using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private DirectionType _relativeToSurface;
        [SerializeField] private int _surfaceAttachPointIndex;
        private AirplaneModelView _airplaneModelView;

        private bool _wingFollowingByMouse;
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
        }

        private void StartFixComponentProcess()
        {
            if (_fixProcess)
                return;
            
            _fixProcess = true;

            StartCoroutine(AttachProcess((resultLocalPos, rotation) =>
            {
                _fixProcess = false;
                OnComponentAttached?.Invoke(resultLocalPos, rotation);
            }));

        }

        private IEnumerator AttachProcess(Action<Vector3, Quaternion> callBack)
        {
            yield return null; //Wait for input handled

            AirplaneSurface surface = _airplaneModelView
                .GetSurface(ComponentsDependencyResolver.GetAirplaneSurfaceFor(_componentType));
            
            
            _handShaker.StartFollowing(surface, _surfaceAttachPointIndex);
            while (true)
            {
                if (Input.GetMouseButtonDown(0) && IsNearToAttachPoint(surface))
                    break;   
                yield return null;
            }
            _handShaker.StopShaking();
            Vector3 resultLocalPos = _airplaneModelView.transform.InverseTransformPoint(transform.position);
            //Quaternion resultLocalRot = Quaternion.Inverse(_airplaneModelView.transform.rotation) * transform.rotation;
            callBack?.Invoke(resultLocalPos, Quaternion.identity);
        }
        private bool IsNearToAttachPoint(AirplaneSurface airplaneSurface)
        {
            return Vector3.Distance(airplaneSurface.Center, transform.position) <= _distanceToAbleFix;
        }

    }

    public enum DirectionType
    {
        Forward,
        Right,
        Up
    }


}