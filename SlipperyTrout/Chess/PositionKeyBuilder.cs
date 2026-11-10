using System.Text;

namespace SlipperyTrout.Chess;

internal static class PositionKeyBuilder
{
    public static string Create(GameState gameState)
    {
        StringBuilder key = new();

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                key.Append(gameState.Board[row, col] == '\0'
                    ? '.'
                    : gameState.Board[row, col]);
            }
        }

        key.Append('|');
        key.Append(gameState.WhiteToMove ? 'w' : 'b');
        key.Append('|');

        if (gameState.WhiteCanCastleKingside)
            key.Append('K');

        if (gameState.WhiteCanCastleQueenSide)
            key.Append('Q');

        if (gameState.BlackCanCastleKingside)
            key.Append('k');

        if (gameState.BlackCanCastleQueenSide)
            key.Append('q');

        key.Append('|');
        key.Append(gameState.EnPassantSquare ?? "-");

        return key.ToString();
    }
}
