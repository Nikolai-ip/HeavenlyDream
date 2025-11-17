using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Game.Source.Common.Services.SceneTransition
{
    public class ViaLoadingSceneSceneLoader: IAsyncSceneLoader
    {
        private readonly LoadingScenesStorage _loadingScenesStorage;
        private CancellationTokenSource _cts;

        public ViaLoadingSceneSceneLoader(LoadingScenesStorage loadingScenesStorage)
        {
            _loadingScenesStorage = loadingScenesStorage;
        }

        public async UniTask LoadScene(string sceneName)
        {
            _cts = new();
            string loadingScene = _loadingScenesStorage.GetLoadingScene(SceneManager.GetActiveScene().name, sceneName);
            await SceneManager.LoadSceneAsync(loadingScene).ToUniTask();
            await UniTask.WaitUntil(() => _cts.IsCancellationRequested);
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
        }

        public void OnLoadingFinished()
        {
            _cts.Cancel();
        }
    }
}