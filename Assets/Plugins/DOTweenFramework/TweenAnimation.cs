using System;
using DG.Tweening;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    public abstract class TweenAnimation: MonoBehaviour
    {
        protected Tween Tween;
        public virtual Tween Play() => Tween;
        public virtual Tween Play<T>(T args) where T : EventArgs => Tween;
        public virtual Tween PlayBackwards() => Tween;
        public virtual void Stop() => Tween.Pause();
        public virtual void Kill() => Tween?.Kill();
        public virtual void ResetAnim(){}
        public virtual event Action OnFinished;
        public virtual event Action OnRewind;
    }
}