using System;
using System.Collections.Generic;
using System.Text;

namespace SlipperyTrout.Chess
{
    public class MoveParser
    {
        public Move ParseStandardMove(string move)
        {
            if (move.Length < 4)
                throw new ArgumentException("Invalid move format");

            int startCol = move[0] - 'a';
            int startRow = 7 - (move[1] - '1');

            int endCol = move[2] - 'a';
            int endRow = 7 - (move[3] - '1');

            return new Move(move, startRow, startCol, endRow, endCol);
        }
    }
}
