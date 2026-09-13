using System;
using System.Collections.Generic;
using _Game.Source.Application.MiniGameUseCases;
using _Game.Source.Common.Validation;
using _Game.Source.Domain.MiniGames;
using _Game.Source.Infrastructure.Factories;
using Plugins.DIHelpers;
using UnityEngine;
using Zenject;

namespace _Game.Source.Infrastructure.Installers
{
    public class MiniGameSystemInstaller: SubInstaller
    {
        [SerializeField] private MiniGameService _miniGameService;

        public override void InstallBindings(DiContainer Container)
        {
            Container.Bind<Dictionary<MiniGameType, Validator>>().FromFactory<MiniGameRequirementComponentsValidatorsFactory>().AsCached();
            Container.Bind<MiniGameLaunchValidator>().AsSingle();
            Container.Bind<MiniGameService>().FromInstance(_miniGameService).AsSingle();
        }
    }
}