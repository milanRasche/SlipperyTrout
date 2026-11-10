using System.Text;

namespace SlipperyTrout.Chess
{
    public static class GameFormatter
    {
        public static string Format(GameState game)
        {
            StringBuilder sb = new();

            sb.AppendLine("========== CHESS GAME ==========");
            sb.AppendLine();

            // Board
            sb.AppendLine("Board:");
            sb.AppendLine("  a b c d e f g h");

            for (int row = 0; row < 8; row++)
            {
                sb.Append($"{8 - row} ");

                for (int col = 0; col < 8; col++)
                {
                    char piece = game.Board[row, col];

                    sb.Append(piece == '\0' ? '.' : piece);
                    sb.Append(' ');
                }

                sb.AppendLine($"{8 - row}");
            }

            sb.AppendLine("  a b c d e f g h");
            sb.AppendLine();

            // Game information
            sb.AppendLine("Game Information:");
            sb.AppendLine($"Turn:             {(game.WhiteToMove ? "White" : "Black")}");
            sb.AppendLine($"En Passant:       {game.EnPassantSquare ?? "-"}");
            sb.AppendLine($"Halfmove Clock:   {game.HalfMoveCLock}");
            sb.AppendLine($"Fullmove Number:  {game.FullMoveNumber}");

            // Castling rights
            sb.AppendLine();
            sb.AppendLine("Castling Rights:");

            sb.AppendLine($"White Kingside:   {game.WhiteCanCastleKingside}");
            sb.AppendLine($"White Queenside:  {game.WhiteCanCastleQueenSide}");
            sb.AppendLine($"Black Kingside:   {game.BlackCanCastleKingside}");
            sb.AppendLine($"Black Queenside:  {game.BlackCanCastleQueenSide}");

            // Move history
            sb.AppendLine();
            sb.AppendLine("Move History:");

            if (game.MoveHistory.Count == 0)
            {
                sb.AppendLine("No moves played.");
            }
            else
            {
                for (int i = 0; i < game.MoveHistory.Count; i++)
                {
                    sb.AppendLine($"{i + 1}. {game.MoveHistory[i].UCIFormat}");
                }
            }

            sb.AppendLine();
            sb.AppendLine("================================");

            return sb.ToString();
        }
    }
}