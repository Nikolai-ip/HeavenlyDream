using System;
using System.Collections.Generic;
using _Game.Source.Application.TempStorages;
using _Game.Source.Domain.Plane.Component;
using Plugins.DIHelpers;
using UnityEngine;
using Zenject;

namespace _Game.Source.Infrastructure.Installers
{
    public class TrajectorySystemInstaller: SubInstaller
    {
        [SerializeField] private BlendTrajectory _summaryTrajectory;
        [SerializeField] private Trajectory _etaloneTrajectory;
        [SerializeField] private Trajectory _wingDependedTrajectory;
        [SerializeField] private Trajectory _stabilizerDependedTrajectory;
        [SerializeField] private Trajectory _finDependedTrajectory;
        [SerializeField] private Trajectory _balancingDependedTrajectory;

        public override void InstallBindings(DiContainer Container)
        {   
            var errorEvaluateResult = Container.Resolve<AircraftMadeResultStorage>().ErrorEvaluateResult;
            var localErrors = errorEvaluateResult.LocalErrors;
            List<Tuple<Trajectory, float>> trajectoryWeights = new();
            trajectoryWeights.Add(new (_etaloneTrajectory, 1 - errorEvaluateResult.CommonError));
            trajectoryWeights.Add(new (_wingDependedTrajectory, localErrors[AirplaneComponentType.Wing]));
            trajectoryWeights.Add(new (_stabilizerDependedTrajectory, localErrors[AirplaneComponentType.Stabilizer]));
            trajectoryWeights.Add(new (_finDependedTrajectory, localErrors[AirplaneComponentType.Fin]));
            trajectoryWeights.Add(new (_balancingDependedTrajectory, localErrors[AirplaneComponentType.Balancing]));
            
            _summaryTrajectory.Construct(trajectoryWeights);
        }
    }
    
    
}