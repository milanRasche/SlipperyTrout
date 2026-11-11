using API.Models;
using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GamesController : Controller
    {
        private readonly GameService _gameService;

        public GamesController(GameService gamesService)
        {
            _gameService = gamesService;
        }

        // GET: api/v1/games
        [HttpGet]
        public ActionResult<IEnumerable<GameStateDto>> GetGames()
        {
            return Ok(_gameService.GetAllGames().Select(GameStateDto.From));
        }

        // GET: api/v1/games/{gameId}
        [HttpGet("{gameId}")]
        public ActionResult<GameStateDto> GetGame(string gameId)
        {
            var game = _gameService.GetGame(gameId);

            if (game == null) return NotFound();

            return Ok(GameStateDto.From(game));
        }

        // POST: api/v1/games
        [HttpPost]
        public ActionResult<GameStateDto> CreateGame(
            [FromBody] GameOptions options)
        {
            var game = _gameService.CreateGame(options);

            return CreatedAtAction(
                nameof(GetGame),
                new { gameId = game.GameId },
                GameStateDto.From(game)
            );
        }

        // POST: api/v1/games/move
        [HttpPost("/move")]
        public ActionResult<GameStateDto> MakeMove([FromBody] MoveRequest request)
        {
            Move move;
            // Validate move is in correct UCI format
            try
            {
                move = new Move(request.Uci);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new { error = exception.Message });
            }

            GameState? game;

            if (_gameService.TryMakeMove(request.GameId, move, out game))
            {
                return Ok(GameStateDto.From(game!));
            }

            return game is null
                ? NotFound()
                : Conflict(new { error = "The move is not valid for the current game state." });
        }
    }
}
