using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.Common
{
    public enum DirectionType
    {
        Forward,
        Right,
        Up
    }

    public static class AttachOrientation
    {
        public static Quaternion GetWorldRotation(Transform attachPoint,
            DirectionType direction,
            bool invertedDirection,
            float yawDegrees = 0f)
        {
            Vector3 up = Vector3.zero;
            Vector3 forward;
    
            if (direction == DirectionType.Up)
            {
                up = invertedDirection? -attachPoint.up : attachPoint.up;
                forward = attachPoint.forward;
            }
            else
            {
                if (direction == DirectionType.Right)
                {
                    up = invertedDirection ? -attachPoint.right : attachPoint.right;
                }
                else if (direction == DirectionType.Forward)
                {
                    up = invertedDirection ? -attachPoint.forward : attachPoint.forward;
                }
                
                forward = Vector3.ProjectOnPlane(attachPoint.up, up);
                if (forward.sqrMagnitude < 1e-6f)
                    forward = Vector3.ProjectOnPlane(attachPoint.forward, up);
                forward.Normalize();
            }

            Quaternion basis = Quaternion.LookRotation(forward, up);
            return basis * Quaternion.Euler(0f, yawDegrees, 0f);
        }
    }
}
