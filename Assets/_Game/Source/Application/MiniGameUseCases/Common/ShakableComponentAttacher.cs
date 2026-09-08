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
    public class ShakableComponentAttacher: MonoBehaviour
    {
        [SerializeField] private PointerInteractionListener _interactionListener;
        [SerializeField] private HandShaker _handShaker;
        [SerializeField] private float _distanceToAbleFix;
        [SerializeField] private AirplaneComponentType _airplaneComponentType;
        [SerializeField] private List<AxisType> _sequenceToFixComponent;
        private Queue<AxisType> _fixPhases = new();

        private FixComponentPhase _fixComponentPhase;
        private bool _wingFollowingByMouse;
        private bool _fixProcess;
        public event Action<List<float>, List<AxisType>> OnComponentAttached; 

        private void OnEnable()
        {
            _interactionListener.onPointerDown.AddListener(StartFixComponentProcess);
        }
        private void OnDisable()
        {
            _interactionListener.onPointerDown.AddListener(StartFixComponentProcess);
        }
        
        public void Construct(AirplaneModelView airplaneModelView)
        {
            _fixComponentPhase = new FixComponentPhase(
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
            List<float> axisValues = new();
            while (_fixPhases.Any())
            {
                var phaseAxisType = _fixPhases.Dequeue();
                float diffWithRef = await _fixComponentPhase.ProcessPhase(phaseAxisType);
                axisValues.Add(diffWithRef);
            }

            _fixProcess = false;
            OnComponentAttached?.Invoke(axisValues, _sequenceToFixComponent);
        }
    }

    public enum AxisType
    {
        Horizontal,
        Vertical,
        Both
    }
}