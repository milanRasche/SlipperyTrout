using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace SlipperyTrout.Engine
{
    public class Evaluation
    {
        //White is Uppercase - Black is Lowercase
        Dictionary<char, int> pieceValues = new Dictionary<char, int>
        {
            { 'P', 100 }, 
            { 'N', 250 }, 
            { 'B', 300 }, 
            { 'R', 500 }, 
            { 'Q', 900 }, 
            { 'K', 0 }, 
            { 'p', -100 }, 
            { 'n', -250 }, 
            { 'b', -300 }, 
            { 'r', -500 }, 
            { 'q', -900 }, 
            { 'k', 0 }   
        };

        public int GetEvaluation(string positionFEN)
        {
            int valuation = 0;
            foreach (char tile in positionFEN)
            {
                if (pieceValues.ContainsKey(tile))
                {
                    valuation += pieceValues[tile];
                }
            }
            return valuation;
        }

        
    }
}
