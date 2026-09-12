using System;
using System.Collections.Generic;
using _Game.Source.Common.Validation;
using _Game.Source.Domain.Plane.Component;
using _Game.Source.Domain.Plane.Validation.Predicates;
using UnityEngine;

namespace _Game.Source.Domain.MiniGames
{
    public class MiniGameLaunchValidator
    {
        private readonly Dictionary<MiniGameType, Validator> _miniGameValidators;
        public MiniGameLaunchValidator(Dictionary<MiniGameType, Validator> miniGameValidators)
        {
            _miniGameValidators = miniGameValidators;
        }

        public MiniGameValidationResult MiniGameCanBeLaunched(MiniGameType miniGameType)
        {
            if (_miniGameValidators.TryGetValue(miniGameType, out Validator miniGameValidator))
            {
                if (miniGameValidator.IsValid(out IPredicate failerPredicate))
                    return MiniGameValidationResult.Success();
                
                GetFailerReason(failerPredicate, out var failReason, out var planeComponentCause);
                return MiniGameValidationResult.Fail(failReason, planeComponentCause);
            }
            
            Debug.LogException(new KeyNotFoundException($"[MiniGameLaunchValidator]. Failed to check the minigame {miniGameType} for launch"));
            return MiniGameValidationResult.CouldntBeEstablished();
        }

        private void GetFailerReason(IPredicate failerPredicate, out MiniGameValidationFailureReasonType failReason, out AirplaneComponentType planeComponentCause)
        {
            failReason = MiniGameValidationFailureReasonType.None;
            planeComponentCause = AirplaneComponentType.None;
            
            if (failerPredicate is PlaneContainComponentPredicate planeContainPredicate)
            {
                planeComponentCause = planeContainPredicate.ComponentType;
                if (planeContainPredicate is PlaneNotContain)
                    failReason = MiniGameValidationFailureReasonType.MissingComponent;
                else
                    failReason = MiniGameValidationFailureReasonType.AlreadyHasComponent;
                
            }
        }
    }

    public struct MiniGameValidationResult
    {
        public bool CanBeLaunched { get; }
        public MiniGameValidationFailureReasonType FailureReason { get; }
        public AirplaneComponentType FailReasonComponent { get; }

        private MiniGameValidationResult(bool canBeLaunched, MiniGameValidationFailureReasonType failureReason, AirplaneComponentType failReasonComponent)
        {
            CanBeLaunched = canBeLaunched;
            FailureReason = failureReason;
            FailReasonComponent = failReasonComponent;
        }

        public static MiniGameValidationResult Success()
        {
            return new MiniGameValidationResult(true, MiniGameValidationFailureReasonType.None, AirplaneComponentType.None);
        }

        public static MiniGameValidationResult Fail(MiniGameValidationFailureReasonType failureReasonType,
            AirplaneComponentType failReasonComponent)
        {
            return new MiniGameValidationResult(false, failureReasonType, failReasonComponent);
        }

        public static MiniGameValidationResult CouldntBeEstablished()
        {
            return new MiniGameValidationResult();
        }
    }
    public enum MiniGameValidationFailureReasonType
    {
        None,
        MissingComponent,
        AlreadyHasComponent
    }
}