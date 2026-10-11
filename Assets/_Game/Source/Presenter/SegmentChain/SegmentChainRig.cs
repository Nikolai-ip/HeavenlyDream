using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Source.Presenter.SegmentChain
{
    public enum ChainAxis
    {
        Z,
        X,
        Y
    }

    [ExecuteAlways]
    public class SegmentChainRig : MonoBehaviour
    {
        private const int FabrikIterations = 16;
        private const float DistanceEpsilon = 1e-4f;

        [SerializeField] private List<SegmentChain> _chains = new List<SegmentChain>();

        private static readonly Color[] _chainColors =
        {
            new Color(1f, 0.85f, 0.2f),
            new Color(0.3f, 0.8f, 1f),
            new Color(1f, 0.45f, 0.7f),
            new Color(0.5f, 1f, 0.55f)
        };

        private void OnEnable()
        {
            Solve();
        }

        private void LateUpdate()
        {
            Solve();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall -= SolveIfAlive;
            UnityEditor.EditorApplication.delayCall += SolveIfAlive;
        }

        private void SolveIfAlive()
        {
            UnityEditor.EditorApplication.delayCall -= SolveIfAlive;
            if (this == null)
                return;

            Solve();
        }
#endif

        private void OnDrawGizmosSelected()
        {
            if (_chains == null)
                return;

            for (int chainIndex = 0; chainIndex < _chains.Count; chainIndex++)
            {
                SegmentChain chain = _chains[chainIndex];
                if (chain == null || chain.bendPoints == null || chain.bendPoints.Count == 0)
                    continue;

                Transform chainRoot = ResolveRoot(chain);
                Gizmos.color = _chainColors[chainIndex % _chainColors.Length];
                Vector3 previous = chainRoot.position;
                for (int pointIndex = 0; pointIndex < chain.bendPoints.Count; pointIndex++)
                {
                    Vector3 point = chainRoot.TransformPoint(chain.bendPoints[pointIndex]);
                    Gizmos.DrawLine(previous, point);
                    Gizmos.DrawSphere(point, SphereRadius(previous, point));
                    previous = point;
                }
            }
        }

        public void Solve()
        {
            if (_chains == null || !isActiveAndEnabled || !gameObject.scene.IsValid())
                return;

            for (int i = 0; i < _chains.Count; i++)
            {
                if (_chains[i] != null)
                    SolveChain(_chains[i]);
            }
        }

        private void SolveChain(SegmentChain chain)
        {
            Transform chainRoot = ResolveRoot(chain);
            EnsureBendPoints(chain, chainRoot);
            int count = chain.segments.Count;
            if (count == 0 || chain.bendPoints.Count != count)
                return;

            float[] lengths = new float[count];
            for (int i = 0; i < count; i++)
                lengths[i] = MeasureLength(chain.segments[i]);

            int active = FindMovedJoint(chain);
            if (active < 0 && LengthsChanged(chain, lengths))
                active = count - 1;
            if (active < 0)
            {
                PlaceSegments(chain, chainRoot);
                return;
            }

            Vector3[] points = new Vector3[count + 1];
            points[0] = chainRoot.position;
            for (int i = 0; i < count; i++)
                points[i + 1] = chainRoot.TransformPoint(chain.bendPoints[i]);

            Vector3[] tailDirections = new Vector3[count];
            for (int i = 0; i < count; i++)
                tailDirections[i] = SegmentDirection(chain, chainRoot, i);

            Vector3 target = points[active + 1];
            if (IsCollapsed(chain))
            {
                float reach = 0f;
                for (int i = 0; i <= active; i++)
                    reach += lengths[i];
                if (reach > DistanceEpsilon)
                    target = chainRoot.position + chainRoot.forward * reach;
            }

            SolvePrefix(points, lengths, active, target, chainRoot);
            for (int i = active + 1; i < count; i++)
            {
                Vector3 direction = tailDirections[i].sqrMagnitude > 1e-8f
                    ? tailDirections[i].normalized
                    : chainRoot.forward;
                points[i + 1] = points[i] + direction * lengths[i];
            }

            bool bendPointsChanged = false;
            for (int i = 0; i < count; i++)
            {
                Vector3 local = chainRoot.InverseTransformPoint(points[i + 1]);
                if ((chain.bendPoints[i] - local).sqrMagnitude <= 1e-8f)
                    continue;

                chain.bendPoints[i] = local;
                bendPointsChanged = true;
            }

            PlaceSegments(chain, chainRoot, points);
            StoreSolved(chain, lengths);
#if UNITY_EDITOR
            if (bendPointsChanged && !UnityEngine.Application.isPlaying)
                UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        private void PlaceSegments(SegmentChain chain, Transform chainRoot)
        {
            Vector3 start = chainRoot.position;
            for (int i = 0; i < chain.segments.Count; i++)
            {
                Vector3 end = chainRoot.TransformPoint(chain.bendPoints[i]);
                PlaceSegment(chain.segments[i], start, end, chainRoot);
                start = end;
            }
        }

        private void PlaceSegments(SegmentChain chain, Transform chainRoot, Vector3[] worldEnds)
        {
            Vector3 start = chainRoot.position;
            for (int i = 0; i < chain.segments.Count; i++)
            {
                Vector3 end = worldEnds[i + 1];
                PlaceSegment(chain.segments[i], start, end, chainRoot);
                start = end;
            }
        }

        private Transform ResolveRoot(SegmentChain chain)
        {
            return chain.root != null ? chain.root : transform;
        }

        private void EnsureBendPoints(SegmentChain chain, Transform chainRoot)
        {
            if (chain.segments == null)
                chain.segments = new List<ChainSegment>();
            if (chain.bendPoints == null)
                chain.bendPoints = new List<Vector3>();

            int needed = chain.segments.Count;
            if (chain.bendPoints.Count == needed + 1)
                chain.bendPoints.RemoveAt(0);

            while (chain.bendPoints.Count < needed)
            {
                int index = chain.bendPoints.Count;
                Vector3 previous = index == 0 ? Vector3.zero : chain.bendPoints[index - 1];
                float length = MeasureLength(chain.segments[index]);
                Vector3 step = chainRoot.InverseTransformVector(chainRoot.forward * length);
                chain.bendPoints.Add(previous + step);
            }

            if (chain.bendPoints.Count > needed)
                chain.bendPoints.RemoveRange(needed, chain.bendPoints.Count - needed);
        }

        private static int FindMovedJoint(SegmentChain chain)
        {
            int count = chain.bendPoints.Count;
            if (count == 0)
                return -1;
            if (chain.solvedBendPoints == null || chain.solvedBendPoints.Count != count)
                return count - 1;

            int moved = -1;
            for (int i = 0; i < count; i++)
            {
                if ((chain.bendPoints[i] - chain.solvedBendPoints[i]).sqrMagnitude > 1e-8f)
                    moved = i;
            }

            return moved;
        }

        private static bool LengthsChanged(SegmentChain chain, float[] lengths)
        {
            if (chain.solvedLengths == null || chain.solvedLengths.Count != lengths.Length)
                return true;

            for (int i = 0; i < lengths.Length; i++)
            {
                if (Mathf.Abs(chain.solvedLengths[i] - lengths[i]) > DistanceEpsilon)
                    return true;
            }

            return false;
        }

        private static bool IsCollapsed(SegmentChain chain)
        {
            for (int i = 0; i < chain.bendPoints.Count; i++)
            {
                if (chain.bendPoints[i].sqrMagnitude > 1e-6f)
                    return false;
            }

            return true;
        }

        private static Vector3 SegmentDirection(SegmentChain chain, Transform chainRoot, int index)
        {
            bool hasHistory = chain.solvedBendPoints != null
                && chain.solvedBendPoints.Count == chain.bendPoints.Count;
            Vector3 start = index == 0
                ? chainRoot.position
                : chainRoot.TransformPoint(hasHistory
                    ? chain.solvedBendPoints[index - 1]
                    : chain.bendPoints[index - 1]);
            Vector3 end = chainRoot.TransformPoint(hasHistory
                ? chain.solvedBendPoints[index]
                : chain.bendPoints[index]);
            return end - start;
        }

        private static void SolvePrefix(
            Vector3[] points,
            float[] lengths,
            int active,
            Vector3 target,
            Transform chainRoot)
        {
            float maxReach = 0f;
            float longest = 0f;
            for (int i = 0; i <= active; i++)
            {
                maxReach += lengths[i];
                if (lengths[i] > longest)
                    longest = lengths[i];
            }

            float minReach = Mathf.Max(0f, longest - (maxReach - longest));
            Vector3 root = points[0];
            Vector3 offset = target - root;
            float distance = offset.magnitude;
            Vector3 direction = distance > 1e-5f ? offset / distance : chainRoot.forward;
            float clampedDistance = Mathf.Clamp(distance, minReach, maxReach);
            Vector3 clampedTarget = root + direction * clampedDistance;
            if (clampedDistance >= maxReach - DistanceEpsilon)
            {
                float traveled = 0f;
                for (int i = 0; i <= active; i++)
                {
                    traveled += lengths[i];
                    points[i + 1] = root + direction * traveled;
                }

                return;
            }

            points[active + 1] = clampedTarget;
            for (int iteration = 0; iteration < FabrikIterations; iteration++)
            {
                for (int i = active; i >= 0; i--)
                    points[i] = PointAlong(points[i + 1], points[i], lengths[i], direction);

                points[0] = root;
                for (int i = 0; i <= active; i++)
                    points[i + 1] = PointAlong(points[i], points[i + 1], lengths[i], direction);
            }
        }

        private static Vector3 PointAlong(Vector3 from, Vector3 toward, float length, Vector3 fallback)
        {
            Vector3 delta = toward - from;
            Vector3 direction = delta.sqrMagnitude > 1e-8f ? delta.normalized : fallback;
            return from + direction * length;
        }

        private static void StoreSolved(SegmentChain chain, float[] lengths)
        {
            if (chain.solvedBendPoints == null)
                chain.solvedBendPoints = new List<Vector3>(chain.bendPoints.Count);
            CopyPoints(chain.bendPoints, chain.solvedBendPoints);

            if (chain.solvedLengths == null)
                chain.solvedLengths = new List<float>(lengths.Length);
            if (chain.solvedLengths.Count != lengths.Length)
            {
                chain.solvedLengths.Clear();
                for (int i = 0; i < lengths.Length; i++)
                    chain.solvedLengths.Add(lengths[i]);
                return;
            }

            for (int i = 0; i < lengths.Length; i++)
            {
                if (Mathf.Abs(chain.solvedLengths[i] - lengths[i]) > DistanceEpsilon)
                    chain.solvedLengths[i] = lengths[i];
            }
        }

        private static void CopyPoints(List<Vector3> source, List<Vector3> target)
        {
            if (target.Count != source.Count)
            {
                target.Clear();
                for (int i = 0; i < source.Count; i++)
                    target.Add(source[i]);
                return;
            }

            for (int i = 0; i < source.Count; i++)
            {
                if ((target[i] - source[i]).sqrMagnitude > 1e-10f)
                    target[i] = source[i];
            }
        }

        private static float MeasureLength(ChainSegment link)
        {
            if (!TryGetBox(link, out BoxCollider box, out Vector3 axis, out int axisIndex))
                return 0f;

            float size = Mathf.Abs(box.size[axisIndex]);
            Vector3 scaled = Vector3.Scale(axis * size, link.segment.localScale);
            Transform parent = link.segment.parent;
            return parent != null ? parent.TransformVector(scaled).magnitude : scaled.magnitude;
        }

        private static void PlaceSegment(ChainSegment link, Vector3 worldStart, Vector3 worldEnd, Transform chainRoot)
        {
            if (link == null || link.segment == null || link.segment == chainRoot)
                return;
            if (!TryGetBox(link, out BoxCollider box, out Vector3 axis, out int axisIndex))
                return;

            Vector3 span = worldEnd - worldStart;
            if (span.sqrMagnitude < 1e-8f)
                return;

            Vector3 worldDirection = span.normalized;
            Quaternion worldRotation = LookRotation(worldDirection, chainRoot.up)
                * Quaternion.Inverse(LookRotation(axis, Vector3.up));
            Transform segment = link.segment;
            Transform parent = segment.parent;
            Quaternion localRotation = parent != null
                ? Quaternion.Inverse(parent.rotation) * worldRotation
                : worldRotation;
            if (RotationDiffers(segment.localRotation, localRotation))
                segment.localRotation = localRotation;

            Vector3 startLocal = box.center - axis * (Mathf.Abs(box.size[axisIndex]) * 0.5f);
            Vector3 worldPosition = worldStart - segment.TransformVector(startLocal);
            if ((segment.position - worldPosition).sqrMagnitude > 1e-8f)
                segment.position = worldPosition;
        }

        private static bool TryGetBox(ChainSegment link, out BoxCollider box, out Vector3 axis, out int axisIndex)
        {
            box = null;
            axis = Vector3.forward;
            axisIndex = 2;
            if (link == null || link.segment == null)
                return false;

            box = link.segment.GetComponent<BoxCollider>();
            if (box == null)
                return false;

            axisIndex = AxisIndex(link.axis);
            if (Mathf.Abs(box.size[axisIndex]) < 1e-5f)
                return false;

            axis = AxisVector(link.axis);
            return true;
        }

        private static Vector3 AxisVector(ChainAxis axis)
        {
            switch (axis)
            {
                case ChainAxis.X:
                    return Vector3.right;
                case ChainAxis.Y:
                    return Vector3.up;
                default:
                    return Vector3.forward;
            }
        }

        private static int AxisIndex(ChainAxis axis)
        {
            switch (axis)
            {
                case ChainAxis.X:
                    return 0;
                case ChainAxis.Y:
                    return 1;
                default:
                    return 2;
            }
        }

        private static Quaternion LookRotation(Vector3 direction, Vector3 up)
        {
            if (direction.sqrMagnitude < 1e-8f)
                return Quaternion.identity;

            direction.Normalize();
            if (up.sqrMagnitude < 1e-8f || Mathf.Abs(Vector3.Dot(direction, up.normalized)) > 0.999f)
            {
                up = Vector3.Cross(direction, Vector3.right);
                if (up.sqrMagnitude < 1e-8f)
                    up = Vector3.Cross(direction, Vector3.up);
            }

            return Quaternion.LookRotation(direction, up);
        }

        private static bool RotationDiffers(Quaternion a, Quaternion b)
        {
            return Mathf.Abs(Quaternion.Dot(a, b)) < 0.999999f;
        }

        private static float SphereRadius(Vector3 from, Vector3 to)
        {
            return Mathf.Clamp(Vector3.Distance(from, to) * 0.04f, 0.02f, 0.2f);
        }

#if UNITY_EDITOR
        internal void DrawSceneHandles()
        {
            if (_chains == null)
                return;

            for (int chainIndex = 0; chainIndex < _chains.Count; chainIndex++)
            {
                SegmentChain chain = _chains[chainIndex];
                if (chain == null)
                    continue;

                Transform chainRoot = ResolveRoot(chain);
                EnsureBendPoints(chain, chainRoot);
                UnityEditor.Handles.color = _chainColors[chainIndex % _chainColors.Length];
                for (int pointIndex = 0; pointIndex < chain.bendPoints.Count; pointIndex++)
                {
                    Vector3 world = chainRoot.TransformPoint(chain.bendPoints[pointIndex]);
                    UnityEditor.Handles.Label(world, "P" + (pointIndex + 1));
                    UnityEditor.EditorGUI.BeginChangeCheck();
                    Vector3 moved = UnityEditor.Handles.PositionHandle(world, chainRoot.rotation);
                    if (!UnityEditor.EditorGUI.EndChangeCheck())
                        continue;

                    UnityEditor.Undo.RecordObject(this, "Move Bend Point");
                    RecordSegments(chain);
                    chain.bendPoints[pointIndex] = chainRoot.InverseTransformPoint(moved);
                    SolveChain(chain);
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }
        }

        private static void RecordSegments(SegmentChain chain)
        {
            for (int i = 0; i < chain.segments.Count; i++)
            {
                ChainSegment link = chain.segments[i];
                if (link != null && link.segment != null)
                    UnityEditor.Undo.RecordObject(link.segment, "Move Bend Point");
            }
        }
#endif
    }

    [Serializable]
    public class SegmentChain
    {
        [Tooltip("Корень цепочки. Его pivot не двигается. Пусто — берётся объект с этим компонентом.")]
        public Transform root;

        [Tooltip("Сегменты по порядку: A, B, C. Риг пишет их position и rotation.")]
        public List<ChainSegment> segments = new List<ChainSegment>();

        [Tooltip("Конец каждого сегмента в локальных координатах корня. Точка 0 — торец первого сегмента.")]
        public List<Vector3> bendPoints = new List<Vector3>();

        [HideInInspector] public List<Vector3> solvedBendPoints = new List<Vector3>();
        [HideInInspector] public List<float> solvedLengths = new List<float>();
    }

    [Serializable]
    public class ChainSegment
    {
        [Tooltip("Модель сегмента. На этом же объекте нужен BoxCollider.")]
        public Transform segment;

        [Tooltip("Ось BoxCollider, вдоль которой лежит звено. Стыки стоят на торцах этой оси.")]
        public ChainAxis axis = ChainAxis.Z;
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(SegmentChainRig))]
    internal sealed class SegmentChainRigEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            SegmentChainRig rig = (SegmentChainRig)target;
            rig.DrawSceneHandles();
        }
    }
#endif
}
