using System;
using DG.Tweening;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    public class SRColorChanger: TweenAnimation
    {
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private ColorChangeAnimationParams _animationParams;
        public override event Action OnRewind;
        public override event Action OnFinished;

        private void InitTween(ColorChangeAnimationParams animParams)
        {
            Tween = _sr
                .DOColor(animParams.Color, animParams.Duration)
                .SetDelay(animParams.Delay)
                .From(animParams.OriginColor)
                .SetEase(animParams.EaseFunc.Func, animParams.EaseFunc.Amplitude, animParams.EaseFunc.Period)
                .OnComplete(()=> OnFinished?.Invoke())
                .OnRewind(() => OnRewind?.Invoke());
        }

        private Tween ChangeColor(ColorChangeAnimationParams animParams)
        {
            Tween?.Kill();
            InitTween(animParams);
            return Tween;
        }

        public override Tween Play() => ChangeColor(_animationParams);
        public override Tween Play<T>(T args)
        {
            var scaleArgs = (args as ColorArgs)!;
            var clone = _animationParams.Clone();
            clone.Color = scaleArgs.Color;
            return ChangeColor(clone);
        }
        
        public override Tween PlayBackwards()
        {
            ChangeColor(_animationParams);
            Tween.Goto(_animationParams.Duration);
            Tween.PlayBackwards();
            return Tween;
        }

        public override void ResetAnim()
        {
            _sr.color = _animationParams.OriginColor;
        }
    }
}