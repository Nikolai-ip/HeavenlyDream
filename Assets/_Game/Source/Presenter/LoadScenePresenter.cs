using System;
using _Game.Source.Common.Services.SceneTransition;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using Zenject;

namespace _Game.Source.Presenter
{
    public class LoadScenePresenter: IInitializable, IDisposable
    {
        private readonly IAsyncSceneLoader _sceneLoader;
        private readonly string _sceneName;
        private ButtonView _continueButton;
        
        public LoadScenePresenter(string sceneName, IAsyncSceneLoader sceneLoader, ButtonView continueButton)
        {
            _sceneLoader = sceneLoader;
            _continueButton = continueButton;
            _sceneName = sceneName;
        }

        public void Initialize()
        {
            _continueButton.Callback += LoadScene;
        }

        private void LoadScene()
        {
            _sceneLoader.LoadScene(_sceneName).Forget();
        }

        public void Dispose()
        {
            _continueButton.Callback -= LoadScene;
        }
    }
}