using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Source.Utilities;
using NaughtyAttributes;
using UnityEngine;

namespace _Game.Source.Common.Services.SceneTransition.StaticData
{
    [CreateAssetMenu(fileName = "ScenesTransitions", menuName = "StaticData/ScenesTransitions")]
    public class ScenesTransitions_SO: ScriptableObject
    {
        [SerializeField] private SceneField _defaultLoadingScene;
        [SerializeField] private SerializableDictionary<ScenesPair, SceneField> _scenesTransitions;

        public string DefaultLoadingScene => _defaultLoadingScene.Scene;

        public Dictionary<Tuple<string, string>, string> ScenesTransitions()
        {
            var dictionary = _scenesTransitions.GetDictionary();
            var keys = dictionary.Keys;
            var values = dictionary.Values;
            var result = new Dictionary<Tuple<string, string>, string>();
            for (int i = 0; i < keys.Count; i++)
            {
                result.Add(new Tuple<string, string>(keys.ElementAt(i).From.Scene, keys.ElementAt(i).To.Scene), values.ElementAt(i).Scene);
            }
            return result;
        }
    }

    [Serializable]
    public class ScenesPair
    {
        [SerializeField] public SceneField From;
        [SerializeField] public SceneField To;
    }

    [Serializable]
    public class SceneField
    {
        [Scene] [SerializeField] public string Scene;
    }
}