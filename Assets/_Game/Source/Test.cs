using System;
using System.Collections.Generic;
using System.Data.Common;
using _Game.Source.Domain.ErrorCalculation;
using _Game.Source.Domain.ErrorCalculation.PlaneComponentsSimilarityEvaluation;
using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using Plugins.Pool;
using UnityEngine;

namespace _Game.Source
{
    public class Test: MonoBehaviour
    {
	    [Header("Fuselage")]
	    [SerializeField] private float _maxPolisghinDegreeValue;
	    [SerializeField] private float _errorDegree;
	    [SerializeField] private float _absentOfComponentValue;
	    [SerializeField] private float _fuselageWeight;

	    [Header("Wing")]
	    [SerializeField] private float MaxMountainDistance ;
	    [SerializeField] private float MountainSimilarityGrade;
	    [SerializeField] private float MaxOffsetXDistance;
	    [SerializeField] private float OffsetXSimilarityGrade;
	    [SerializeField] private float AbsentOfComponentValue;
	    [SerializeField] private float _wingWeight;
	    
	    [Header("Stabilizer")]
	    [SerializeField] private float _stabilizerMaxCenterOffsetX;
	    [SerializeField] private float _stabilizerErrorPower;
	    [SerializeField] private float _stabilizerAbsentOfComponentValue;
	    [SerializeField] private float _stabilizerWeight;
	    
	    [Header("Fin")]
	    [SerializeField]  private float _finMaxCenterOffsetXDistance;
	    [SerializeField] private float _finCenterOffsetXSimilarityGrade;
	    [SerializeField] private float _finAbsentOfComponentValue;
	    [SerializeField] private float _finWeight;
	    
	    [Header("BalancingWeight")]
	    [SerializeField] private float _maxWeightAmount;
	    [SerializeField] private float _weightGrade;
	    [SerializeField] private float _balancingWeightAbsentOfComponentValue;
	    [SerializeField] private float _balanceWeight;
        private void Start()
        {
	        List<PlaneComponent> refPlaneComponents = new()
	        {
		        new FuselageComponent(1),
		        new WingComponent(5, 0),
		        new StabilizerComponent(0),
		        new AirplaneFinComponent(0),
		        new BalancingWeightComponent(3)
	        };
	        // List<PlaneComponent> planeComponents = new()
	        // {
		       //  new FuselageComponent(1),
		       //  new WingComponent(5, 0),
		       //  new StabilizerComponent(0),
		       //  new AirplaneFinComponent(0),
		       //  new BalancingWeightComponent(3)
	        // };
	        List<PlaneComponent> planeComponents = new()
	        {
		        new FuselageComponent(0.91f),
		        new WingComponent(4.5f, 1.2f),
		        new StabilizerComponent(0.3f),
		        new AirplaneFinComponent(0.17f),
		        new BalancingWeightComponent(4)
	        };
	        PlaneComponentContainer referencePlane = new PlaneComponentContainer(refPlaneComponents);
	        PlaneComponentContainer plane = new PlaneComponentContainer(planeComponents);

	        var componentsWeight = new Dictionary<Type, float>()
	        {
		        {
			        typeof(FuselageComponent),
			        _fuselageWeight
		        },
		        {
			        typeof(WingComponent),
			        _wingWeight
		        },
		        {
			        typeof(StabilizerComponent),
			        _stabilizerWeight
		        },
		        {
			        typeof(AirplaneFinComponent),
			        _finWeight
		        },
		        {
			        typeof(BalancingWeightComponent),
			        _balanceWeight
		        }
	        };
	        var planeComponentsSimilarityEvaluators =
		        new Dictionary<Type, IPlaneComponentSimilarityEvaluator<PlaneComponent>>()
		        {
			        {
				        typeof(FuselageComponent),
				        new PlaneComponentSimilarityEvaluatorAdapter<FuselageComponent>(
					        new FuselageSimilarityEvaluator(_maxPolisghinDegreeValue, _errorDegree,
						        _absentOfComponentValue))
			        },
			        {
				        typeof(WingComponent),
				        new PlaneComponentSimilarityEvaluatorAdapter<WingComponent>
				        (new WingComponentSimilarityEvaluator(
					        new WingErrorEvaluateParams(MaxMountainDistance, MountainSimilarityGrade,
						        MaxOffsetXDistance, OffsetXSimilarityGrade, AbsentOfComponentValue)))
			        },
			        {
				        typeof(StabilizerComponent),
				        new PlaneComponentSimilarityEvaluatorAdapter<StabilizerComponent>(
					        new StabilizerSimilarityEvaluator(_stabilizerMaxCenterOffsetX, _stabilizerErrorPower,
						        _stabilizerAbsentOfComponentValue))
			        },
			        {
				        typeof(AirplaneFinComponent),
				        new PlaneComponentSimilarityEvaluatorAdapter<AirplaneFinComponent>(
					        new AirplaneFinSimilarityEvaluator(_finMaxCenterOffsetXDistance, _finCenterOffsetXSimilarityGrade, _finAbsentOfComponentValue))
			        },
			        {
				        typeof(BalancingWeightComponent),
				        new PlaneComponentSimilarityEvaluatorAdapter<BalancingWeightComponent>(
					        new BalancingWeightSimilarityEvaluator(_maxWeightAmount, _weightGrade, _balancingWeightAbsentOfComponentValue))
			        }
		        };
	        ErrorPlaneEvaluator errorPlaneEvaluator = new(componentsWeight, planeComponentsSimilarityEvaluators);
	        var evaluateResult = errorPlaneEvaluator.EvaluateError(referencePlane, plane);
	        Debug.Log("Common model error percentage: " + evaluateResult.CommonError);
	        foreach (var localError in evaluateResult.LocalErrors)
	        {
		        Debug.Log($"Plane component type {localError.Key} : Local error: {localError.Value}");
	        }
        }
    }
}