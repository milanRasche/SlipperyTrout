using System;
using System.Collections.Generic;
using System.Text;

namespace SlipperyTrout.Chess
{
    public class GameState
    {
        public char[,] Board { get; set; } = new char[8, 8];

        public bool WhiteToMove { get; set; }

        public bool WhiteCanCastleKingside { get; set; }
        public bool WhiteCanCastleQueenSide { get; set; }

        public bool BlackCanCastleKingside { get; set; }
        public bool BlackCanCastleQueenSide { get; set; }

        public string? EnPassantSquare { get; set; }

        public int HalfMoveCLock { get; set; }
        public int FullMoveNumber { get; set; }
        public List<Move> MoveHistory { get; set; } = new();
        public Dictionary<string, int> PositionOccurrences { get; set; } = new();
    }
}
