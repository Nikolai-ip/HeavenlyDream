using System;
using System.Collections.Generic;
using _Game.Source.Common.Validation;
using _Game.Source.Domain.MiniGames;
using _Game.Source.Domain.Plane.Component;
using _Game.Source.Domain.Plane.Validation;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Project.Tests
{
    public class AlwaysTruePredicate : IPredicate
    {
        public bool Evaluate()
        {
            return true;
        }
    }

    public class AlwaysFalsePredicate : IPredicate
    {
        public bool Evaluate()
        {
            return false;
        }
    }

    [TestFixture]
    public class MiniGameLaunchValidatorTests
    {
        // Позитивный сценарий: все валидаторы возвращают true
        [Test]
        public void MiniGameCanBeLaunched_WhenValidatorReturnsTrue_ShouldReturnCanBeLaunchedTrue()
        {
            // Arrange
            var predicate = new AlwaysTruePredicate();
            var validator = new Validator(new List<PredicateWithOperator>
            {
                new(predicate, BoolOperatorType.And)
            });

            var validators = new Dictionary<MiniGameType, Validator>
            {
                { MiniGameType.AttachWing, validator }
            };
            

            var launchValidator = new MiniGameLaunchValidator(validators);

            // Act
            var result = launchValidator.MiniGameCanBeLaunched(MiniGameType.AttachWing);

            // Assert
            Assert.IsTrue(result.CanBeLaunched, "Мини-игра должна запускаться, если все предикаты возвращают true.");
            Assert.AreEqual(MiniGameValidationFailureReasonType.None, result.FailureReason);
        }

        
        // Негативный сценарий: один из предикатов возвращает false
        [Test]
        public void MiniGameCanBeLaunched_WhenValidatorFails_ShouldReturnCanBeLaunchedFalseAndCorrectFailureReason()
        {
            // Arrange
            var predicate = new AlwaysFalsePredicate();
            var validator = new Validator(new List<PredicateWithOperator>
            {
                new(predicate, BoolOperatorType.And)
            });

            var validators = new Dictionary<MiniGameType, Validator>
            {
                { MiniGameType.AttachWing, validator }
            };

            var launchValidator = new MiniGameLaunchValidator(validators);

            // Act
            var result = launchValidator.MiniGameCanBeLaunched(MiniGameType.AttachWing);

            // Assert
            Assert.IsFalse(result.CanBeLaunched, "Мини-игра не должна запускаться, если хотя бы один предикат ложен.");
            Assert.AreEqual(MiniGameValidationFailureReasonType.MissingComponent, result.FailureReason,
                "Причина отказа должна соответствовать типу ложного предиката.");
            Assert.AreEqual(AirplaneComponentType.Wing, result.FailReasonComponent,
                "Причина некорректности компонента самолета должа соответствовать типу ложного предиката.");
        }

        // Негативный сценарий: для мини-игры нет зарегистрированного валидатора
        [Test]
        public void MiniGameCanBeLaunched_WhenValidatorNotFound_ShouldReturnFalseAndLogException()
        {
            // Arrange
            var validators = new Dictionary<MiniGameType, Validator>();

            var launchValidator = new MiniGameLaunchValidator(validators);

            LogAssert.ignoreFailingMessages = true; // подавляем ожидаемый Debug.LogException

            // Act
            var result = launchValidator.MiniGameCanBeLaunched(MiniGameType.AttachWing);

            // Assert
            Assert.IsFalse(result.CanBeLaunched, "Мини-игра не должна запускаться без валидатора.");
            Assert.AreEqual(MiniGameValidationFailureReasonType.None, result.FailureReason);
        }
      
    }
}