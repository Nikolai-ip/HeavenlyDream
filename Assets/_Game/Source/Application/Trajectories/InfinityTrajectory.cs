using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Source
{
    public class InfinityTrajectory: Trajectory
    {
        [SerializeField] private Trajectory _trajectory; 
        [SerializeField] private Vector3 _generateTrajectoryDirection;
        [SerializeField] private int _generatedPointsCount;

        public override event Action<List<Vector3>> OnTrajectoryCreated
        {
            add => _trajectory.OnTrajectoryCreated += value;
            remove => _trajectory.OnTrajectoryCreated -= value;
        }
        public override List<Vector3> TrajectoryPoints => _trajectory.TrajectoryPoints;

        public override void CreateTrajectoryPoints(Vector3 startPosition)
        {
            _trajectory.CreateTrajectoryPoints(startPosition);
        }

        public override bool TryGetNextPoint(out Vector3 point)
        {
            point = Vector3.zero;
            _index++;
            if (IsTrajectoryOver())
                GenerateTrajectoryPoints();
            point = TrajectoryPoints[_index];
            return true;
        }

        private void GenerateTrajectoryPoints()
        {
            for (int i = 0; i < _generatedPointsCount; i++)
            {
                var newPoint = TrajectoryPoints[^1] + _generateTrajectoryDirection;
                TrajectoryPoints.Add(newPoint);
            }
        }
    }
}