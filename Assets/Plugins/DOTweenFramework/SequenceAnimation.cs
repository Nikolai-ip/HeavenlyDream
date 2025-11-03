using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    public class SequenceAnimation:TweenAnimation
    {
        public override event Action OnFinished;
        public override event Action OnRewind;
        [SerializeField] private List<AnimSequenceElem> _animations;
        private void InitTween()
        {
            var sequence = DOTween.Sequence();
            for (var i = 0; i < _animations.Count; i++)
            {
                var anim = _animations[i];
                if (anim.ExecuteType == ElemExecuteType.Append)
                    sequence.Append(anim.Tween.Play());
                else if (anim.ExecuteType == ElemExecuteType.Join)
                    sequence.Join(anim.Tween.Play());
            }
            sequence
                .OnComplete(()=>OnFinished?.Invoke())
                .OnRewind(()=>OnRewind?.Invoke());
            Tween = sequence;
        }
        public override Tween Play()
        {
            Tween?.Kill();
            InitTween();
            return Tween;
        }

        public override Tween PlayBackwards()
        {
            Play();
            Tween.Goto(int.MaxValue);
            Tween.PlayBackwards();
            return Tween;
        }

        public override void ResetAnim()
        {
            _animations.ForEach(anim=>anim.Tween.ResetAnim());
        }
    }

    [Serializable]
    public class AnimSequenceElem
    {
        [field: SerializeField] public TweenAnimation Tween { get; private set; }
        [field: SerializeField] public ElemExecuteType ExecuteType { get; private set; }
    }

    public enum ElemExecuteType
    {
        Append,
        Join,
    }
}