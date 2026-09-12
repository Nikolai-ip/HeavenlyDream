using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Source.Domain.ErrorCalculation.PlaneComponentsSimilarityEvaluation;
using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;

namespace _Game.Source.Domain.ErrorCalculation
{
    public class ErrorPlaneEvaluator
    {
        private readonly Dictionary<AirplaneComponentType, IPlaneComponentSimilarityEvaluator<PlaneComponent>> _similarityPlaneComponentsCalculators;
        private readonly Dictionary<AirplaneComponentType, float> _componentWeights;

        public ErrorPlaneEvaluator(Dictionary<AirplaneComponentType, float> componentWeights, Dictionary<AirplaneComponentType, IPlaneComponentSimilarityEvaluator<PlaneComponent>> similarityPlaneComponentsCalculators)
        {
            _componentWeights = componentWeights;
            _similarityPlaneComponentsCalculators = similarityPlaneComponentsCalculators;
        }

        /// <param name="referencePlane">The object witch will be reference for evaluate similarity</param>
        /// <param name="plane">The comparable object</param>
        /// <returns>Error value [0..1]</returns>
        public ErrorEvaluateResult EvaluateError(PlaneComponentContainer referencePlane, PlaneComponentContainer plane)
		{
            float totalError = 0;
            Dictionary<AirplaneComponentType, float> localErrorsMap = new();
            foreach (var refPlaneComponent in referencePlane.PlaneComponents)
            {
                var type =  refPlaneComponent.Type;
                if (_similarityPlaneComponentsCalculators.TryGetValue(type,
                        out var similarityEvaluator) && _componentWeights.TryGetValue(type, out float errorWeight))
                {
                    
                    var comparablePlaneComponent = plane.PlaneComponents.
                        FirstOrDefault(component=> component.Type == refPlaneComponent.Type);
                    float  error = EvaluateError(comparablePlaneComponent,
                        refPlaneComponent, similarityEvaluator);
                    
                    error *= errorWeight;
                    totalError += error;
                    
                    localErrorsMap.Add(type, error);
                }
            }

            totalError = Math.Clamp(totalError, 0, 1);
            return new ErrorEvaluateResult(totalError, localErrorsMap);
		}
        private float EvaluateError(PlaneComponent refComponent, PlaneComponent comparablePlaneComponent, IPlaneComponentSimilarityEvaluator<PlaneComponent> similarityEvaluator)
        {
            return comparablePlaneComponent != null ? 
                similarityEvaluator.EvaluateErrorValue(refComponent, comparablePlaneComponent) :
                similarityEvaluator.EvaluateInAbsentOfComparableErrorValue();
        }
    }

    public struct ErrorEvaluateResult
    {
        public float CommonError { get; set; }
        public Dictionary<AirplaneComponentType, float> LocalErrors { get; set; }

        public ErrorEvaluateResult(float commonError, Dictionary<AirplaneComponentType, float> localErrors)
        {
            CommonError = commonError;
            LocalErrors = localErrors;
        }
    }
}