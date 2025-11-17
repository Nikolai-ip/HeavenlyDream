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
        private readonly PlaneComponentsErrorParams _params;

        public ErrorPlaneEvaluateFactory(PlaneComponentsErrorParams @params)
        {
            _params = @params;
        }

        public ErrorPlaneEvaluator Create()
        {
            	       
	        var componentsWeight = new Dictionary<Type, float>()
	        {
		        {
			        typeof(FuselageComponent),
			        _params.FuselageErrorParams.Weight
		        },
		        {
			        typeof(WingComponent),
			        _params.WingErrorParams.Weight
		        },
		        {
			        typeof(StabilizerComponent),
			        _params.StabilizerErrorParams.Weight
		        },
		        {
			        typeof(AirplaneFinComponent),
			        _params.FinErrorParams.Weight
		        },
		        {
			        typeof(BalancingWeightComponent),
			        _params.BalancingWeightErrorParams.Weight
		        }
	        };
	        var planeComponentsSimilarityEvaluators =
		        new Dictionary<Type, IPlaneComponentSimilarityEvaluator<PlaneComponent>>()
		        {
			        {
				        typeof(FuselageComponent),
				        new PlaneComponentSimilarityEvaluatorAdapter<FuselageComponent>(
					        new FuselageSimilarityEvaluator(_params.FuselageErrorParams.MaxPolishingDegreeValue, _params.FuselageErrorParams.EvaluateSimilarityFuncDegree,
						        _params.FuselageErrorParams.AbsentOfComponentValue))
			        },
			        {
				        typeof(WingComponent),
				        new PlaneComponentSimilarityEvaluatorAdapter<WingComponent>
				        (new WingComponentSimilarityEvaluator(
					        new WingErrorEvaluateParams(_params.WingErrorParams.MaxVerticalDistance, _params.WingErrorParams.OffsetVerticalSimilarityFuncDegree,
						        _params.WingErrorParams.MaxHorizontalDistance, _params.WingErrorParams.OffsetHorizontalSimilarityDegree, _params.WingErrorParams.AbsentOfComponentValue)))
			        },
			        {
				        typeof(StabilizerComponent),
				        new PlaneComponentSimilarityEvaluatorAdapter<StabilizerComponent>(
					        new StabilizerSimilarityEvaluator(_params.StabilizerErrorParams.MaxCenterOffsetHorizontal, _params.StabilizerErrorParams.EvaluateSimilarityFuncDegree,
						        _params.StabilizerErrorParams.AbsentOfComponentValue))
			        },
			        {
				        typeof(AirplaneFinComponent),
				        new PlaneComponentSimilarityEvaluatorAdapter<AirplaneFinComponent>(
					        new AirplaneFinSimilarityEvaluator(_params.FinErrorParams.MaxCenterOffsetHorizontal, _params.FinErrorParams.EvaluateSimilarityFuncDegree, _params.FinErrorParams.AbsentOfComponentValue))
			        },
			        {
				        typeof(BalancingWeightComponent),
				        new PlaneComponentSimilarityEvaluatorAdapter<BalancingWeightComponent>(
					        new BalancingWeightSimilarityEvaluator(_params.BalancingWeightErrorParams.MaxWeightAmount, _params.BalancingWeightErrorParams.EvaluateSimilarityFuncDegree, _params.BalancingWeightErrorParams.EvaluateSimilarityFuncDegree))
			        }
		        };

	        ErrorPlaneEvaluator errorPlaneEvaluator = new(componentsWeight, planeComponentsSimilarityEvaluators);
	        return errorPlaneEvaluator;
        }
    }
}