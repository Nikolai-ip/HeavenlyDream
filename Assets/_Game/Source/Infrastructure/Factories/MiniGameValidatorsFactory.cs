using System;
using System.Collections.Generic;
using _Game.Source.Common.Validation;
using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using _Game.Source.Domain.Plane.MiniGames;
using _Game.Source.Domain.Plane.Validation;
using _Game.Source.Domain.Plane.Validation.Predicates;
using Zenject;

namespace _Game.Source.Infrastructure.Factories
{
    public class MiniGameValidatorsFactory: IFactory<Dictionary<MiniGameType, Validator>>, IFactory<Dictionary<Type, MiniGameValidationFailureReasonType>>
    {
        private readonly PlaneComponentContainer _planeComponentContainer;

        public MiniGameValidatorsFactory(PlaneComponentContainer planeComponentContainer)
        {
            _planeComponentContainer = planeComponentContainer;
        }

        public Dictionary<MiniGameType, Validator> Create()
        {
            return new Dictionary<MiniGameType, Validator>()
            {
                {
                    MiniGameType.PolishRail, new Validator(new List<PredicateWithOperator>())
                },
                { MiniGameType.AttachWing, new(new List<PredicateWithOperator>()
                {
                    new (new PlaneContainComponentPredicate<FuselageComponent>(_planeComponentContainer), BoolOperatorType.Or)
                })},
                { MiniGameType.AttachStabilizer, new(new List<PredicateWithOperator>()
                {
                    new (new PlaneContainComponentPredicate<FuselageComponent>(_planeComponentContainer), BoolOperatorType.Or)
                })},
                { MiniGameType.AttachFin, new(new List<PredicateWithOperator>()
                {
                    new (new PlaneContainComponentPredicate<FuselageComponent>(_planeComponentContainer), BoolOperatorType.And),
                    new (new PlaneContainComponentPredicate<StabilizerComponent>(_planeComponentContainer), BoolOperatorType.And)
                })},
                { MiniGameType.SetBalancing, new(new List<PredicateWithOperator>()
                {
                    new (new PlaneContainComponentPredicate<FuselageComponent>(_planeComponentContainer), BoolOperatorType.And),
                    new (new PlaneContainComponentPredicate<StabilizerComponent>(_planeComponentContainer), BoolOperatorType.And),
                    new (new PlaneContainComponentPredicate<AirplaneFinComponent>(_planeComponentContainer), BoolOperatorType.And),
                })}
            };
        }

        Dictionary<Type, MiniGameValidationFailureReasonType> IFactory<Dictionary<Type, MiniGameValidationFailureReasonType>>.Create()
        {
            return new Dictionary<Type, MiniGameValidationFailureReasonType>()
            {
                {
                    typeof(PlaneContainComponentPredicate<FuselageComponent>),
                    MiniGameValidationFailureReasonType.MissingFuselage
                },
                {
                    typeof(PlaneContainComponentPredicate<WingComponent>),
                    MiniGameValidationFailureReasonType.MissingWings
                },
                {
                    typeof(PlaneContainComponentPredicate<StabilizerComponent>),
                    MiniGameValidationFailureReasonType.MissingStabilizer
                },
                {
                    typeof(PlaneContainComponentPredicate<AirplaneFinComponent>),
                    MiniGameValidationFailureReasonType.MissingFin
                },
                {
                    typeof(PlaneContainComponentPredicate<BalancingWeightComponent>),
                    MiniGameValidationFailureReasonType.MissingBalance
                },
            };
        }
    }
}