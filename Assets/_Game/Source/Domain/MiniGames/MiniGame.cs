using System;

namespace _Game.Source.Domain.MiniGames
{
    public class MiniGame
    {
		public event Action OnExit;
		public event Action OnStart;

		public void Run()
		{
			OnStart?.Invoke();
		}

		public void Exit()
		{
			OnExit?.Invoke();
		}
    }
}