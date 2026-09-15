using System;
using DG.Tweening;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    public class RotationTween: TweenAnimation
    {
        [SerializeField] private Transform _tr;
        [SerializeField] private RotateAnimationParams _animationParams;
        private Quaternion _originRotate;
        public override event Action OnFinished;
        public override event Action OnRewind;

        private void Start()
        {
            _originRotate = _tr.rotation;
        }

        private void InitTween()
        {
            _tr
                .DOLocalRotate(_animationParams.Rotate, _animationParams.Duration)
                .SetDelay(_animationParams.Delay)
                .SetEase(_animationParams.EaseFunc.Func, _animationParams.EaseFunc.Amplitude,
                    _animationParams.EaseFunc.Period)
                .OnComplete(()=>OnFinished?.Invoke())
                .OnRewind(()=>OnRewind?.Invoke());
        }

        public override Tween Play()
        {
            Tween?.Kill();
            InitTween();
            return Tween;
        }

        public override void ResetAnim()
        {
            _tr.localRotation = _originRotate;
        }
    }
}