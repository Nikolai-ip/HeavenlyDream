using UnityEngine;

namespace Plugins.DOTweenFramework.Utilities
{
    public static class RectPosConverter
    {
        public static Vector3 ConvertToLocalPos(RectTransform a, RectTransform b)
        {
            Vector3[] worldCorners = new Vector3[4];
            b.GetWorldCorners(worldCorners);
            Vector3 worldCenter = (worldCorners[0] + worldCorners[2]) * 0.5f;
            Vector3 localPoint = a.parent.InverseTransformPoint(worldCenter);
            return localPoint;
        }
    }
}