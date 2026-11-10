
using System;

namespace SlipperyTrout.Chess
{
    public class ChessGame
    {
        private readonly MoveValidator _validator = new();
        private readonly MovePerformer _performer = new();
        private readonly MoveParser _moveParser = new();

        public static GameState NewGame()
        {
            return FENParser.ParseFEN(
                "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }

        public static GameState FromFEN(string fen)
        {
            return FENParser.ParseFEN(fen);
        }

        public GameState MakeMove(GameState gameState, string uciMove)
        {
            Move move = ParseMove(uciMove);

            // Validate the move
            if (!_validator.ValidateMove(gameState, move))
            {
                throw new ArgumentException(
                    $"Illegal move: {uciMove}");
            }

            // Perform the move
            _performer.PerformMove(gameState, move);

            return gameState;
        }

        private Move ParseMove(string uciMove)
        {
            return _moveParser.ParseStandardMove(uciMove);
        }
    }
}
