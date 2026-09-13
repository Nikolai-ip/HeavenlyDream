using Plugins.DOTweenFramework;
using UnityEngine;

namespace _Game.Source.Presenter.MiniGames
{
    public class DefaultMiniGameEnvironmentView: MiniGameEnvironmentView
    {
        [SerializeField] private TweenAnimation _showPlaneAnimation;
        [SerializeField] private TweenAnimation _hidePlaneAnimation;
        [SerializeField] private SRColorChanger _shadowPanel;
        [SerializeField] private Canvas _canvas;
        public override void Show()
        {
            _showPlaneAnimation.Play();
            _shadowPanel.Play();
            _canvas.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            _hidePlaneAnimation.Play();
            _shadowPanel.PlayBackwards();
            _canvas.gameObject.SetActive(false);
        }
    }
}