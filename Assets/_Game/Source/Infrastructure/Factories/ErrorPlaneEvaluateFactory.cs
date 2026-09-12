using System;
using System.Collections.Generic;
using _Game.Source.Domain.ErrorCalculation;
using _Game.Source.Domain.ErrorCalculation.PlaneComponentsSimilarityEvaluation;
using _Game.Source.Domain.Parameters;
using _Game.Source.Domain.Plane.Component;
using Zenject;

namespace _Game.Source.Infrastructure.Factories
{
    public class ErrorPlaneEvaluateFactory: IFactory<ErrorPlaneEvaluator>
    {
        private readonly PlaneComponentsErrorParams _errorParams;

        public ErrorPlaneEvaluateFactory(PlaneComponentsErrorParams errorParams)
        {
            _errorParams = errorParams;
        }

        public ErrorPlaneEvaluator Create()
        {
            	       
	        var componentsWeight = new Dictionary<AirplaneComponentType, float>()
	        {
		        {
			        AirplaneComponentType.Fuselage,
			        _errorParams.FuselageErrorParams.Weight
		        },
		        {
			        AirplaneComponentType.Wing,
			        _errorParams.WingErrorParams.Weight
		        },
		        {
			        AirplaneComponentType.Stabilizer,
			        _errorParams.StabilizerErrorParams.Weight
		        },
		        {
			        AirplaneComponentType.Fin,
			        _errorParams.FinErrorParams.Weight
		        },
		        {
			        AirplaneComponentType.Balancing,
			        _errorParams.BalancingWeightErrorParams.Weight
		        }
	        };
	        var planeComponentsSimilarityEvaluators =
		        new Dictionary<AirplaneComponentType, IPlaneComponentSimilarityEvaluator<PlaneComponent>>()
		        {
			        {
				        AirplaneComponentType.Fuselage,
				        new PlaneComponentSimilarityEvaluatorAdapter<FuselageComponent>(
					        new FuselageSimilarityEvaluator(
						        _errorParams.FuselageErrorParams.MaxPolishingDegreeValue,
						        _errorParams.FuselageErrorParams.EvaluateSimilarityFuncDegree,
						        _errorParams.FuselageErrorParams.AbsentOfComponentValue))
			        },
			        {
				        AirplaneComponentType.Wing,
				        new PlaneComponentSimilarityEvaluatorAdapter<PlaneComponent>
				        (new TransformSimilarityEvaluator(
					        _errorParams.WingErrorParams.ErrorPower,
					        _errorParams.WingErrorParams.MaxDistanceDiff,
						    _errorParams.WingErrorParams.MaxRotationDiff,
					        _errorParams.WingErrorParams.AbsentOfComponentValue))
			        },
			        {
				        AirplaneComponentType.Stabilizer,
				        new PlaneComponentSimilarityEvaluatorAdapter<PlaneComponent>
					        (new TransformSimilarityEvaluator(
						        _errorParams.StabilizerErrorParams.ErrorPower,
						        _errorParams.StabilizerErrorParams.MaxDistanceDiff,
						        _errorParams.StabilizerErrorParams.MaxRotationDiff,
						        _errorParams.StabilizerErrorParams.AbsentOfComponentValue))
			        },
			        {
				        AirplaneComponentType.Fin,
				        new PlaneComponentSimilarityEvaluatorAdapter<PlaneComponent>
				        (new TransformSimilarityEvaluator(
					        _errorParams.FinErrorParams.ErrorPower,
					        _errorParams.FinErrorParams.MaxDistanceDiff,
					        _errorParams.FinErrorParams.MaxRotationDiff,
					        _errorParams.FinErrorParams.AbsentOfComponentValue))
			        },
			        {
				        AirplaneComponentType.Balancing,
				        new PlaneComponentSimilarityEvaluatorAdapter<BalancingWeightComponent>(
					        new BalancingWeightSimilarityEvaluator(
						        _errorParams.BalancingWeightErrorParams.MaxWeightAmount,
						        _errorParams.BalancingWeightErrorParams.EvaluateSimilarityFuncDegree, 
						        _errorParams.BalancingWeightErrorParams.AbsentOfComponentValue))
			        }
		        };

	        ErrorPlaneEvaluator errorPlaneEvaluator = new(componentsWeight, planeComponentsSimilarityEvaluators);
	        return errorPlaneEvaluator;
        }
    }
}