using Core.Interfaces;
using Core.Models;
using Engine.Validation;

namespace Providers.Local
{
    public class LocalMoveValidationProvider : IMoveValidationProvider
    {
        private readonly MoveValidator _moveValidator;

        public LocalMoveValidationProvider(MoveValidator moveValidator)
        {
            _moveValidator = moveValidator;
        }

        public bool IsValidMove(GameState game, Move move)
        {
            return _moveValidator.IsValidMove(game, move);
        }
    }
}
