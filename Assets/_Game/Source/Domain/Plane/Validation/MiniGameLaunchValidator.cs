using System;
using System.Collections.Generic;
using _Game.Source.Common.Validation;
using _Game.Source.Domain.Plane.MiniGames;
using UnityEngine;

namespace _Game.Source.Domain.Plane.Validation
{
    public class MiniGameLaunchValidator
    {
        private readonly Dictionary<MiniGameType, Validator> _miniGameValidators;
        private readonly Dictionary<Type, MiniGameValidationFailureReasonType> _miniGameValidationResultTypes;
        public MiniGameLaunchValidator(Dictionary<MiniGameType, Validator> miniGameValidators, Dictionary<Type, MiniGameValidationFailureReasonType> miniGameValidationResultTypes)
        {
            _miniGameValidators = miniGameValidators;
            _miniGameValidationResultTypes = miniGameValidationResultTypes;
        }

        public MiniGameValidationResult MiniGameCanBeLaunched(MiniGameType miniGameType)
        {
            if (_miniGameValidators.TryGetValue(miniGameType, out Validator miniGameValidator))
            {
                bool canBeLaunched = miniGameValidator.IsValid(out IPredicate failerPredicate);
                MiniGameValidationFailureReasonType failerReasonType = MiniGameValidationFailureReasonType.None;
                if (!canBeLaunched && _miniGameValidationResultTypes.TryGetValue(failerPredicate.GetType(), out failerReasonType)){}
                return new MiniGameValidationResult(canBeLaunched, failerReasonType);
            }
            Debug.LogException(new KeyNotFoundException($"[MiniGameLaunchValidator]. Failed to check the minigame {miniGameType} for launch"));
            return new MiniGameValidationResult(false, MiniGameValidationFailureReasonType.None);
        }
    }

    public struct MiniGameValidationResult
    {
        public bool CanBeLaunched { get; }
        public MiniGameValidationFailureReasonType FailureReason { get; }

        public MiniGameValidationResult(bool canBeLaunched, MiniGameValidationFailureReasonType failureReason)
        {
            CanBeLaunched = canBeLaunched;
            FailureReason = failureReason;
        }
    }
    public enum MiniGameValidationFailureReasonType
    {
        None,
        MissingFuselage,
        MissingWings,
        MissingStabilizer,
        MissingFin,
        MissingBalance
    }
}