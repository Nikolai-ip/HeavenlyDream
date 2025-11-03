using System;
using DG.Tweening;
using UnityEngine;

namespace Plugins.DOTweenFramework
{
    [Serializable]
    public class ScaleAnimationParams
    {
        [field: SerializeField] public float Delay { get; set; }
        [field: SerializeField] public float Duration { get; set; }
            
        [field: SerializeField] public Vector3 OriginScale { get; set; }
        [field: SerializeField] public Vector3 Scale { get; set; }
        [field: SerializeField] public EaseParams EaseFunc { get; set; }

        public ScaleAnimationParams Clone()
        {
            return (ScaleAnimationParams)MemberwiseClone();
        }
        
    }
    [Serializable]
    public class ColorChangeAnimationParams
    {
        [field: SerializeField] public float Delay { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public Color OriginColor { get; private set; }
        [field: SerializeField] public Color Color { get; set; }
        [field: SerializeField] public EaseParams EaseFunc { get; private set; }
        public ColorChangeAnimationParams Clone()
        {
            return (ColorChangeAnimationParams)MemberwiseClone();
        }
        
    }
    [Serializable]
    public class MoveAnimationParams
    {
        [field: SerializeField] public float Delay { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public RectTransform StartPoint { get; private set; }
        [field: SerializeField] public RectTransform EndPoint { get; private set; }
        [field: SerializeField] public EaseParams EaseFunc { get; private set; }
        
    }
    [Serializable]
    public class MoveDXYAnimationParams
    {
        [field: SerializeField] public float Delay { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public Vector2 DStart { get; private set; }
        [field: SerializeField] public Vector2 DEnd { get; private set; }
        [field: SerializeField] public EaseParams EaseFunc { get; private set; }
        
    }
    [Serializable]
    public class RotateAnimationParams
    {
        [field: SerializeField] public float Delay { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public Vector3 Rotate { get; private set; }
        [field: SerializeField] public EaseParams EaseFunc { get; private set; }
        
    }
    [Serializable]
    public class AnimationParams
    {
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public EaseParams EaseFunc { get; private set; }
        
    }
    [Serializable]
    public class EaseParams
    {
        [field: SerializeField] public Ease Func { get; private set; }
        [field: SerializeField] public float Amplitude { get; private set; }
        [field: SerializeField] public float Period { get; private set; }
    }
}