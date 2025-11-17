using Cysharp.Threading.Tasks;

namespace _Game.Source.Common.Services.SceneTransition
{
    public interface IAsyncSceneLoader
    {
        UniTask LoadScene(string sceneName);
        void OnLoadingFinished();
    }
}