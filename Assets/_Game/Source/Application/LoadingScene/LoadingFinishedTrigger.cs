using System;
using _Game.Source.Common.Services.SceneTransition;
using UnityEngine;
using Zenject;

namespace _Game.Source.Application.LoadingScene
{
    public class LoadingFinishedTrigger: MonoBehaviour
    {
        private ILoadingFinisher _loadingFinisher;

        [Inject]
        public void Construct(ILoadingFinisher loadingFinisher)
        {
            _loadingFinisher = loadingFinisher;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _loadingFinisher.OnLoadingFinished();
            }
        }
    }
}