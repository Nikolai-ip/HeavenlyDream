using _Game.Source.Common.Services.SceneTransition;
using _Game.Source.Common.Services.SceneTransition.StaticData;
using Plugins.DIHelpers;
using UnityEngine;
using Zenject;

namespace _Game.Source.Infrastructure.Installers
{
    public class SceneLoaderInstaller: SubInstaller
    {
        [SerializeField] private ScenesTransitions_SO  _scenesTransitions;
        public override void InstallBindings(DiContainer Container)
        {
            Container.Bind<LoadingScenesStorage>().AsSingle().WithArguments(_scenesTransitions.ScenesTransitions(), _scenesTransitions.DefaultLoadingScene);
            Container.BindInterfacesTo<ViaLoadingSceneSceneLoader>().AsSingle();
        }
    }
}