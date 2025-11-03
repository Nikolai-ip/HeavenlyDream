using _Game.Source.Domain.MiniGames;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application.MiniGameUseCases
{
    public class Logger_MiniGameEntryEvents: MonoBehaviour
    {
        [SerializeField] private MiniGameType _miniGameType;
        private MiniGame _miniGame;
        
        [Inject]
        public void Construct(MiniGamesContainer container)
        {
            container.TryGetGame(_miniGameType, out _miniGame);
        }

        private void OnEnable()
        {
            _miniGame.OnStart += LogOnStart;
            _miniGame.OnExit += LogOnExit;
        }

        private void OnDisable()
        {
            _miniGame.OnStart -= LogOnStart;
            _miniGame.OnExit -= LogOnExit;
        }

        private void LogOnExit()
        {
            Debug.Log("[Log]" + _miniGameType + " has been exited");
        }

        private void LogOnStart()
        {
            Debug.Log("[Log]" + _miniGameType + " has started");
        }
        
    }
}