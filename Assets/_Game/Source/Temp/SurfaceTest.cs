
using _Game.Source.Application.MiniGameUseCases.Common;
using UnityEngine;

namespace _Game.Source.Temp
{
    public class SurfaceTest: MonoBehaviour
    {
        [SerializeField] private Transform _origin;
        [SerializeField] private DirectionType _relativeDirection;

        private void Update()
        {
            Vector3 dir = Vector3.zero;
            switch (_relativeDirection)
            {
                case DirectionType.Forward: dir = _origin.forward;
                    break;
                case DirectionType.Right: dir = _origin.right;
                    break;
                case DirectionType.Up: dir = _origin.up;
                    break;
            }

            transform.forward = dir;
        }
    }
}