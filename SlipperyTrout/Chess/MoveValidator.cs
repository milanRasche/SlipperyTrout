using System;
using System.Collections.Generic;
using System.Text;

namespace SlipperyTrout.Chess
{
    public class MoveValidator
    {
        public bool ValidateMove (GameState gameState, Move move)
        {
            char piece = gameState.Board[move.FromRow, move.FromCol];
            bool isWhiteMove = gameState.WhiteToMove;

            //Check that the player is trying to move their own piece
            if (char.IsUpper(piece) && !isWhiteMove) return false;

            switch (char.ToLower(piece))
            {
                case 'p':
                    if (! ValidatePawnMove(gameState, move, isWhiteMove)) return false;
                    break;
                case 'r':
                    if (!ValidateRookMove(gameState, move, isWhiteMove)) return false;
                    break;
                case 'n':
                    if (!ValidateKnightMove(gameState, move, isWhiteMove)) return false;
                    break;
                case 'b':
                    if (!ValidateBishopMove(gameState, move, isWhiteMove)) return false;
                    break;
                case 'q':
                    if (!ValidateQueenMove(gameState, move, isWhiteMove)) return false;
                    break;
                case 'k':
                    if (!ValidateKingMove(gameState, move, isWhiteMove)) return false;
                    break;
                default:
                    return false;
            }
            //Check for check, checkmate, stalemate, repetition of moves.
        }

        private bool ValidatePawnMove(GameState gameState, Move move, bool isWhiteMove)
        {
            // Implement pawn move validation logic here
            return true;
        }

        private bool ValidateRookMove(GameState gameState, Move move, bool isWhiteMove)
        {
            // Implement rook move validation logic here
            return true;
        }

        private bool ValidateKnightMove(GameState gameState, Move move, bool isWhiteMove)
        {
            // Implement knight move validation logic here
            return true;
        }   

        private bool ValidateBishopMove(GameState gameState, Move move, bool isWhiteMove)
        {
            // Implement bishop move validation logic here
            return true;
        } 

        private bool ValidateQueenMove(GameState gameState, Move move, bool isWhiteMove)
        {
            // Implement queen move validation logic here
            return true;
        }

        private bool ValidateKingMove(GameState gameState, Move move, bool isWhiteMove)
        {
            // Implement king move validation logic here
            return true;
        }
    }
}
