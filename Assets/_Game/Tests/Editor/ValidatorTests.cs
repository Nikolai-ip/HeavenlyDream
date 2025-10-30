using System;
using System.Collections.Generic;
using _Game.Source.Common.Validation;
using NUnit.Framework;

namespace Project.Tests
{
    class PredicateMock: IPredicate
    {
        private readonly bool _returnValue;

        public PredicateMock(bool returnValue)
        {
            _returnValue = returnValue;
        }

        public bool Evaluate()
        {
            return _returnValue;
        }
    }
    [TestFixture]
    public class ValidatorTests
    {
        private PredicateMock _truePredicateMock;
        private PredicateMock _falsePredicateMock;

        [SetUp]
        public void SetUp()
        {
            _truePredicateMock = new PredicateMock(true);
            _falsePredicateMock = new PredicateMock(false);
        }

        // Позитивный сценарий: все предикаты true с оператором AND — валидация успешна
        [Test]
        public void IsValid_WhenAllPredicatesTrueAndOperatorAnd_ShouldReturnTrue()
        {
            // Arrange
            var predicates = new List<PredicateWithOperator>
            {
                new PredicateWithOperator(_truePredicateMock, BoolOperatorType.And),
                new PredicateWithOperator(_truePredicateMock, BoolOperatorType.And)
            };
            var validator = new Validator(predicates);

            // Act
            var result = validator.IsValid();

            // Assert
            Assert.IsTrue(result, "Все предикаты истинны — результат должен быть true");
        }

        // Позитивный сценарий: хотя бы один предикат true с оператором OR — валидация успешна
        [Test]
        public void IsValid_WhenAnyPredicateTrueAndOperatorOr_ShouldReturnTrue()
        {
            // Arrange
            var predicates = new List<PredicateWithOperator>
            {
                new PredicateWithOperator(_falsePredicateMock, BoolOperatorType.Or),
                new PredicateWithOperator(_truePredicateMock, BoolOperatorType.Or)
            };
            var validator = new Validator(predicates);

            // Act
            var result = validator.IsValid();

            // Assert
            Assert.IsTrue(result, "Хотя бы один предикат истинный — результат должен быть true");
        }

        // Негативный сценарий: все предикаты false с оператором AND — валидация неуспешна
        [Test]
        public void IsValid_WhenAllPredicatesFalseAndOperatorAnd_ShouldReturnFalse()
        {
            // Arrange
            var predicates = new List<PredicateWithOperator>
            {
                new PredicateWithOperator(_falsePredicateMock, BoolOperatorType.And),
                new PredicateWithOperator(_falsePredicateMock, BoolOperatorType.And)
            };
            var validator = new Validator(predicates);

            // Act
            var result = validator.IsValid();

            // Assert
            Assert.IsFalse(result, "Все предикаты ложные — результат должен быть false");
        }

        // Негативный сценарий: все предикаты false с оператором OR — результат false
        [Test]
        public void IsValid_WhenAllPredicatesFalseAndOperatorOr_ShouldReturnFalse()
        {
            // Arrange
            var predicates = new List<PredicateWithOperator>
            {
                new PredicateWithOperator(_falsePredicateMock, BoolOperatorType.Or),
                new PredicateWithOperator(_falsePredicateMock, BoolOperatorType.Or)
            };
            var validator = new Validator(predicates);

            // Act
            var result = validator.IsValid();

            // Assert
            Assert.IsFalse(result, "Ни один предикат не истинен — результат должен быть false");
        }

        // Граничный случай: список предикатов пуст — возвращает true (нет ограничений)
        [Test]
        public void IsValid_WhenNoPredicates_ShouldReturnTrue()
        {
            // Arrange
            var validator = new Validator(new List<PredicateWithOperator>());

            // Act
            var result = validator.IsValid();

            // Assert
            Assert.IsTrue(result, "Пустой список предикатов должен считаться валидным");
        }
        
        // Регрессионный тест: порядок предикатов не должен влиять на результат при одинаковых значениях
        [Test]
        public void IsValid_ShouldBeOrderIndependentForSameValues()
        {
            // Arrange
            var list1 = new List<PredicateWithOperator>
            {
                new PredicateWithOperator(_truePredicateMock, BoolOperatorType.And),
                new PredicateWithOperator(_falsePredicateMock, BoolOperatorType.Or)
            };

            var list2 = new List<PredicateWithOperator>
            {
                new PredicateWithOperator(_falsePredicateMock, BoolOperatorType.Or),
                new PredicateWithOperator(_truePredicateMock, BoolOperatorType.And)
            };

            var v1 = new Validator(list1);
            var v2 = new Validator(list2);

            // Act
            var result1 = v1.IsValid();
            var result2 = v2.IsValid();

            // Assert
            Assert.AreEqual(result1, result2, "Результат не должен зависеть от порядка предикатов");
        }
    }


}
