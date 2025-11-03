using System;
using DG.Tweening;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    public class UIScaler:TweenAnimation
    {
        [SerializeField] private RectTransform _tr;
        [SerializeField] private ScaleAnimationParams _animationParams;
        public override event Action OnRewind;
        public override event Action OnFinished;
        
        private void InitTween(ScaleAnimationParams animParams)
        {
            Tween = _tr
                .DOScale(animParams.Scale, animParams.Duration)
                .SetDelay(animParams.Delay)
                .From(animParams.OriginScale)
                .SetEase(animParams.EaseFunc.Func, animParams.EaseFunc.Amplitude, animParams.EaseFunc.Period)
                .OnComplete(() => OnFinished?.Invoke())
                .OnRewind(() => OnRewind?.Invoke());
        }

        private Tween Scale(ScaleAnimationParams animationParams)
        {
            Tween?.Kill();
            InitTween(animationParams);
            return Tween;
        }

        public override Tween Play()
        {
            return Scale(_animationParams);
        }

        public override Tween Play<T>(T args)
        {
            var clone = _animationParams.Clone();
            var scaleArgs = (args as ScaleArgs)!;
            clone.Scale = scaleArgs.Scale;
            return Scale(clone);
        }

        public override Tween PlayBackwards()
        {
            Scale(_animationParams);
            Tween.Goto(_animationParams.Duration);
            Tween.PlayBackwards();
            return Tween;
        }

        public override void ResetAnim()
        {
            _tr.localScale = _animationParams.OriginScale;
        }
        
    }

    public class ScaleArgs : EventArgs
    {
        public ScaleArgs(Vector3 scale)
        {
            Scale = scale;
        }
        
        public Vector3 Scale { get; }
        public Vector3 From { get; }
    }
}