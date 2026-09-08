using System;
using System.Collections.Generic;
using _Game.Source.Application.MiniGameUseCases.Context;
using _Game.Source.Common.Validation;
using _Game.Source.Domain.MiniGames;
using _Game.Source.Domain.Plane.Validation;
using _Game.Source.Infrastructure.Factories;
using Plugins.DIHelpers;
using UnityEngine;
using Zenject;

namespace _Game.Source.Infrastructure.Installers
{
    public class MiniGameSystemInstaller: SubInstaller
    {
        [SerializeField] private MiniGamesUnityContextContainer _unityContextContainer;
        public override void InstallBindings(DiContainer Container)
        {
            Container.Bind<Dictionary<MiniGameType, Validator>>().FromFactory<MiniGameValidatorsFactory>().AsCached();
            Container.Bind<Dictionary<Type, MiniGameValidationFailureReasonType>>().FromFactory<MiniGameValidatorsFactory>().AsCached();
            Container.Bind<Dictionary<MiniGameType, MiniGame>>().FromFactory<MiniGamesFactory>().AsCached();
            Container.Bind<MiniGamesUnityContextContainer>().FromInstance(_unityContextContainer).AsSingle();
            Container.Bind<MiniGamesContainer>().AsSingle();
            Container.Bind<MiniGameLaunchValidator>().AsSingle();
            Container.BindInterfacesAndSelfTo<MiniGameLauncher>().AsSingle();
        }
    }
}