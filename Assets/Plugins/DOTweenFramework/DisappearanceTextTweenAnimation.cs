using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    public class DisappearanceTextTweenAnimation: TweenAnimation
    {
        [SerializeField] private ColorChangeAnimationParams _colorChangeAnimationParams;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Color _originalColor;
        public override event Action OnFinished;
        public override Tween Play()
        {
            Tween = _text
                .DOColor(_colorChangeAnimationParams.Color, _colorChangeAnimationParams.Duration)
                .SetDelay(_colorChangeAnimationParams.Delay)
                .SetEase(_colorChangeAnimationParams.EaseFunc.Func, _colorChangeAnimationParams.EaseFunc.Amplitude,
                    _colorChangeAnimationParams.EaseFunc.Period)
                .OnComplete(()=>OnFinished?.Invoke());
            return Tween;
        }

        public override void ResetAnim()
        {
            Tween?.Kill();
            _text.color = _originalColor;
        }

        public override void Stop() => Tween.Pause();
    }
}