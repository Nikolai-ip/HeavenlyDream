using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Source
{
    public class ManualTrajectory: Trajectory
    {
        [SerializeField] private List<Transform> _trajectory; 
        public override event Action<List<Vector3>> OnTrajectoryCreated;
        public override void CreateTrajectoryPoints(Vector3 startPosition)
        {
            var diffPoses = _trajectory.Select(tr=>tr.position - _trajectory[0].position);
            TrajectoryPoints.Clear();
            foreach (var diffPos in diffPoses)
            {
                TrajectoryPoints.Add(startPosition + diffPos);
            }
            OnTrajectoryCreated?.Invoke(TrajectoryPoints);
        }
    }
}