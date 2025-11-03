using _Game.Source.Domain.MiniGames;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application
{
    public class MiniGameRunner: MonoBehaviour
    {
        [SerializeField] private MiniGameType _miniGameType;
        private MiniGameLauncher _miniGameLauncher;

        [Inject]
        public void Construct(MiniGameLauncher miniGameLauncher)
        {
            _miniGameLauncher = miniGameLauncher;
        }
        public void RunMiniGame()
        {
            _miniGameLauncher.RunMiniGame(_miniGameType);
        }
    }
}