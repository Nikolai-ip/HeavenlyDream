using _Game.Source.Common;
using Plugins.DOTweenFramework;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases.Commands
{
    public class PlayAnimationCommand: ICommandMono
    {
        [SerializeField] private bool _forward = true;
        [SerializeField] private TweenAnimation _animation;
        public override void Execute()
        {
            if (_forward)
                _animation.Play();
            else
                _animation.PlayBackwards();
        }
    }
}