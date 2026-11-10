
using System;

namespace SlipperyTrout.Chess
{
    public class ChessGame
    {
        private readonly MoveValidator _validator = new();
        private readonly MovePerformer _performer = new();

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
            if (string.IsNullOrWhiteSpace(uciMove))
                throw new ArgumentException("Move cannot be empty.");

            uciMove = uciMove.Trim().ToLowerInvariant();

            if (uciMove.Length != 4 && uciMove.Length != 5)
                throw new ArgumentException("Invalid UCI move format.");

            char fromFile = uciMove[0];
            char fromRank = uciMove[1];
            char toFile = uciMove[2];
            char toRank = uciMove[3];

            // Validate coordinates
            if (fromFile < 'a' || fromFile > 'h' ||
                toFile < 'a' || toFile > 'h' ||
                fromRank < '1' || fromRank > '8' ||
                toRank < '1' || toRank > '8')
            {
                throw new ArgumentException("Invalid chess coordinates.");
            }

            int fromCol = fromFile - 'a';
            int fromRow = 8 - (fromRank - '0');

            int toCol = toFile - 'a';
            int toRow = 8 - (toRank - '0');

            char promotion = '\0';

            if (uciMove.Length == 5)
            {
                promotion = uciMove[4];

                if (promotion != 'q' && promotion != 'r' &&
                    promotion != 'b' && promotion != 'n')
                {
                    throw new ArgumentException(
                        "Invalid promotion piece. Use q, r, b or n.");
                }
            }

            return new Move(
                uciMove,
                fromRow,
                fromCol,
                toRow,
                toCol,
                promotion);
        }
    }
}
