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
        [SerializeField] private bool _vertical = true;
        public override event Action<List<Vector3>> OnTrajectoryCreated;
        public override void CreateTrajectoryPoints(Vector3 startPosition)
        {
            TrajectoryPoints.Clear();
            Vector2 dir = Direction.normalized;
            Vector3 up = new Vector3(-dir.y, dir.x);
            Vector3 normal = _vertical ? up : Vector3.Cross(dir, up);

            for (int i = 0; i < PointsCount; i++)
            {
                float t = i / (float)(PointsCount - 1);          // 0..1
                float distance = t * Length;

                Vector3 basePoint = startPosition + (Vector3)(dir * distance);
                float offset = Mathf.Sin(t * _frequency * Mathf.PI * 2f) * _amplitude;

                Vector3 point = basePoint + (normal * offset);
                TrajectoryPoints.Add(point);
            }

            OnTrajectoryCreated?.Invoke(TrajectoryPoints);
        }
        
    }
}