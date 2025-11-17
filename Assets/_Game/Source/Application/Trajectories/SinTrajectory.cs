using System;
using System.Collections.Generic;
using _Game.Source.Common;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Source
{
    public class SinTrajectory: Trajectory
    {
        [SerializeField] private float _amplitude;
        [SerializeField] private float _frequency;
        public override event Action<List<Vector3>> OnTrajectoryCreated;
        public override void CreateTrajectoryPoints(Vector3 startPosition)
        {
            TrajectoryPoints.Clear();
            Vector2 dir = Direction.normalized;
            Vector2 normal = new Vector2(-dir.y, dir.x);

            for (int i = 0; i < PointsCount; i++)
            {
                float t = i / (float)(PointsCount - 1);          // 0..1
                float distance = t * Length;

                Vector3 basePoint = startPosition + (Vector3)(dir * distance);
                float offset = Mathf.Sin(t * _frequency * Mathf.PI * 2f) * _amplitude;

                Vector3 point = basePoint + (Vector3)(normal * offset);
                TrajectoryPoints.Add(point);
            }

            OnTrajectoryCreated?.Invoke(TrajectoryPoints);
        }
        
    }
}