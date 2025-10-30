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
        [SerializeField] private float _length;
        [SerializeField] private float _pointsCount;
        [field: SerializeField] public override List<Vector3> TrajectoryPoints { get; protected set; } = new();
        public override event Action<List<Vector3>> OnTrajectoryChanged;

        private void Start()
        {
            CreateTrajectory();
        }

        [Button]
        private void CreateTrajectory()
        {
            TrajectoryPoints.Clear();
            Vector2 dir = Direction.normalized;
            Vector2 normal = new Vector2(-dir.y, dir.x);
            Vector3 startPos = transform.position;

            for (int i = 0; i < _pointsCount; i++)
            {
                float t = i / (float)(_pointsCount - 1);          // 0..1
                float distance = t * _length;

                Vector3 basePoint = startPos + (Vector3)(dir * distance);
                float offset = Mathf.Sin(t * _frequency * Mathf.PI * 2f) * _amplitude;

                Vector3 point = basePoint + (Vector3)(normal * offset);
                TrajectoryPoints.Add(point);
            }

            OnTrajectoryChanged?.Invoke(TrajectoryPoints);
        }
    }
}