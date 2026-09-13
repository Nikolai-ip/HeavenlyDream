using UnityEngine;

namespace _Game.Source.Presenter.MiniGames
{
    public abstract class MiniGameEnvironmentView: MonoBehaviour
    {
        public abstract void Show();
        public abstract void Hide();
    }
}