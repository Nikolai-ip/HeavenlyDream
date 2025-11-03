using System;
using DG.Tweening;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    public class UIMover: TweenAnimation
    {
        [SerializeField] private RectTransform _rt;
        [SerializeField] private MoveAnimationParams _moveParams;
        public override event Action OnFinished;
        public override event Action OnRewind;

        private void InitTween()
        {
            Tween = _rt
                .DOLocalMove(_moveParams.EndPoint.anchoredPosition, _moveParams.Duration)
                .From(_moveParams.StartPoint.anchoredPosition)
                .SetDelay(_moveParams.Delay)
                .SetEase(_moveParams.EaseFunc.Func, _moveParams.EaseFunc.Amplitude, _moveParams.EaseFunc.Period)
                .OnComplete(() => OnFinished?.Invoke())
                .OnRewind(() => OnRewind?.Invoke());
        }
        public override Tween Play()
        {
            Tween?.Kill();
            InitTween();
            return Tween;
        }

        public override Tween PlayBackwards()
        {
            Tween?.Kill();
            InitTween();
            Tween.PlayBackwards();
            return Tween;
        }
    }
}