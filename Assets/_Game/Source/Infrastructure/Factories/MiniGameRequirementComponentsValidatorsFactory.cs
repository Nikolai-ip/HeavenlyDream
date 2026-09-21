using System;
using System.Collections.Generic;
using _Game.Source.Common.Validation;
using _Game.Source.Domain.MiniGames;
using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using _Game.Source.Domain.Plane.Validation;
using _Game.Source.Domain.Plane.Validation.Predicates;
using Zenject;

namespace _Game.Source.Infrastructure.Factories
{
    public class MiniGameRequirementComponentsValidatorsFactory: IFactory<Dictionary<MiniGameType, Validator>>
    {
        private readonly PlaneComponentContainer _planeComponentContainer;

        public MiniGameRequirementComponentsValidatorsFactory(PlaneComponentContainer planeComponentContainer)
        {
            _planeComponentContainer = planeComponentContainer;
        }

        public Dictionary<MiniGameType, Validator> Create()
        {
            return new Dictionary<MiniGameType, Validator>()
            {
                {
                    MiniGameType.PolishRail, new Validator(new List<PredicateWithOperator>()
                    {
                        new (new PlaneNotContain(_planeComponentContainer, AirplaneComponentType.Wing), BoolOperatorType.And),
                        new (new PlaneNotContain(_planeComponentContainer, AirplaneComponentType.Stabilizer), BoolOperatorType.And),
                        new (new PlaneNotContain(_planeComponentContainer, AirplaneComponentType.Fin), BoolOperatorType.And),
                    })
                },
                { MiniGameType.AttachWing, new(new List<PredicateWithOperator>()
                {
                    new (new PlaneContainComponentPredicate(_planeComponentContainer, AirplaneComponentType.Fuselage), BoolOperatorType.And),
                })},
                { MiniGameType.AttachStabilizer, new(new List<PredicateWithOperator>()
                {
                    new (new PlaneContainComponentPredicate(_planeComponentContainer, AirplaneComponentType.Fuselage), BoolOperatorType.And),
                })},
                { MiniGameType.AttachFin, new(new List<PredicateWithOperator>()
                {
                    new (new PlaneContainComponentPredicate(_planeComponentContainer, AirplaneComponentType.Fuselage), BoolOperatorType.And),
              //          new (new PlaneContainComponentPredicate(_planeComponentContainer, AirplaneComponentType.Stabilizer), BoolOperatorType.And),
                })},
                { MiniGameType.SetBalancing, new(new List<PredicateWithOperator>()
                {
                    new (new PlaneContainComponentPredicate(_planeComponentContainer, AirplaneComponentType.Fuselage), BoolOperatorType.And),
                    new (new PlaneNotContain(_planeComponentContainer, AirplaneComponentType.Balancing), BoolOperatorType.And)
                })}
            };
        }
    }
}