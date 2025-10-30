using System;
using System.Collections.Generic;
using _Game.Source.Common;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Source.Presenter
{
    [ExecuteInEditMode]
    public class TrajectoryPresenter: MonoBehaviour
    {
        [SerializeField] private CurveDrawer _curveDrawer;
        [SerializeField] private Trajectory _trajectory;
        [SerializeField] private float _radius;
        [SerializeField] private Color _color;
        
        private void OnDrawGizmos()
        {
            if (_trajectory.TrajectoryPoints == null) return;
            DrawPoints(_trajectory.TrajectoryPoints, _radius, _color); 
        }

        private void OnEnable()
        {
            _trajectory.OnTrajectoryChanged += ShowTrajectory;
        }

        private void OnDisable()
        {
            _trajectory.OnTrajectoryChanged -= ShowTrajectory;
        }

        private void ShowTrajectory(List<Vector3> trajectory)
        {
            _curveDrawer.ClearLine();
            _curveDrawer.DrawCurve(trajectory);
        }
        private void DrawPoints(List<Vector3> points, float radius, Color color)
        {
            Gizmos.color = color;
            foreach (var point in points)
            {
                Gizmos.DrawSphere(point, radius);
            }
        }
    }
}