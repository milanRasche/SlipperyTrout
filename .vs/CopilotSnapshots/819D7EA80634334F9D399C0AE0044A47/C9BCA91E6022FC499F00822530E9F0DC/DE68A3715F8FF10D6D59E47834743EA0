using Core.Models;
using System.Collections.Concurrent;

namespace Core.Services
{
    public class GameService
    {
        private readonly ConcurrentDictionary<string, GameState> _games = new();

        public GameState CreateGame(GameOptions options)
        {
            var game = new GameState
            {
                GameId = Guid.NewGuid().ToString(),
                Status = "Active",
                WhiteTimeMs = options.InitialTimeSeconds * 1000,
                BlackTimeMs = options.InitialTimeSeconds * 1000
            };
            _games.TryAdd(game.GameId, game);

            return game;
        }

        public GameState? GettGame(string gameId)
        {
            _games.TryGetValue(gameId, out var game);
            return game;
        }

        public IEnumerable<GameState> GetAllGames()
        {
            return _games.Values;
        }
    }
}
