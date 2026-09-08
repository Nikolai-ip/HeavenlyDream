using _Game.Source.Domain.Plane.Component;
using _Game.Source.Presenter;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.AttackWingMiniGame
{
    public class FixComponentPhase
    {
        private HandShaker _handShaker;
        private readonly AirplaneModelView _airplaneView;
        private readonly Transform _fixableComponentTr;
        private readonly float _distanceToAbleFix;


        public FixComponentPhase(HandShaker handShaker, AirplaneModelView airplaneView, Transform fixableComponentTr, float distanceToAbleFix)
        {
            _handShaker = handShaker;
            _airplaneView = airplaneView;
            _fixableComponentTr = fixableComponentTr;
            _distanceToAbleFix = distanceToAbleFix;
        }

        public async UniTask<float> ProcessPhase(AxisType axisType)
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
                case AxisType.Horizontal:
                    return _fixableComponentTr.localPosition.x;
                case AxisType.Vertical:
                    return _fixableComponentTr.localPosition.y;
                default:
                    return 0;
            }            
        }

        private void SetUpHandShake(AxisType axisType)
        {
            _handShaker.SetFreezeAxis(axisType != AxisType.Vertical,axisType != AxisType.Horizontal);
        }


        private bool IsNearToAttachPoint()
        {
            return Vector2.Distance(_airplaneView.Center, _fixableComponentTr.position)
                   <= _distanceToAbleFix;
        }
    }
}