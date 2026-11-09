using System;
using System.Collections.Generic;
using System.Text;

namespace SlipperyTrout.Chess
{
    public class Move
    {
        public string UCIFormat { get; }
        public int FromRow { get; }
        public int FromCol { get; }
        public int ToRow { get; }
        public int ToCol { get; }

        public char Promotion { get; }

        public Move(
            string UCIFormat,
            int fromRow,
            int fromCol,
            int toRow,
            int toCol,
            char promotion = '\0')
        {
            UCIFormat = UCIFormat;
            FromRow = fromRow;
            FromCol = fromCol;
            ToRow = toRow;
            ToCol = toCol;
            Promotion = promotion;
        }
    }
}
