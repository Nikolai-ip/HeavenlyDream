using System;
using System.Collections.Generic;
using _Game.Source.Common.Validation;
using _Game.Source.Domain.MiniGames;
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

            var resultTypes = new Dictionary<Type, MiniGameValidationFailureReasonType>
            {
                { typeof(AlwaysTruePredicate), MiniGameValidationFailureReasonType.None }
            };

            var launchValidator = new MiniGameLaunchValidator(validators, resultTypes);

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

            var resultTypes = new Dictionary<Type, MiniGameValidationFailureReasonType>
            {
                { typeof(AlwaysFalsePredicate), MiniGameValidationFailureReasonType.MissingWings }
            };

            var launchValidator = new MiniGameLaunchValidator(validators, resultTypes);

            // Act
            var result = launchValidator.MiniGameCanBeLaunched(MiniGameType.AttachWing);

            // Assert
            Assert.IsFalse(result.CanBeLaunched, "Мини-игра не должна запускаться, если хотя бы один предикат ложен.");
            Assert.AreEqual(MiniGameValidationFailureReasonType.MissingWings, result.FailureReason,
                "Причина отказа должна соответствовать типу ложного предиката.");
        }

        // Негативный сценарий: для мини-игры нет зарегистрированного валидатора
        [Test]
        public void MiniGameCanBeLaunched_WhenValidatorNotFound_ShouldReturnFalseAndLogException()
        {
            // Arrange
            var validators = new Dictionary<MiniGameType, Validator>();
            var resultTypes = new Dictionary<Type, MiniGameValidationFailureReasonType>();

            var launchValidator = new MiniGameLaunchValidator(validators, resultTypes);

            LogAssert.ignoreFailingMessages = true; // подавляем ожидаемый Debug.LogException

            // Act
            var result = launchValidator.MiniGameCanBeLaunched(MiniGameType.AttachWing);

            // Assert
            Assert.IsFalse(result.CanBeLaunched, "Мини-игра не должна запускаться без валидатора.");
            Assert.AreEqual(MiniGameValidationFailureReasonType.None, result.FailureReason);
        }

        // Граничный случай: словарь причин не содержит типа предиката
        [Test]
        public void MiniGameCanBeLaunched_WhenFailerPredicateTypeNotMapped_ShouldReturnFailureReasonNone()
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

            var resultTypes = new Dictionary<Type, MiniGameValidationFailureReasonType>(); // Пустая мапа

            var launchValidator = new MiniGameLaunchValidator(validators, resultTypes);

            // Act
            var result = launchValidator.MiniGameCanBeLaunched(MiniGameType.AttachWing);

            // Assert
            Assert.IsFalse(result.CanBeLaunched);
            Assert.AreEqual(MiniGameValidationFailureReasonType.None, result.FailureReason,
                "Если тип ложного предиката не зарегистрирован, причина должна быть None.");
        }

        // Регрессионный тест: при одинаковом наборе данных результат должен быть детерминированным
        [Test]
        public void MiniGameCanBeLaunched_ShouldBeDeterministic()
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

            var resultTypes = new Dictionary<Type, MiniGameValidationFailureReasonType>
            {
                { typeof(AlwaysTruePredicate), MiniGameValidationFailureReasonType.None }
            };

            var launchValidator = new MiniGameLaunchValidator(validators, resultTypes);

            // Act
            var result1 = launchValidator.MiniGameCanBeLaunched(MiniGameType.AttachWing);
            var result2 = launchValidator.MiniGameCanBeLaunched(MiniGameType.AttachWing);

            // Assert
            Assert.AreEqual(result1.CanBeLaunched, result2.CanBeLaunched);
            Assert.AreEqual(result1.FailureReason, result2.FailureReason,
                "Метод должен возвращать одинаковый результат при одинаковых входных данных.");
        }
    }
}