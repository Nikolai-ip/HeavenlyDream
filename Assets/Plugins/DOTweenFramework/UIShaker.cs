using System;
using DG.Tweening;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    public class UIShaker: TweenAnimation
    {
        [SerializeField] private RectTransform _rt;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _strength = 50f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _randomness = 90f;
        [SerializeField] private bool _fadeOut = true;
        [SerializeField] private Ease _ease = Ease.OutQuad;

        private Vector2 _originalPosition;
        public override event Action OnFinished;
        private void Awake()
        {
            _originalPosition = _rt.anchoredPosition;
        }

        public override Tween Play()
        {
            Kill();

            Tween = _rt.DOShakeAnchorPos(
                    duration: _duration,
                    strength: _strength,
                    vibrato: _vibrato,
                    randomness: _randomness,
                    snapping: false,
                    fadeOut: _fadeOut
                )
                .SetEase(_ease)
                .OnComplete(() => OnFinished?.Invoke());

            return Tween;
        }

        public override void ResetAnim()
        {
            Kill();
            _rt.anchoredPosition = _originalPosition;
        }
    }
}