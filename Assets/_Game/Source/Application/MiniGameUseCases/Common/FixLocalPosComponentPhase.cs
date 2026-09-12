using _Game.Source.Domain.Plane.Component;
using _Game.Source.Presenter;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.AttackWingMiniGame
{
    public class FixLocalPosComponentPhase
    {
        private HandShaker _handShaker;
        private readonly AirplaneModelView _airplaneView;
        private readonly Transform _fixableComponentTr;
        private readonly float _distanceToAbleFix;


        public FixLocalPosComponentPhase(HandShaker handShaker, AirplaneModelView airplaneView, Transform fixableComponentTr, float distanceToAbleFix)
        {
            _handShaker = handShaker;
            _airplaneView = airplaneView;
            _fixableComponentTr = fixableComponentTr;
            _distanceToAbleFix = distanceToAbleFix;
        }

        public async UniTask<Vector3> ProcessPhase(AxisType axisType)
        {
            _airplaneView.RotateForMiniGame(axisType);
            SetUpHandShake(axisType);
            while (true)
            {
                if (Input.GetMouseButtonDown(0) && IsNearToAttachPoint())
                    break;   
            }

            switch (axisType)
            {
                case AxisType.X:
                    return new Vector3(_fixableComponentTr.localPosition.x, 0, 0);
                case AxisType.Y:
                    return new Vector3(0, _fixableComponentTr.localPosition.y, 0);
                case AxisType.Z:
                    return new Vector3(0, 0, _fixableComponentTr.localPosition.z);
                default:
                    return _fixableComponentTr.localPosition;
            }            
        }

        private void SetUpHandShake(AxisType axisType)
        {
            _handShaker.SetFreezeAxis(axisType != AxisType.Y,axisType != AxisType.X);
        }


        private bool IsNearToAttachPoint()
        {
            return Vector2.Distance(_airplaneView.Center, _fixableComponentTr.position)
                   <= _distanceToAbleFix;
        }
    }
}