using System;
using System.Collections.Generic;
using System.Text;

namespace SlipperyTrout.Chess
{
    public class MoveValidator
    {
        public bool ValidateMove (GameState gameState, Move move)
        {
            if (move.FromRow < 0 || move.FromRow >= 8 ||
                move.FromCol < 0 || move.FromCol >= 8 ||
                move.ToRow < 0 || move.ToRow >= 8 ||
                move.ToCol < 0 || move.ToCol >= 8)
                return false;

            char piece = gameState.Board[move.FromRow, move.FromCol];
            bool isWhiteMove = gameState.WhiteToMove;

            //Cant move empty space
            if (piece == '\0')
                return false;

            //Check that the player is trying to move their own piece
            if (char.IsUpper(piece) != isWhiteMove) return false;

            bool isPawn = char.ToUpper(piece) == 'P';
            bool reachesBackRank = move.ToRow == 0 || move.ToRow == 7;

            // UCI requires a promotion piece exactly when a pawn reaches the back rank.
            if (isPawn && reachesBackRank != (move.Promotion != '\0'))
                return false;

            if (move.Promotion != '\0' &&
                (!isPawn || !reachesBackRank ||
                 !IsValidPromotionPiece(move.Promotion)))
                return false;

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
            // A move is not legal if it leaves the moving side's king in check.
            if (!IsMoveSafeAfterMove(gameState, move, isWhiteMove))
                return false;

            return true;
        }

        public bool IsSquareAttacked(GameState gameState, int row, int col, bool byWhite)
        {
            if (!IsInBounds(row, col))
                return false;

            int pawnRow = byWhite ? row + 1 : row - 1;
            char pawn = byWhite ? 'P' : 'p';

            if (IsPieceAt(gameState, pawnRow, col - 1, pawn) ||
                IsPieceAt(gameState, pawnRow, col + 1, pawn))
                return true;

            char knight = byWhite ? 'N' : 'n';
            int[,] knightOffsets =
            {
                { -2, -1 }, { -2, 1 }, { -1, -2 }, { -1, 2 },
                { 1, -2 }, { 1, 2 }, { 2, -1 }, { 2, 1 }
            };

            for (int i = 0; i < knightOffsets.GetLength(0); i++)
            {
                if (IsPieceAt(
                    gameState,
                    row + knightOffsets[i, 0],
                    col + knightOffsets[i, 1],
                    knight))
                    return true;
            }

            char king = byWhite ? 'K' : 'k';
            for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
            {
                for (int colOffset = -1; colOffset <= 1; colOffset++)
                {
                    if ((rowOffset != 0 || colOffset != 0) &&
                        IsPieceAt(gameState, row + rowOffset, col + colOffset, king))
                        return true;
                }
            }

            char rook = byWhite ? 'R' : 'r';
            char bishop = byWhite ? 'B' : 'b';
            char queen = byWhite ? 'Q' : 'q';

            int[,] rookDirections = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
            if (IsAttackedAlongLines(gameState, row, col, rookDirections, rook, queen))
                return true;

            int[,] bishopDirections = { { -1, -1 }, { -1, 1 }, { 1, -1 }, { 1, 1 } };
            return IsAttackedAlongLines(gameState, row, col, bishopDirections, bishop, queen);
        }

        public bool IsInCheck(GameState gameState, bool isWhite)
        {
            char king = isWhite ? 'K' : 'k';

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (gameState.Board[row, col] == king)
                        return IsSquareAttacked(gameState, row, col, !isWhite);
                }
            }

            return false;
        }

        public bool IsCheckmate(GameState gameState)
        {
            bool isWhite = gameState.WhiteToMove;
            return IsInCheck(gameState, isWhite) && !HasAnyLegalMove(gameState);
        }

        public bool IsStalemate(GameState gameState)
        {
            bool isWhite = gameState.WhiteToMove;
            return !IsInCheck(gameState, isWhite) && !HasAnyLegalMove(gameState);
        }

        public bool HasAnyLegalMove(GameState gameState)
        {
            bool isWhite = gameState.WhiteToMove;

            for (int fromRow = 0; fromRow < 8; fromRow++)
            {
                for (int fromCol = 0; fromCol < 8; fromCol++)
                {
                    char piece = gameState.Board[fromRow, fromCol];
                    if (piece == '\0' || char.IsUpper(piece) != isWhite)
                        continue;

                    for (int toRow = 0; toRow < 8; toRow++)
                    {
                        for (int toCol = 0; toCol < 8; toCol++)
                        {
                            bool isPromotion = char.ToUpper(piece) == 'P' &&
                                (toRow == 0 || toRow == 7);

                            if (isPromotion)
                            {
                                char[] promotions = { 'q', 'r', 'b', 'n' };
                                foreach (char promotion in promotions)
                                {
                                    if (ValidateMove(gameState, new Move(
                                        string.Empty,
                                        fromRow,
                                        fromCol,
                                        toRow,
                                        toCol,
                                        promotion)))
                                        return true;
                                }
                            }
                            else if (ValidateMove(gameState, new Move(
                                string.Empty,
                                fromRow,
                                fromCol,
                                toRow,
                                toCol)))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool IsMoveSafeAfterMove(GameState gameState, Move move, bool isWhiteMove)
        {
            char[,] originalBoard = (char[,])gameState.Board.Clone();

            try
            {
                ApplyMoveToBoard(gameState, move, isWhiteMove);
                return !IsInCheck(gameState, isWhiteMove);
            }
            finally
            {
                Array.Copy(originalBoard, gameState.Board, originalBoard.Length);
            }
        }

        private void ApplyMoveToBoard(GameState gameState, Move move, bool isWhiteMove)
        {
            char piece = gameState.Board[move.FromRow, move.FromCol];
            bool isPawn = char.ToUpper(piece) == 'P';
            bool isEnPassant = isPawn &&
                move.FromCol != move.ToCol &&
                gameState.Board[move.ToRow, move.ToCol] == '\0';

            gameState.Board[move.FromRow, move.FromCol] = '\0';

            if (isEnPassant)
                gameState.Board[move.FromRow, move.ToCol] = '\0';

            char movedPiece = piece;
            if (isPawn && (move.ToRow == 0 || move.ToRow == 7) && move.Promotion != '\0')
            {
                movedPiece = isWhiteMove
                    ? char.ToUpper(move.Promotion)
                    : char.ToLower(move.Promotion);
            }

            gameState.Board[move.ToRow, move.ToCol] = movedPiece;

            if (char.ToUpper(piece) == 'K' && Math.Abs(move.ToCol - move.FromCol) == 2)
            {
                int rookFromCol = move.ToCol > move.FromCol ? 7 : 0;
                int rookToCol = move.ToCol > move.FromCol ? 5 : 3;

                gameState.Board[move.ToRow, rookToCol] =
                    gameState.Board[move.ToRow, rookFromCol];
                gameState.Board[move.ToRow, rookFromCol] = '\0';
            }
        }

        private bool IsAttackedAlongLines(
            GameState gameState,
            int row,
            int col,
            int[,] directions,
            char attackingPiece,
            char attackingQueen)
        {
            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int currentRow = row + directions[i, 0];
                int currentCol = col + directions[i, 1];

                while (IsInBounds(currentRow, currentCol))
                {
                    char piece = gameState.Board[currentRow, currentCol];
                    if (piece != '\0')
                    {
                        if (piece == attackingPiece || piece == attackingQueen)
                            return true;

                        break;
                    }

                    currentRow += directions[i, 0];
                    currentCol += directions[i, 1];
                }
            }

            return false;
        }

        private bool IsPieceAt(GameState gameState, int row, int col, char expectedPiece)
        {
            return IsInBounds(row, col) && gameState.Board[row, col] == expectedPiece;
        }

        private bool IsInBounds(int row, int col)
        {
            return row >= 0 && row < 8 && col >= 0 && col < 8;
        }

        private bool IsValidPromotionPiece(char promotion)
        {
            return char.ToLower(promotion) is 'q' or 'r' or 'b' or 'n';
        }

        private bool ValidatePawnMove(GameState gameState, Move move, bool isWhiteMove)
        {
            int direction = isWhiteMove ? -1 : 1;
            int startingRow = isWhiteMove ? 6 : 1;

            int rowDifference = move.ToRow - move.FromRow;
            int colDifference = Math.Abs(move.ToCol - move.FromCol);

            char destinationPiece = gameState.Board[move.ToRow, move.ToCol];

            // Check that the pawn is not moving more than two squares
            if (Math.Abs(rowDifference) > 2)
                return false;

            // Check that the pawn is moving in the correct direction
            if (Math.Sign(rowDifference) != direction)
                return false;

            // Straight movement
            if (move.FromCol == move.ToCol)
            {
                // Pawns cannot move forward into an occupied square
                if (destinationPiece != '\0')
                    return false;

                // Moving two squares
                if (Math.Abs(rowDifference) == 2)
                {
                    // Pawn must be on its starting position
                    if (move.FromRow != startingRow)
                        return false;

                    // Check that the intermediate square is empty
                    int intermediateRow = move.FromRow + direction;

                    if (gameState.Board[intermediateRow, move.FromCol] != '\0')
                        return false;
                }

                return true;
            }

            // Diagonal movement
            if (colDifference == 1 && Math.Abs(rowDifference) == 1)
            {
                // Normal diagonal capture
                if (destinationPiece != '\0')
                {
                    // Cannot capture your own piece
                    return char.IsUpper(destinationPiece) != isWhiteMove;
                }

                // En passant capture
                if (!string.IsNullOrEmpty(gameState.EnPassantSquare))
                {
                    // Convert FEN square (e.g. "e3") to board coordinates
                    int enPassantCol = gameState.EnPassantSquare[0] - 'a';
                    int enPassantRow = 8 - int.Parse(gameState.EnPassantSquare[1].ToString());

                    if (move.ToCol == enPassantCol &&
                        move.ToRow == enPassantRow)
                    {
                        // The captured pawn must exist behind the en passant square
                        char capturedPawn = gameState.Board[move.FromRow, move.ToCol];

                        char expectedPawn = isWhiteMove ? 'p' : 'P';

                        return capturedPawn == expectedPawn;
                    }
                }
            }

            return false;
        }

        private bool ValidateRookMove(GameState gameState, Move move, bool isWhiteMove)
        {
            int rowDifference = move.ToRow - move.FromRow;
            int colDifference = move.ToCol - move.FromCol;

            // Rook must move horizontally or vertically, not diagonally
            if (rowDifference != 0 && colDifference != 0)
                return false;

            // Rook must actually move
            if (rowDifference == 0 && colDifference == 0)
                return false;

            // Check destination square
            char destinationPiece = gameState.Board[move.ToRow, move.ToCol];

            // Cannot capture your own piece
            if (destinationPiece != '\0')
            {
                if (char.IsUpper(destinationPiece) == isWhiteMove)
                    return false;
            }

            // Determine movement direction
            int rowDirection = Math.Sign(rowDifference);
            int colDirection = Math.Sign(colDifference);

            // Check every square between the starting and destination squares
            int currentRow = move.FromRow + rowDirection;
            int currentCol = move.FromCol + colDirection;

            while (currentRow != move.ToRow || currentCol != move.ToCol)
            {
                if (gameState.Board[currentRow, currentCol] != '\0')
                    return false;

                currentRow += rowDirection;
                currentCol += colDirection;
            }

            return true;
        }

        private bool ValidateKnightMove(GameState gameState, Move move, bool isWhiteMove)
        {
            int rowDifference = Math.Abs(move.ToRow - move.FromRow);
            int colDifference = Math.Abs(move.ToCol - move.FromCol);

            // Knight must move in an L-shape
            if (!((rowDifference == 2 && colDifference == 1) ||
                  (rowDifference == 1 && colDifference == 2)))
                return false;

            // Check destination square
            char destinationPiece = gameState.Board[move.ToRow, move.ToCol];

            // Cannot capture your own piece
            if (destinationPiece != '\0')
            {
                if (char.IsUpper(destinationPiece) == isWhiteMove)
                    return false;
            }

            return true;
        }   

        private bool ValidateBishopMove(GameState gameState, Move move, bool isWhiteMove)
        {
            int rowDifference = move.ToRow - move.FromRow;
            int colDifference = move.ToCol - move.FromCol;

            // Bishop must move diagonally
            if (Math.Abs(rowDifference) != Math.Abs(colDifference))
                return false;

            // Bishop must actually move
            if (rowDifference == 0)
                return false;

            // Check destination square
            char destinationPiece = gameState.Board[move.ToRow, move.ToCol];

            // Cannot capture your own piece
            if (destinationPiece != '\0')
            {
                if (char.IsUpper(destinationPiece) == isWhiteMove)
                    return false;
            }

            // Determine movement direction
            int rowDirection = Math.Sign(rowDifference);
            int colDirection = Math.Sign(colDifference);

            // Check every square between the starting and destination squares
            int currentRow = move.FromRow + rowDirection;
            int currentCol = move.FromCol + colDirection;

            while (currentRow != move.ToRow || currentCol != move.ToCol)
            {
                if (gameState.Board[currentRow, currentCol] != '\0')
                    return false;

                currentRow += rowDirection;
                currentCol += colDirection;
            }

            return true;
        } 

        private bool ValidateQueenMove(GameState gameState, Move move, bool isWhiteMove)
        {
            // Queen is a rock with a bishop combined
            return ValidateRookMove(gameState, move, isWhiteMove) || ValidateBishopMove(gameState, move, isWhiteMove);
        }

        private bool ValidateKingMove(GameState gameState, Move move, bool isWhiteMove)
        {
            int rowDifference = Math.Abs(move.ToRow - move.FromRow);
            int colDifference = Math.Abs(move.ToCol - move.FromCol);

            char destinationPiece = gameState.Board[move.ToRow, move.ToCol];

            // Cannot capture your own piece
            if (destinationPiece != '\0')
            {
                if (char.IsUpper(destinationPiece) == isWhiteMove)
                    return false;
            }

            // Normal king movement: one square in any direction
            if (rowDifference <= 1 && colDifference <= 1)
            {
                if (rowDifference == 0 && colDifference == 0)
                    return false;

                return true;
            }

            // Castling: king moves two squares horizontally
            if (rowDifference == 0 && colDifference == 2)
            {
                return ValidateCastling(gameState, move, isWhiteMove);
            }

            return false;
        }

        private bool ValidateCastling(GameState gameState, Move move, bool isWhiteMove)
        {
            int startingRow = isWhiteMove ? 7 : 0;

            // King must start on its original square
            if (move.FromCol != 4 || move.FromRow != startingRow)
                return false;

            // King must move two squares horizontally
            if (move.ToRow != startingRow ||
                Math.Abs(move.ToCol - move.FromCol) != 2)
                return false;

            // The king may not castle while in check or through an attacked square.
            if (IsSquareAttacked(gameState, startingRow, move.FromCol, !isWhiteMove))
                return false;

            int transitCol = move.FromCol + Math.Sign(move.ToCol - move.FromCol);
            Move transitMove = new Move(
                move.UCIFormat,
                move.FromRow,
                move.FromCol,
                startingRow,
                transitCol);

            if (!IsMoveSafeAfterMove(gameState, transitMove, isWhiteMove))
                return false;

            bool isKingSide = move.ToCol > move.FromCol;

            // Check castling rights
            if (isWhiteMove)
            {
                if (isKingSide && !gameState.WhiteCanCastleKingside)
                    return false;

                if (!isKingSide && !gameState.WhiteCanCastleQueenSide)
                    return false;
            }
            else
            {
                if (isKingSide && !gameState.BlackCanCastleKingside)
                    return false;

                if (!isKingSide && !gameState.BlackCanCastleQueenSide)
                    return false;
            }

            // Check that the correct rook exists
            int rookCol = isKingSide ? 7 : 0;
            char expectedRook = isWhiteMove ? 'R' : 'r';

            if (gameState.Board[startingRow, rookCol] != expectedRook)
                return false;

            // Check squares between king and rook are empty
            int direction = isKingSide ? 1 : -1;

            for (int col = move.FromCol + direction;
                 col != rookCol;
                 col += direction)
            {
                if (gameState.Board[startingRow, col] != '\0')
                    return false;
            }

            return true;
        }
    }
}
