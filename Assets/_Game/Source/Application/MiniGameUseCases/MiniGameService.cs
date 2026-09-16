using System;
using System.Collections.Generic;
using _Game.Source.Application.Logging;
using _Game.Source.Domain.MiniGames;
using _Game.Source.Infrastructure;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Game.Source.Application.MiniGameUseCases
{
    public class MiniGameService: MonoBehaviour
    {
        [SerializeField] private Button _exitMiniGameButton;
        [SerializeField] private DictionaryInspector<MiniGameType, MiniGameController> _miniGameControllersInspector;
        private MiniGameLaunchValidator _miniGameLaunchValidator;
        private Dictionary<MiniGameType, MiniGameController> _miniGameControllers;

        private MiniGameController _currentMiniGameController;
        private Action _onExitCallBack;

        public event Action<MiniGameValidationFailureReasonType> OnLaunchFailed;

        [Inject]
        void Construct(MiniGameLaunchValidator miniGameLaunchValidator)
        {
            _miniGameLaunchValidator = miniGameLaunchValidator;
            _miniGameControllers = _miniGameControllersInspector.GetDictionary;
        }

        public void RunMiniGame(MiniGameType miniGameType, Action onExitCallBack = null)
        {
            if (_currentMiniGameController != null)
                return;
            
            var validatorResult = _miniGameLaunchValidator.MiniGameCanBeLaunched(miniGameType);
            if (validatorResult.CanBeLaunched)
            {
                if (_miniGameControllers.TryGetValue(miniGameType, out _currentMiniGameController))
                {
                    _onExitCallBack = onExitCallBack;
                    _currentMiniGameController.StateChanged += OnCurrentMiniGameStateChanged;
                    MiniGameLogger.Instance.Log(()=>$"Start minigame {miniGameType}");
                    _currentMiniGameController.OnMiniGameStart();
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

        private void OnEnable()
        {
            _exitMiniGameButton.onClick.AddListener(TryFinishMiniGame);
        }

        private void TryFinishMiniGame()
        {
            _currentMiniGameController?.OnMiniGameEnd();
        }

        private void OnCurrentMiniGameStateChanged(MiniGameState state)
        {
            if (state != MiniGameState.Completed && state != MiniGameState.Cancelled)
                return;

            _currentMiniGameController.StateChanged -= OnCurrentMiniGameStateChanged;
            _currentMiniGameController = null;
            _onExitCallBack?.Invoke();
            _onExitCallBack = null;
        }

        private void OnDisable()
        {
            _exitMiniGameButton.onClick.RemoveListener(TryFinishMiniGame);
        }
    }
}