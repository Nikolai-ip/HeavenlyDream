using Unity.Cinemachine;
using UnityEngine;

namespace _Game.Source.Presenter
{
    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")] // Hide in menu
    public class CameraAirplaneFollower : CinemachineExtension
    {
        [Tooltip("Lock the camera's X position to this value")]
        public float _zPosition = 0;

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (enabled && stage == CinemachineCore.Stage.Finalize)
            {
                var pos = state.GetCorrectedPosition();
                pos.z = _zPosition;
                state.RawPosition = pos;
                state.PositionCorrection = Vector3.zero;
            }
        }
    }
}