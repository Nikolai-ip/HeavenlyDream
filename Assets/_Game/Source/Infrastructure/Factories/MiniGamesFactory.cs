using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Source.Domain.MiniGames;
using Zenject;

namespace _Game.Source.Infrastructure.Factories
{
    public class MiniGamesFactory: IFactory<Dictionary<MiniGameType, MiniGame>>
    {
        public Dictionary<MiniGameType, MiniGame> Create()
        {
            var miniGames =  new List<MiniGameType>();
            for (var i = 0; i < Enum.GetValues(typeof(MiniGameType)).Length; i++)
            {
                miniGames.Add((MiniGameType)i);
            }

            return miniGames.ToDictionary(key => key, value => new MiniGame());
        }
    }
}