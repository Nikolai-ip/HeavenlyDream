using _Game.Source.Presenter.MiniGames;
using UnityEngine;

namespace _Game.Source.Application.MiniGameUseCases
{
    public abstract class MiniGameController : MonoBehaviour
    {
        [SerializeField] private MiniGameEnvironmentView miniGameEnvironmentView; 
        public virtual void OnMiniGameStart()
        {
            miniGameEnvironmentView.Show();
        }

        public virtual void OnMiniGameEnd()
        {
            miniGameEnvironmentView.Hide();
        }
    }
}