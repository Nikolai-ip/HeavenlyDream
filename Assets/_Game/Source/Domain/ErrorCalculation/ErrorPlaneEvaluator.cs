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
        private readonly Dictionary<Type, IPlaneComponentSimilarityEvaluator<PlaneComponent>> _similarityPlaneComponentsCalculators;
        private readonly Dictionary<Type, float> _componentWeights;

        public ErrorPlaneEvaluator(Dictionary<Type, float> componentWeights, Dictionary<Type, IPlaneComponentSimilarityEvaluator<PlaneComponent>> similarityPlaneComponentsCalculators)
        {
            _componentWeights = componentWeights;
            _similarityPlaneComponentsCalculators = similarityPlaneComponentsCalculators;
        }

        /// <param name="referencePlane">The object witch will be reference for evaluate similarity</param>
        /// <param name="plane">The comparable object</param>
        /// <returns>Error value [0..1]</returns>
        public ErrorEvaluateResult EvaluateError(PlaneComponentContainer referencePlane, PlaneComponentContainer plane)
		{
            float errorPercentage = 0;
            Dictionary<Type, float> _localErrorsMap = new();
            foreach (var refPlaneComponent in referencePlane.PlaneComponents)
            {
                var type =  refPlaneComponent.GetType();
                if (_similarityPlaneComponentsCalculators.TryGetValue(type,
                        out var similarityEvaluator) && _componentWeights.TryGetValue(type, out float weight))
                {
                    
                    var comparablePlaneComponent = plane.PlaneComponents.
                        FirstOrDefault(component=> component.GetType() == refPlaneComponent.GetType());
                    float  similarity = EvaluateSimilarity(comparablePlaneComponent,
                        refPlaneComponent, similarityEvaluator);
                    
                    similarity *= weight;
                    errorPercentage += similarity;
                    
                    _localErrorsMap.Add(type, similarity);
                }
            }

            errorPercentage = Math.Clamp(errorPercentage, 0, 1);
            return new ErrorEvaluateResult(errorPercentage, _localErrorsMap);
		}
        private float EvaluateSimilarity(PlaneComponent refComponent, PlaneComponent comparablePlaneComponent, IPlaneComponentSimilarityEvaluator<PlaneComponent> similarityEvaluator)
        {
            return comparablePlaneComponent != null ? 
                similarityEvaluator.EvaluateErrorValue(refComponent, comparablePlaneComponent) :
                similarityEvaluator.EvaluateInAbsentOfComparableErrorValue();
        }
    }

    public struct ErrorEvaluateResult
    {
        public float CommonError { get; set; }
        public Dictionary<Type, float> LocalErrors { get; set; }

        public ErrorEvaluateResult(float commonError, Dictionary<Type, float> localErrors)
        {
            CommonError = commonError;
            LocalErrors = localErrors;
        }
    }
}