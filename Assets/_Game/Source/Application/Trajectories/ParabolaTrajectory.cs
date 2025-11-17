using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Source
{
    public class ParabolaTrajectory: Trajectory
    {
        [SerializeField] private float _coef;
        public override event Action<List<Vector3>> OnTrajectoryCreated;
        public override void CreateTrajectoryPoints(Vector3 startPosition)
        {
            TrajectoryPoints.Clear();
            float step = Length / PointsCount;
            float angle = Mathf.Atan2(Direction.y, Direction.x);
            for (int i = 0; i < PointsCount; i++)
            {
                float x = i * step;
                float y = _coef * (x * x);
                float rX = x * Mathf.Cos(angle) - y * Mathf.Sin(angle);
                float rY = x * Mathf.Sin(angle) + y * Mathf.Cos(angle);
                Vector3 point = startPosition + new  Vector3(rX, rY, 0);
                TrajectoryPoints.Add(point);
            }
            OnTrajectoryCreated?.Invoke(TrajectoryPoints);
        }


    }
}