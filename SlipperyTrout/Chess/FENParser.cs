using System;
using System.Collections.Generic;
using System.Text;

namespace SlipperyTrout.Chess
{
    public class FENParser
    {
        public static GameState ParseFEN(string fen)
        {
            var gameState = new GameState();
            var parts = fen.Split(' ');
            
            // Parse board
            var rows = parts[0].Split('/');
            for (int row = 0; row < 8; row++)
            {
                int col = 0;
                foreach (char c in rows[row])
                {
                    if (char.IsDigit(c))
                    {
                        col += c - '0';
                    }
                    else
                    {
                        gameState.Board[row, col] = c;
                        col++;
                    }
                }
            }

            // White to move?
            gameState.WhiteToMove = parts[1] == "w";
            
            // Castling availability
            gameState.WhiteCanCastleKingside = parts[2].Contains("K");
            gameState.WhiteCanCastleQueenSide = parts[2].Contains("Q");
            gameState.BlackCanCastleKingside = parts[2].Contains("k");
            gameState.BlackCanCastleQueenSide = parts[2].Contains("q");
            
            // En passant target square
            gameState.EnPassantSquare = parts[3] != "-" ? parts[3] : null;
            
            // Halfmove clock
            gameState.HalfMoveCLock = int.Parse(parts[4]);
            // Fullmove number
            gameState.FullMoveNumber = int.Parse(parts[5]);

            string positionKey = PositionKeyBuilder.Create(gameState);
            gameState.PositionOccurrences[positionKey] = 1;

            return gameState;
        }
    }
}
