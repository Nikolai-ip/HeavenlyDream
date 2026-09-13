using _Game.Source.Application.MiniGameUseCases;
using _Game.Source.Domain.MiniGames;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application
{
    public class MiniGameRunner: MonoBehaviour
    {
        [SerializeField] private MiniGameType _miniGameType;
        private MiniGameService _miniGameLauncher;
        [SerializeField] private bool _disableOnMiniGameExit = true;

        [Inject]
        public void Construct(MiniGameService miniGameLauncher)
        {
            _miniGameLauncher = miniGameLauncher;
        }
        public void RunMiniGame()
        {
            _miniGameLauncher.RunMiniGame(_miniGameType, () =>
            {
                if (_disableOnMiniGameExit)
                    gameObject.SetActive(false);
            });
        }
    }
}