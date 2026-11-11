using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IMoveValidationProvider
    {
        bool IsValidMove(GameState game, Move move);
    }
}
