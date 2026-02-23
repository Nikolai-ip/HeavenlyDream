using System.Collections.Generic;
using UnityEngine;

namespace _Game.Source.Common
{
    public class CurveDrawer:MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _step;
        private readonly CurveGenerator _curveGenerator = new();
        private List<Vector3> _curvePoints;

        public void ClearLine()
        {
            _lineRenderer.positionCount = 0;
            _curvePoints?.Clear();
        }

        public void DrawCurve(List<Vector3> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(i, points[i]);
            }
        }
        public void DrawCurve(Vector3 from, Vector3 to, Vector3 weightPoint)
        {
            _curvePoints = _curveGenerator.GerCurve(from, to, weightPoint, _step);
            for (int i = 0; i < _curvePoints.Count; i++)
            {
                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _curvePoints[i]);
            }
        }


        public void EnableCurve(bool showTrajectory)
        {
            _lineRenderer.enabled = showTrajectory;
        }
    }
}