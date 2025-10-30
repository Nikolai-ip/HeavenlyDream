using System.Collections.Generic;
using UnityEngine;

namespace _Game.Source.Common
{
    public class CurveGenerator
    {
        public List<Vector3> GerCurve(Vector3 from, Vector3 to, Vector3 weightPoint, float step)
        {
            var curve = new List<Vector3>();
            float distance = Vector3.Distance(from, to);
            if (distance <= 0) return curve;
            int lineCount = Mathf.CeilToInt(distance/step);
            for (int i = 0; i <= lineCount; i++)
            {
                float t = i / (float)lineCount;
                Vector3 point = QuadraticBezier(from, weightPoint, to, t);
                curve.Add(point);
            }
            return curve;
        }
        
        private Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            return (1 - t) * (1 - t) * a + 2 * (1 - t) * t * b + t * t * c;
        }
    }
}