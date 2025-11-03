using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.DOTweenFramework
{
    public class UIColorChanger: TweenAnimation
    {
        [SerializeField] private Image _image;
        [SerializeField] private ColorChangeAnimationParams _animationParams;
        public override event Action OnRewind;
        public override event Action OnFinished;

        private void InitTween(ColorChangeAnimationParams animParams)
        {
            Tween = _image
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
            _image.color = _animationParams.OriginColor;
        }
    }

    public class ColorArgs: EventArgs
    {
        public Color Color { get; }

        public ColorArgs(Color color)
        {
            Color = color;
        }
    }
}