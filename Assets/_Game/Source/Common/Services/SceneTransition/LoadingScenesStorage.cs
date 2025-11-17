using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace _Game.Source.Common.Services.SceneTransition
{
    public class LoadingScenesStorage
    {
        private readonly Dictionary<Tuple<string, string>, string> _loadingScenes;
        private readonly string _defaultLoadingScene;

        public LoadingScenesStorage(Dictionary<Tuple<string, string>, string> loadingScenes, string defaultLoadingScene)
        {
            _loadingScenes = loadingScenes;
            _defaultLoadingScene = defaultLoadingScene;
        }


        public string GetLoadingScene(string currentSceneName, string loadingSceneName)
        {
            return _loadingScenes.GetValueOrDefault(Tuple.Create(currentSceneName, loadingSceneName), _defaultLoadingScene);
        }
    }
}