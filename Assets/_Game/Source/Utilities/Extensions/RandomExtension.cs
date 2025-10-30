using System;
using UnityEngine;
using Random = System.Random;

namespace _Game.Source.Utilities.Extensions
{
    public static class RandomExtension
    {
        public static float RandomNormal(this Random self, float offset, float stdDev = 0.3f)
        {
            float u1 = (float)self.NextDouble() % 1;
            float u2 = (float)self.NextDouble() % 1;
            float step = (float)(Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2));
            return Mathf.Clamp(offset + stdDev * step, -1f, 1f);
        }
    }
}