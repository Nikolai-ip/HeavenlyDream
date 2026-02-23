using System;
using _Game.Source.Common.Services.SceneTransition;
using Cysharp.Threading.Tasks;
using Plugins.MVP;
using Zenject;

namespace _Game.Source.Presenter
{
    public class LoadScenePresenter: IInitializable, IDisposable
    {
        private readonly IViewInteractable _viewInteractable;
        private readonly IAsyncSceneLoader _sceneLoader;
        private readonly string _sceneName;
        
        public LoadScenePresenter(IViewInteractable viewInteractable, string sceneName, IAsyncSceneLoader sceneLoader)
        {
            _viewInteractable = viewInteractable;
            _sceneLoader = sceneLoader;
            _sceneName = sceneName;
        }

        public void Initialize()
        {
            _viewInteractable.Callback += LoadScene;
        }

        private void LoadScene()
        {
            _sceneLoader.LoadScene(_sceneName).Forget();
        }

        public void Dispose()
        {
            _viewInteractable.Callback -= LoadScene;
        }
    }
}