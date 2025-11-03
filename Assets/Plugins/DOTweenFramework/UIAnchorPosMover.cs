using System;
using DG.Tweening;
using Plugins.DOTweenFramework.Utilities;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    public class UIAnchorPosMover: TweenAnimation
    {
        [SerializeField] private RectTransform _rt;
        [SerializeField] private MoveAnimationParams _moveParams;
        public override event Action OnFinished;
        public override event Action OnRewind;

        private void InitTween()
        {
            var startPos = RectPosConverter.ConvertToLocalPos(_rt, _moveParams.StartPoint);
            var endPos = RectPosConverter.ConvertToLocalPos(_rt, _moveParams.EndPoint);
            
            Tween = _rt
                .DOLocalMove(endPos, _moveParams.Duration)
                .From(startPos)
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
            Tween.Goto(_moveParams.Duration);
            Tween.PlayBackwards();
            return Tween;
        }

        public void ForceMoveToStart() => _rt.anchoredPosition = _moveParams.StartPoint.anchoredPosition;

        public void ForceMoveToEnd() => _rt.anchoredPosition = _moveParams.EndPoint.anchoredPosition;
    }
}