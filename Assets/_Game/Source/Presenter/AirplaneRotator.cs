using System;
using _Game.Source.Application;
using DG.Tweening;
using UnityEngine;

namespace _Game.Source.Presenter
{
    public class AirplaneRotator: MonoBehaviour
    {
        [SerializeField] private Transform _airplane;
        [SerializeField] private float _rotationDuration;
        [SerializeField] private Ease _rotationEase;
        private Tween _rotationTween;
        [SerializeField] private Vector3 _top = new Vector3(0, 270, 90);
        [SerializeField] private Vector3 _side = new Vector3(0,180,0);
        [SerializeField] private Vector3 _front = new Vector3(0,90,0);

        public void Rotate(RotationType rotation, Action callback)
        {
            _rotationTween?.Kill();
            
            Quaternion target = Quaternion.identity;
            switch (rotation)
            {
                case RotationType.Up:
                    target = Quaternion.Euler(_top);
                    break;
                case RotationType.Right:
                    target = Quaternion.Euler(_side);
                    break;
                case RotationType.Forward:
                    target = Quaternion.Euler(_front);
                    break;
            }
            _rotationTween = _airplane.DOLocalRotateQuaternion(target, _rotationDuration).SetEase(_rotationEase).OnComplete(()=>callback?.Invoke());
        }
    }
}