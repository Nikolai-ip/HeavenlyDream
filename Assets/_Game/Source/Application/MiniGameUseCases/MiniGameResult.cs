using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases
{
    public enum MiniGameResultStatus
    {
        Completed,
        Cancelled,
        Failed
    }

    public readonly struct MiniGameResult
    {
        public Vector3 LocalPosition { get; }
        public Quaternion LocalRotation { get; }
        public float Accuracy { get; }
        public float Error { get; }
        public MiniGameResultStatus Status { get; }

        public MiniGameResult(
            Vector3 localPosition,
            Quaternion localRotation,
            float accuracy,
            float error,
            MiniGameResultStatus status)
        {
            LocalPosition = localPosition;
            LocalRotation = localRotation;
            Accuracy = accuracy;
            Error = error;
            Status = status;
        }

        public static MiniGameResult Completed(
            Vector3 localPosition,
            Quaternion localRotation,
            float accuracy = 1f,
            float error = 0f)
        {
            return new MiniGameResult(
                localPosition,
                localRotation,
                accuracy,
                error,
                MiniGameResultStatus.Completed);
        }

        public static MiniGameResult Cancelled()
        {
            return new MiniGameResult(
                Vector3.zero,
                Quaternion.identity,
                0f,
                1f,
                MiniGameResultStatus.Cancelled);
        }
    }
}
