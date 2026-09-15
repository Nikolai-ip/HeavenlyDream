using System;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Source.Temp
{
    public class Rotator: MonoBehaviour
    {
        private Vector3 _prevPoint;
        private Vector3 _currentPoint;
        private Vector3 _desiredDelta;
        private Vector3 _currentDelta;
        private int _updatePrevFramesInterval = 3;
        private int _frameCounter = 0;
        
        [SerializeField] private Vector2Int _invert = Vector2Int.one;
        [SerializeField] private float _sensivity = 5;
        [SerializeField] private float _smooth = 1f;
        [SerializeField] private Camera _camera;
        [SerializeField] private bool _enabled;

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
            _prevPoint = Vector3.zero;
            _currentPoint = Vector3.zero;
            _desiredDelta = Vector3.zero;
        }
        public void Update()
        {
            if (!_enabled)
                return;
            
            if (Input.GetMouseButton(0))
            { 
                _currentPoint = Input.mousePosition;
                if (_prevPoint == Vector3.zero)
                    _prevPoint = _currentPoint;
                
                _desiredDelta  = _currentPoint - _prevPoint;
                _desiredDelta *= (_sensivity * Time.deltaTime);
                _frameCounter++;
                if (_frameCounter >= _updatePrevFramesInterval)
                {
                    _frameCounter = 0;
                    _prevPoint = _currentPoint;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                _prevPoint = Vector3.zero;
                _currentPoint = Vector3.zero;
                _desiredDelta = Vector3.zero;
            }
            _currentDelta = Vector3.Lerp(_currentDelta, _desiredDelta, Time.deltaTime * _smooth);

            transform.rotation *= Quaternion.Euler(_invert.y * _currentDelta.y, _invert.x * _currentDelta.x, 0);

        }
    }
}