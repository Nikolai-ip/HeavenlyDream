using System;
using System.Collections.Generic;
using System.Data.Common;
using _Game.Source.Domain.ErrorCalculation;
using _Game.Source.Domain.ErrorCalculation.PlaneComponentsSimilarityEvaluation;
using _Game.Source.Domain.Parameters;
using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using _Game.Source.Infrastructure.Factories;
using UnityEngine;

namespace _Game.Source
{
    public class Test: MonoBehaviour
    {
	    [SerializeField] PlaneComponentsErrorParams _errorParams;
	 
        private void Start()
        {
	        List<PlaneComponent> refPlaneComponents = new()
	        {
		        new FuselageComponent(1),
		        new PlaneComponent(new Vector3(), Quaternion.identity, AirplaneComponentType.Wing),
		        new PlaneComponent(new Vector3(), Quaternion.identity, AirplaneComponentType.Stabilizer),
		        new PlaneComponent(new Vector3(), Quaternion.identity, AirplaneComponentType.Fin),
		        new BalancingWeightComponent(3)
	        };
	 
	        List<PlaneComponent> planeComponents = new()
	        {
		        new FuselageComponent(0.91f),
		        new PlaneComponent(new Vector3(), Quaternion.identity, AirplaneComponentType.Wing),
		        new PlaneComponent(new Vector3(), Quaternion.identity, AirplaneComponentType.Stabilizer),
		        new PlaneComponent(new Vector3(), Quaternion.identity, AirplaneComponentType.Fin),
		        new BalancingWeightComponent(4)
	        };
	        PlaneComponentContainer referencePlane = new PlaneComponentContainer(refPlaneComponents);
	        PlaneComponentContainer plane = new PlaneComponentContainer(planeComponents);
	        ErrorPlaneEvaluator errorPlaneEvaluator = new ErrorPlaneEvaluateFactory(_errorParams).Create();
	        var evaluateResult = errorPlaneEvaluator.EvaluateError(referencePlane, plane);
	        Debug.Log("Common model error percentage: " + evaluateResult.CommonError);
	        foreach (var localError in evaluateResult.LocalErrors)
	        {
		        Debug.Log($"Plane component type {localError.Key} : Local error: {localError.Value}");
	        }
        }
    }
}