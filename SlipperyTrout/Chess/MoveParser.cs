using System;
using System.Collections.Generic;
using System.Text;

namespace SlipperyTrout.Chess
{
    public class MoveParser
    {
        public Move ParseStandardMove(string move)
        {
            if (string.IsNullOrWhiteSpace(move))
                throw new ArgumentException("Move cannot be empty.");

            move = move.Trim().ToLowerInvariant();

            if (move.Length != 4 && move.Length != 5)
                throw new ArgumentException("Invalid UCI move format.");

            if (move[0] < 'a' || move[0] > 'h' ||
                move[2] < 'a' || move[2] > 'h' ||
                move[1] < '1' || move[1] > '8' ||
                move[3] < '1' || move[3] > '8')
            {
                throw new ArgumentException("Invalid chess coordinates.");
            }

            int startCol = move[0] - 'a';
            int startRow = 7 - (move[1] - '1');

            int endCol = move[2] - 'a';
            int endRow = 7 - (move[3] - '1');

            char promotion = '\0';
            if (move.Length == 5)
            {
                promotion = move[4];
                if (promotion != 'q' && promotion != 'r' &&
                    promotion != 'b' && promotion != 'n')
                {
                    throw new ArgumentException(
                        "Invalid promotion piece. Use q, r, b or n.");
                }
            }

            return new Move(
                move, startRow, startCol, endRow, endCol, promotion);
        }
    }
}
