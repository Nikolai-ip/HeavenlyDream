using System;
using System.Collections.Generic;
using _Game.Source.Common.Validation;
using _Game.Source.Domain.Plane.MiniGames;
using _Game.Source.Domain.Plane.Validation;
using _Game.Source.Infrastructure.Factories;
using Plugins.DIHelpers;
using Zenject;

namespace _Game.Source.Infrastructure.Installers
{
    public class MiniGameSystemInstaller: SubInstaller
    {
        public override void InstallBindings(DiContainer Container)
        {
            Container.Bind<Dictionary<MiniGameType, Validator>>().FromFactory<MiniGameValidatorsFactory>().AsCached();
            Container.Bind<Dictionary<Type, MiniGameValidationFailureReasonType>>().FromFactory<MiniGameValidatorsFactory>().AsCached();

            Container.Bind<Dictionary<MiniGameType, MiniGame>>().FromFactory<MiniGamesFactory>().AsCached();
            Container.Bind<MiniGameLaunchValidator>().AsSingle();
            Container.Bind<MiniGameLauncher>().AsSingle();
        }
    }
}