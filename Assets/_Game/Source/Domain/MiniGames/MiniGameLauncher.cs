using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Source.Domain.MiniGames
{
    public class MiniGameLauncher: IDisposable
    {
        private readonly MiniGameLaunchValidator _miniGameLaunchValidator;
        private MiniGame _miniGame;
        private readonly MiniGamesContainer _miniGamesContainer;
        public event Action<MiniGameValidationFailureReasonType> OnLaunchFailed;

        public MiniGameLauncher(MiniGameLaunchValidator miniGameLaunchValidator, MiniGamesContainer miniGamesContainer)
        {
            _miniGameLaunchValidator = miniGameLaunchValidator;
            _miniGamesContainer = miniGamesContainer;
        }

        public void RunMiniGame(MiniGameType miniGameType)
        {
            if (_miniGame != null) return;
            var validatorResult = _miniGameLaunchValidator.MiniGameCanBeLaunched(miniGameType);
            if (validatorResult.CanBeLaunched)
            {
                if (_miniGamesContainer.TryGetGame(miniGameType, out _miniGame))
                {
                    _miniGame.Run();
                    _miniGame.OnExit += () => _miniGame = null;
                }
                else
                    Debug.LogException(new KeyNotFoundException($"[MiniGameLauncher]. Failed to run the minigame {miniGameType}"));
            }
            else
            {
                OnLaunchFailed?.Invoke(validatorResult.FailureReason);   
            }
        }

        public void Dispose()
        {
            _miniGame = null;
        }
    }
}