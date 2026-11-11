using Core.Enums;
using Core.Interfaces;
using Core.Models;
using System.Collections.Concurrent;

namespace Core.Services
{
    public class GameService
    {
        private readonly ConcurrentDictionary<string, GameState> _games = new();
        private readonly IMoveValidationProvider _moveValidationProvider;

        public GameService(IMoveValidationProvider moveValidationProvider)
        {
            _moveValidationProvider = moveValidationProvider;
        }

        public GameState CreateGame(GameOptions options)
        {
            var game = new GameState
            {
                GameId = Guid.NewGuid().ToString(),
                Status = "Active",
                WhiteTimeMs = options.InitialTimeSeconds * 1000,
                BlackTimeMs = options.InitialTimeSeconds * 1000
            };
            InitializeStartingPosition(game);
            _games.TryAdd(game.GameId, game);

            return game;
        }

        public bool TryMakeMove(string gameId, Move move, out GameState? game)
        {
            if (!_games.TryGetValue(gameId, out game))
            {
                return false;
            }

            lock (game)
            {
                if (!_moveValidationProvider.IsValidMove(game, move))
                {
                    return false;
                }

                var piece = game.Board[move.FromCol, move.FromRow]!;
                game.Board[move.FromCol, move.FromRow] = null;
                game.Board[move.ToCol, move.ToRow] = move.PromotionTo.HasValue
                    ? new Piece(piece.Color, move.PromotionTo.Value)
                    : piece;
                game.Moves.Add(move);
                game.SideToMove = piece.Color == PlayerColor.White
                    ? PlayerColor.Black.ToString()
                    : PlayerColor.White.ToString();

                return true;
            }
        }

        private static void InitializeStartingPosition(GameState game)
        {
            PieceType[] backRank =
            [
                PieceType.Rook,
                PieceType.Knight,
                PieceType.Bishop,
                PieceType.Queen,
                PieceType.King,
                PieceType.Bishop,
                PieceType.Knight,
                PieceType.Rook
            ];

            for (var col = 0; col < 8; col++)
            {
                game.Board[col, 0] = new Piece(PlayerColor.White, backRank[col]);
                game.Board[col, 1] = new Piece(PlayerColor.White, PieceType.Pawn);
                game.Board[col, 6] = new Piece(PlayerColor.Black, PieceType.Pawn);
                game.Board[col, 7] = new Piece(PlayerColor.Black, backRank[col]);
            }
        }

        public GameState? GetGame(string gameId)
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
