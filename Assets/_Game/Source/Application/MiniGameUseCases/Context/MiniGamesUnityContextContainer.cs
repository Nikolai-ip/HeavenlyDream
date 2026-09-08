using System;
using System.Collections.Generic;
using _Game.Source.Domain.MiniGames;
using _Game.Source.Infrastructure;
using _Game.Source.Presenter;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.Context
{
    public class MiniGamesUnityContextContainer: MonoBehaviour
    {
        [SerializeField] private AirplaneModelView _airplanePresenter;
        [SerializeField] private DictionaryInspector<MiniGameType, MiniGameUnityContext> _miniGameContextsInspector;
        private Dictionary<MiniGameType, MiniGameUnityContext> _miniGameContexts;

        private void Awake()
        {
            _miniGameContexts = _miniGameContextsInspector.GetDictionary;
            SetUpContextes();
        }

        private void SetUpContextes()
        {
            foreach (var ctx in _miniGameContexts.Values)
            {
                ctx.AirplanePresenter = _airplanePresenter;
            }
        }

        public T GetContext<T>(MiniGameType type) where T : MiniGameUnityContext
        {
            return (T)_miniGameContexts[type];
        }
    }
    
    public class MiniGameUnityContext: MonoBehaviour
    {
        public virtual Transform Space => transform;
        public AirplaneModelView AirplanePresenter { get; set; }
    }
    
}