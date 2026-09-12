using System;
using _Game.Source.Presenter;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.AttackWingMiniGame
{
    public abstract class ComponentAttacher: MonoBehaviour
    {
        public abstract event Action<Vector3, Quaternion> OnComponentAttached;

        public abstract void Construct(AirplaneModelView airplaneModelView);
    }
}