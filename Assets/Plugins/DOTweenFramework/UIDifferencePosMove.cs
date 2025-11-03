using System;
using DG.Tweening;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    public class UIDifferencePosMove: TweenAnimation
    {
        [SerializeField] private RectTransform _rt;
        [SerializeField] private MoveDXYAnimationParams _moveParams;
        public override event Action OnFinished;
        public override event Action OnRewind;
        public void SetParams(MoveDXYAnimationParams moveParams) => _moveParams = moveParams;
        private void InitTween()
        {
            Vector3 startPos = _rt.anchoredPosition + _moveParams.DStart;
            Vector3 endPos = _rt.anchoredPosition + _moveParams.DEnd;
            
            Tween = _rt
                .DOAnchorPos(endPos, _moveParams.Duration)
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
            Tween.PlayBackwards();
            return Tween;
        }
        
    }
}