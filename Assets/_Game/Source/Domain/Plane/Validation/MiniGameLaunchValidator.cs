using System.Collections.Generic;
using _Game.Source.Common.Validation;
using _Game.Source.Domain.Plane.MiniGames;
using UnityEngine;

namespace _Game.Source.Domain.Plane.Validation
{
    public class MiniGameLaunchValidator
    {
        private readonly Dictionary<MiniGameType, Validator> _miniGameValidators;

        public MiniGameLaunchValidator(Dictionary<MiniGameType, Validator> miniGameValidators)
        {
            _miniGameValidators = miniGameValidators;
        }

        public bool MiniGameCanBeLaunched(MiniGameType miniGameType)
        {
            if (_miniGameValidators.TryGetValue(miniGameType, out Validator miniGameValidator))
            {
                return miniGameValidator.IsValid();
            }
            Debug.LogException(new KeyNotFoundException($"[MiniGameLaunchValidator]. Failed to check the minigame {miniGameType} for launch"));
            return false;
        }
    }
}