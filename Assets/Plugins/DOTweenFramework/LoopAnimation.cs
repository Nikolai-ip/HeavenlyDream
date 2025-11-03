using DG.Tweening;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    public class LoopAnimation: TweenAnimation
    {
        [SerializeField] private TweenAnimation _animation;
        [SerializeField] private LoopType _loopType;

        public override Tween Play()
        {
            Tween = _animation.Play().SetLoops(-1, _loopType);
            return Tween;
        }
    }
}