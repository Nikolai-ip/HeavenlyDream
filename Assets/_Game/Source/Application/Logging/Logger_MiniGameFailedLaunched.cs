using System;
using _Game.Source.Domain.MiniGames;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application.Logging
{
    public class Logger_MiniGameFailedLaunched: MonoBehaviour
    {
        private MiniGameLauncher _miniGameLauncher;

        [Inject]
        public void Construct(MiniGameLauncher launcher)
        {
            _miniGameLauncher  = launcher;
        }

        private void OnEnable()
        {
            _miniGameLauncher.OnLaunchFailed += LogFailReason;
        }

        private void LogFailReason(MiniGameValidationFailureReasonType failureReasonType)
        {
            Debug.Log("[Log] Failed to launch miniGame, cause is " + failureReasonType);
        }

        private void OnDisable()
        {
            _miniGameLauncher.OnLaunchFailed -= LogFailReason;
        }
    }
}