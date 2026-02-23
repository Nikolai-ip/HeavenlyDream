using System;
using UnityEngine;

namespace _Game.Source.Presenter
{
    public class LinearVelocityPresenter: MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _lengthCoefficient = 1;
        [SerializeField] private bool _showVelocity = true;

        private void Start()
        {
            _lineRenderer.positionCount = 2;
        }

        private void FixedUpdate()
        {
            _lineRenderer.enabled = _showVelocity;
            Vector3 velocity = _rigidbody.linearVelocity;
            _lineRenderer.SetPosition(0, _rigidbody.position);
            _lineRenderer.SetPosition(1, _rigidbody.position + velocity * _lengthCoefficient);
        }
    }
}