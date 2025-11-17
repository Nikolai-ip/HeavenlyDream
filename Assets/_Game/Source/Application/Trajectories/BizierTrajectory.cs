using System;
using System.Collections.Generic;
using _Game.Source.Common;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Source
{
    public class BizierTrajectory: Trajectory
    {
        private readonly CurveGenerator _curveGenerator = new();
        [SerializeField] private List<Vector3> _curveKeyPoints;
        [SerializeField] private float _step;
        public override event Action<List<Vector3>> OnTrajectoryCreated;
        [Button]
        public override void CreateTrajectoryPoints(Vector3 startPosition)
        {
            TrajectoryPoints.Clear();
       
            for (int i = 0; i < _curveKeyPoints.Count - 2; i+=2)
            {
                TrajectoryPoints
                    .AddRange(
                        _curveGenerator.GerCurve(_curveKeyPoints[i], _curveKeyPoints[i+2],  _curveKeyPoints[i+1], _step));
            }
            OnTrajectoryCreated?.Invoke(TrajectoryPoints);
        }
        

    }
}