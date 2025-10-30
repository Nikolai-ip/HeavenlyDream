using System.Collections.Generic;
using _Game.Source.Domain.Plane.Validation;
using UnityEngine;

namespace _Game.Source.Domain.Plane.MiniGames
{
    public class MiniGameLauncher
    {
        private readonly MiniGameLaunchValidator _miniGameLaunchValidator;
        private readonly Dictionary<MiniGameType, MiniGame> _miniGameDictionary;
        private MiniGame _miniGame;

        public MiniGameLauncher(MiniGameLaunchValidator miniGameLaunchValidator, Dictionary<MiniGameType, MiniGame> miniGameDictionary)
        {
            _miniGameLaunchValidator = miniGameLaunchValidator;
            _miniGameDictionary = miniGameDictionary;
        }

        public void RunMiniGame(MiniGameType miniGameType)
        {
            if (_miniGame != null) return;
            if (_miniGameLaunchValidator.MiniGameCanBeLaunched(miniGameType))
            {
                if (_miniGameDictionary.TryGetValue(miniGameType, out _miniGame))
                {
                    _miniGame.Run();
                    _miniGame.OnExit += () => _miniGame = null;
                }
                else
                    Debug.LogException(new KeyNotFoundException($"[MiniGameLauncher]. Failed to run the minigame {miniGameType}"));
            }
        }
    }
}