using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Source
{
    public abstract class Trajectory: MonoBehaviour
    {
        [SerializeField] private int _index;
        [SerializeField] protected Vector3 Direction;
        [SerializeField] private Vector3 _fallDirection;

        public abstract List<Vector3> TrajectoryPoints { get; protected set; }
        public abstract event Action<List<Vector3>> OnTrajectoryChanged;

        public Vector3 GetNextPoint()
        {
            if (_index < TrajectoryPoints.Count - 2)
            {
              return TrajectoryPoints[_index + 1];
            }

            return CreateNextPointBasedLastPoint();
        }

        private Vector3 CreateNextPointBasedLastPoint()
        {
            var newPoint = TrajectoryPoints[^1] + _fallDirection;
            TrajectoryPoints.Add(newPoint);
            return newPoint;
        }

        public Vector3 GetCurrentPoint() => TrajectoryPoints[_index];

        public void OnKeyPointReached()
        {
            _index++;
        }
        
    }
}