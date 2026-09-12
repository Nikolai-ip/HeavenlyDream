using System;
using System.Collections.Generic;
using _Game.Source.Application.Logging;
using UnityEngine;

namespace _Game.Source.Domain.MiniGames
{
    public class MiniGameLauncher: IDisposable
    {
        private readonly MiniGameLaunchValidator _miniGameLaunchValidator;
        private MiniGame _currentMiniGame;
        private readonly MiniGamesContainer _miniGamesContainer;
        public event Action<MiniGameValidationFailureReasonType> OnLaunchFailed;

        public MiniGameLauncher(MiniGameLaunchValidator miniGameLaunchValidator, MiniGamesContainer miniGamesContainer)
        {
            _miniGameLaunchValidator = miniGameLaunchValidator;
            _miniGamesContainer = miniGamesContainer;
        }

        public void RunMiniGame(MiniGameType miniGameType)
        {
            if (_currentMiniGame != null) return;
            var validatorResult = _miniGameLaunchValidator.MiniGameCanBeLaunched(miniGameType);
            if (validatorResult.CanBeLaunched)
            {
                if (_miniGamesContainer.TryGetGame(miniGameType, out _currentMiniGame))
                {
                    MiniGameLogger.Instance.Log(()=>$"Start minigame {miniGameType}");
                    _currentMiniGame.Run();
                    _currentMiniGame.OnExit += () => _currentMiniGame = null;
                }
                else
                    Debug.LogException(new KeyNotFoundException($"[MiniGameLauncher]. Failed to run the minigame {miniGameType}"));
            }
            else
            {
                MiniGameLogger.Instance.Log(()=>$"Failed Start minigame {miniGameType}. Fail reason: {validatorResult.FailureReason}");
                OnLaunchFailed?.Invoke(validatorResult.FailureReason);   
            }
        }

        public void Dispose()
        {
            _currentMiniGame = null;
        }
    }
}