using System;
using System.Collections.Generic;
using _Game.Source.Application.Airplane.Adders;
using _Game.Source.Application.MiniGameUseCases.Context;
using _Game.Source.Common;
using _Game.Source.Domain.MiniGames;
using _Game.Source.Domain.Plane;
using _Game.Source.Domain.Plane.Component;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application.MiniGameUseCases.AttackWingMiniGame
{
    public class AttachWingController: ICommandMono
    {
        [Inject] private PlaneComponentContainer _planeComponentContainer;
        [Inject] private MiniGamesUnityContextContainer _contextContainer;
        private AttachWingMiniGameContext _ctx;
        private ShakableComponentAttacher _wingInstance;
        [SerializeField] private MiniGameManager _miniGameManager;
        
        //Start
        public override void Execute()
        {
            _ctx ??= _contextContainer.GetContext<AttachWingMiniGameContext>(MiniGameType.AttachWing);
            _wingInstance = Instantiate(_ctx.WingPrefab, _ctx.AirplanePresenter.transform, true);
            _wingInstance.transform.position = _ctx.WingSpawnPoint.position;
            _wingInstance.transform.localRotation = _ctx.WingSpawnPoint.localRotation;
            _wingInstance.Construct(_ctx.AirplanePresenter);
            _wingInstance.OnComponentAttached += OnComponentAttached;
        }

        private void OnComponentAttached(List<float> diffs, List<AxisType> axis)
        {
            _wingInstance.OnComponentAttached -= OnComponentAttached;
            
            _planeComponentContainer.AddComponent(
                new WingComponent(diffs[0],
                diffs[1]), 
                PlaneComponentQuantityMode.Single);
            
            _miniGameManager.ExitMiniGame();
        }

        private void OnDisable()
        {
            if (_wingInstance != null)
                _wingInstance.OnComponentAttached -= OnComponentAttached;
            
        }
    }
}