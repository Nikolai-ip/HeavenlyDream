using System;
using _Game.Source.Presenter;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application.MiniGameUseCases
{
    public abstract class MiniGamePhase: MonoBehaviour
    {
        [Inject] protected AirplaneModelView AirplaneModelView;
        [SerializeField] private RotationType _rotation;
        
        public event Action<MiniGameResult> Completed;

        public bool IsActive { get; private set; }

        public void Enter()
        {
            if (IsActive)
                return;

            IsActive = true;
            ApplyView(OnEnter);
        }

        protected virtual void ApplyView(Action callback)
        {
            AirplaneModelView.Rotate(_rotation, callback);
        }

        public void Tick()
        {
            if (IsActive)
                OnTick();
        }

        public void Complete(MiniGameResult result)
        {
            if (!IsActive)
                return;

            IsActive = false;
            OnComplete(result);
            Completed?.Invoke(result);
        }

        public void Cancel()
        {
            if (!IsActive)
                return;

            IsActive = false;
            OnCancel();
        }

        public void Exit()
        {
            OnExit();
            IsActive = false;
        }

        protected virtual void OnEnter()
        {
        }

        protected virtual void OnTick()
        {
        }

        protected virtual void OnComplete(MiniGameResult result)
        {
        }

        protected virtual void OnCancel()
        {
        }

        protected virtual void OnExit()
        {
        }

        protected void CompletePhase(MiniGameResult result)
        {
            Complete(result);
        }
    }
}
