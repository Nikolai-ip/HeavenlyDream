using System.Collections.Generic;
using _Game.Source.Domain.Plane.MiniGames;
using Zenject;

namespace _Game.Source.Infrastructure.Factories
{
    public class MiniGamesFactory: IFactory<Dictionary<MiniGameType, MiniGame>>
    {
        public Dictionary<MiniGameType, MiniGame> Create()
        {
            return new Dictionary<MiniGameType, MiniGame>();
        }
    }
}