using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Source
{
    public abstract class Trajectory: MonoBehaviour
    {
        [ReadOnly] [SerializeField] protected int _index;
        [SerializeField] protected Vector3 StartPosition;
        [SerializeField] protected Vector3 Direction;
        [SerializeField] protected float Length;
        [SerializeField] protected int PointsCount;
        [field: SerializeField] public virtual List<Vector3> TrajectoryPoints { get; protected set; } = new();
        public abstract event Action<List<Vector3>> OnTrajectoryCreated;
        [Button("CreateTrajectoryPoints")]
        protected void CreateTrajectoryPoints()=> CreateTrajectoryPoints(StartPosition);
        public abstract void CreateTrajectoryPoints(Vector3 startPosition);

        public virtual bool TryGetNextPoint(out Vector3 point)
        {
            point =  Vector3.zero;
            _index++;
            if (!IsTrajectoryOver())
            {
                point = TrajectoryPoints[_index];
                return true;
            }

            return false;
        }

        protected bool IsTrajectoryOver()
        {
            return _index > TrajectoryPoints.Count - 1;
        }
        
    }
}