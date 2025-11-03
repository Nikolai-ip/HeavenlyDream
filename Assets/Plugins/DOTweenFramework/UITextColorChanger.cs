using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    public class UITextColorChanger: TweenAnimation
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private ColorChangeAnimationParams _animationParams;
        public override event Action OnRewind;
        public override event Action OnFinished;

        private void InitTween(ColorChangeAnimationParams animParams)
        {
            Tween = _text
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
            _text.color = _animationParams.OriginColor;
        }
    }
}