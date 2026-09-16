using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Game.Source.Domain.Plane.Component;
using _Game.Source.Infrastructure.Input.Pointer;
using _Game.Source.Presenter;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.Common
{
    public class ShakableComponentAttacher: ComponentAttacher
    {
        [SerializeField] private PointerInteractionListener _interactionListener;
        [SerializeField] private ShakablePointerFollower _handShaker;
        [SerializeField] private float _distanceToAbleFix;
        private AirplaneModelView _airplaneModelView;

        private bool _wingFollowingByMouse;
        private bool _fixProcess;
        public override event Action<Vector3, Quaternion> OnComponentAttached; 

        private void OnEnable()
        {
            _interactionListener.onPointerUp.AddListener(StartFixComponentProcess);
        }
        private void OnDisable()
        {
            _interactionListener.onPointerUp.RemoveListener(StartFixComponentProcess);
        }
        
        public override void Construct(AirplaneModelView airplaneModelView, AirplaneComponentType componentType)
        {
            _airplaneModelView = airplaneModelView;
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
            
            _handShaker.StartShaking(_airplaneModelView.FuselageTr, _airplaneModelView.FuselageSize);
            while (true)
            {
                if (Input.GetMouseButtonDown(0) && IsNearToAttachPoint())
                    break;   
                yield return null;
            }
            _handShaker.StopShaking();
            Vector3 resultLocalPos = _airplaneModelView.transform.InverseTransformPoint(transform.position);
            Quaternion resultLocalRot = Quaternion.Inverse(_airplaneModelView.transform.rotation) * transform.rotation;
            callBack?.Invoke(resultLocalPos, resultLocalRot);
        }
        private bool IsNearToAttachPoint()
        {
            return Vector2.Distance(_airplaneModelView.Center, transform.position)
                   <= _distanceToAbleFix;
        }

    }


}