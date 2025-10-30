using System;

namespace _Game.Source.Domain.Plane.MiniGames
{
    public abstract class MiniGame
    {
		public event Action OnExit;
		public event Action OnStart;
        public abstract void Run();
		public abstract void Exit();
    }
}