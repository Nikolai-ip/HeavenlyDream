using System.Collections.Generic;

namespace _Game.Source.Domain.MiniGames
{
    public class MiniGamesContainer
    {
        private readonly Dictionary<MiniGameType, MiniGame> _miniGameDictionary;

        public MiniGamesContainer(Dictionary<MiniGameType, MiniGame> miniGameDictionary)
        {
            _miniGameDictionary = miniGameDictionary;
        }

        public bool TryGetGame(MiniGameType type, out MiniGame game)
        {
            return _miniGameDictionary.TryGetValue(type, out game);
        }
    }
}