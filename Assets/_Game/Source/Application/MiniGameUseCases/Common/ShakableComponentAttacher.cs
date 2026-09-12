using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Source.Domain.Plane.Component;
using _Game.Source.Infrastructure.Input.Pointer;
using _Game.Source.Presenter;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.AttackWingMiniGame
{
    public class ShakableComponentAttacher: ComponentAttacher
    {
        [SerializeField] private PointerInteractionListener _interactionListener;
        [SerializeField] private HandShaker _handShaker;
        [SerializeField] private float _distanceToAbleFix;
        [SerializeField] private AirplaneComponentType _airplaneComponentType;
        [SerializeField] private List<AxisType> _sequenceToFixComponent;
        private Queue<AxisType> _fixPhases = new();

        private FixLocalPosComponentPhase _fixLocalPosComponentPhase;
        private bool _wingFollowingByMouse;
        private bool _fixProcess;
        public override event Action<Vector3, Quaternion> OnComponentAttached; 

        private void OnEnable()
        {
            _interactionListener.onPointerDown.AddListener(StartFixComponentProcess);
        }
        private void OnDisable()
        {
            _interactionListener.onPointerDown.AddListener(StartFixComponentProcess);
        }
        
        public override void Construct(AirplaneModelView airplaneModelView)
        {
            _fixLocalPosComponentPhase = new FixLocalPosComponentPhase(
                _handShaker,
                airplaneModelView,
                transform,
                _distanceToAbleFix);
        }

        private async void StartFixComponentProcess()
        {
            if (_fixProcess)
                return;
            
            _fixProcess = true;
            
            foreach (var axisType in _sequenceToFixComponent)
            {
                _fixPhases.Enqueue(axisType);
            }
            Vector3 resultLocalPos = Vector2.zero;
            while (_fixPhases.Any())
            {
                var phaseAxisType = _fixPhases.Dequeue();
                Vector3 additiveLocalPos = await _fixLocalPosComponentPhase.ProcessPhase(phaseAxisType);
                resultLocalPos += additiveLocalPos;
            }

            _fixProcess = false;
            OnComponentAttached?.Invoke(resultLocalPos, Quaternion.identity);
        }
    }

    public enum AxisType
    {
        X,
        Y,
        Z,
        All
    }
}