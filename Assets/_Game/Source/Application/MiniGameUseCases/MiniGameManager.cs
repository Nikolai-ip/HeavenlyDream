using System.Collections.Generic;
using _Game.Source.Common;
using _Game.Source.Domain.MiniGames;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application.MiniGameUseCases
{
    public class MiniGameManager: MonoBehaviour
    {
        [SerializeField] private MiniGameType _miniGameType;
        private MiniGame _miniGame;
        [SerializeField] private List<ICommandMono> _onMiniGameStartedCommands;
        [SerializeField] private List<ICommandMono> _onMiniGameExitedCommands;
        [Inject]
        public void Construct(MiniGamesContainer container)
        {
            container.TryGetGame(_miniGameType, out _miniGame);
        }
        private void OnEnable() => _miniGame.OnStart += OnMiniGameStarted;

        private void OnDisable() => _miniGame.OnStart -= OnMiniGameStarted;

        private void OnMiniGameStarted()
        {
            _onMiniGameStartedCommands.ForEach(command=>command.Execute());
        }

        public void ExitMiniGame()
        {
            _onMiniGameExitedCommands.ForEach(command=>command.Execute());
            _miniGame.Exit();
        }
    }
}