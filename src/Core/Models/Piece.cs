using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Piece
    {
        public PlayerColor Color { get; }
        public PieceType Type { get; }

        public Piece(PlayerColor colour, PieceType type)
        {
            Color = colour;
            Type = type;
        }

        public Piece Promotion(PieceType newType)
        {
            return new Piece(Color, newType);
        }
        
    }
}
