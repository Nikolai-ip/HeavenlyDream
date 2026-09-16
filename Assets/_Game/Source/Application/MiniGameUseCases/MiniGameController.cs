using System;
using System.Collections.Generic;
using _Game.Source.Presenter.MiniGames;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases
{
    public class MiniGameController : MonoBehaviour
    {
        [SerializeField] private MiniGameEnvironmentView miniGameEnvironmentView;
        [SerializeField] private List<MiniGamePhase> miniGamePhases = new();
        
        private MiniGamePhase _currentPhase;
        private int _currentPhaseIndex;

        public MiniGameState State { get; private set; } = MiniGameState.Idle;
        public event Action<MiniGameState> StateChanged;

        public virtual void OnMiniGameStart()
        {
            if (State == MiniGameState.Running)
                return;
            
            _currentPhaseIndex = 0;
            SetState(MiniGameState.Running);
            miniGameEnvironmentView.Show();

            if (miniGamePhases.Count > 0)
                EnterCurrentPhase();
        }

        public virtual void OnMiniGameEnd()
        {
            if (State != MiniGameState.Running)
                return;

            _currentPhase?.Cancel();
            ExitCurrentPhase();
            miniGameEnvironmentView.Hide();
            SetState(MiniGameState.Cancelled);
        }

        protected virtual IReadOnlyList<MiniGamePhase> GetPhases()
        {
            return miniGamePhases;
        }

        private void Update()
        {
            _currentPhase?.Tick();
        }

        private void EnterCurrentPhase()
        {
            _currentPhase = miniGamePhases[_currentPhaseIndex];
            _currentPhase.Completed += OnCurrentPhaseCompleted;
            _currentPhase.Enter();
        }

        private void OnCurrentPhaseCompleted(MiniGameResult result)
        {
            if (result.Status != MiniGameResultStatus.Completed)
            {
                ExitCurrentPhase();
                miniGameEnvironmentView.Hide();
                SetState(MiniGameState.Cancelled);
                return;
            }

            ExitCurrentPhase();
            _currentPhaseIndex++;

            if (_currentPhaseIndex < miniGamePhases.Count)
            {
                EnterCurrentPhase();
                return;
            }

            miniGameEnvironmentView.Hide();
            SetState(MiniGameState.Completed);
        }

        private void ExitCurrentPhase()
        {
            if (_currentPhase == null)
                return;

            _currentPhase.Completed -= OnCurrentPhaseCompleted;
            _currentPhase.Exit();
            _currentPhase = null;
        }

        private void SetState(MiniGameState state)
        {
            State = state;
            StateChanged?.Invoke(state);
        }
    }
}