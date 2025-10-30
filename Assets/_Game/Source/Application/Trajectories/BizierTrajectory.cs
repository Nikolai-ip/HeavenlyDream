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
        public override List<Vector3> TrajectoryPoints { get; protected set; } = new();
        public override event Action<List<Vector3>> OnTrajectoryChanged;

        private void Start()
        {
            CreateTrajectory();
        }
        [Button]
        private void CreateTrajectory()
        {
            TrajectoryPoints.Clear();
       
            for (int i = 0; i < _curveKeyPoints.Count - 2; i+=2)
            {
                TrajectoryPoints
                    .AddRange(
                        _curveGenerator.GerCurve(_curveKeyPoints[i], _curveKeyPoints[i+2],  _curveKeyPoints[i+1], _step));
            }
            OnTrajectoryChanged?.Invoke(TrajectoryPoints);
        }



    }
}