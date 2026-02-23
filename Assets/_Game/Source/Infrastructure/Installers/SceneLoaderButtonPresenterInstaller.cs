using _Game.Source.Presenter;
using Plugins.DIHelpers;
using UnityEngine;
using Zenject;

namespace _Game.Source.Infrastructure.Installers
{
    public class SceneLoaderButtonPresenterInstaller: SubInstaller
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private ButtonView _buttonView;
        public override void InstallBindings(DiContainer Container)
        {
            Container.BindInterfacesTo<LoadScenePresenter>().AsCached().WithArguments(_buttonView, _sceneName);
        }
    }
}