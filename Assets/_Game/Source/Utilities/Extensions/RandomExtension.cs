using System;
using UnityEngine;
using Random = System.Random;

namespace _Game.Source.Utilities.Extensions
{
    public static class RandomExtension
    {
        /// <summary>
        /// Generates a normally distributed random value (Box–Muller transform),
        /// centered at <paramref name="offset"/> with standard deviation <paramref name="stdDev"/>,
        /// then clamped to [-1, 1].
        /// </summary>
        /// <param name="self">Source of uniform [0, 1) randomness.</param>
        /// <param name="offset">Mean of the distribution (before clamping).</param>
        /// <param name="stdDev">Spread around the mean; smaller values cluster closer to <paramref name="offset"/>.</param>
        /// <returns>A float in [-1, 1], typically near <paramref name="offset"/>.</returns>
        public static float RandomNormal(this Random self, float offset, float stdDev = 0.3f)
        {
            float u1 = (float)self.NextDouble() % 1;
            float u2 = (float)self.NextDouble() % 1;
            float step = (float)(Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2));
            return Mathf.Clamp(offset + stdDev * step, -1f, 1f);
        }
    }
}