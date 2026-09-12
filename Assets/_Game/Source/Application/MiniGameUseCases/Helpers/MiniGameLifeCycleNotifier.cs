using System;
using _Game.Source.Domain.MiniGames;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace _Game.Source.Application.MiniGameUseCases
{
    public class MiniGameLifeCycleNotifier: MonoBehaviour
    {
        [SerializeField] private MiniGameType _miniGameType;
        private MiniGame _miniGame;
        public UnityEvent OnMiniGameStarted;
        public UnityEvent OnMiniGameExited;

        [Inject]
        public void Construct(MiniGamesContainer miniGamesContainer)
        {
            miniGamesContainer.TryGetGame(_miniGameType, out _miniGame);
        }

        private void OnEnable()
        {
            _miniGame.OnStart += MiniGameStartedNotify;
            _miniGame.OnExit += MiniGameExitedNotify;
        }
        private void OnDisable()
        {
            _miniGame.OnStart -= MiniGameStartedNotify;
            _miniGame.OnExit -= MiniGameExitedNotify;
        }

        private void MiniGameStartedNotify() => OnMiniGameStarted.Invoke();
        private void MiniGameExitedNotify() => OnMiniGameExited.Invoke();
    }
}