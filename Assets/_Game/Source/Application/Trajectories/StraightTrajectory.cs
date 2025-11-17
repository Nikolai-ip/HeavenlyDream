using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Source
{
    public class StraightTrajectory: Trajectory
    {
        public override event Action<List<Vector3>> OnTrajectoryCreated;
        public override void CreateTrajectoryPoints(Vector3 startPosition)
        {
            TrajectoryPoints.Clear();
            float step = Length / PointsCount;
            for (int i = 0; i < PointsCount; i++)
            {
                var point = startPosition + (Direction * (i * step));
                TrajectoryPoints.Add(point);
            }
            OnTrajectoryCreated?.Invoke(TrajectoryPoints);
        }
        
    }
}