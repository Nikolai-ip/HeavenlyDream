using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace _Game.Source
{
    public class BlendTrajectory: Trajectory
    {
        public override event Action<List<Vector3>> OnTrajectoryCreated;
        private List<Tuple<Trajectory, float>> _trajectoryWeights;
        
        public void Construct(List<Tuple<Trajectory, float>> trajectoryWeights)
        {
            _trajectoryWeights = trajectoryWeights;
            CreateTrajectoryPoints(StartPosition);
        }

        public override void CreateTrajectoryPoints(Vector3 startPosition)
        {
            TrajectoryPoints.Clear();
            float weightsSum = 0;
            _trajectoryWeights.ForEach(tw=>weightsSum += tw.Item2);
            PointsCount /= (int)weightsSum;
            
            for (int i = 0; i < PointsCount; i++)
            {
                Vector3 middlePoint = Vector3.zero;
                foreach (var trajectoryAndWeight in _trajectoryWeights)
                {
                    middlePoint += trajectoryAndWeight.Item1.TrajectoryPoints[i] * trajectoryAndWeight.Item2;
                }
                middlePoint /= weightsSum;
                TrajectoryPoints.Add(middlePoint);
            }
            OnTrajectoryCreated?.Invoke(TrajectoryPoints);
        }
    }
}