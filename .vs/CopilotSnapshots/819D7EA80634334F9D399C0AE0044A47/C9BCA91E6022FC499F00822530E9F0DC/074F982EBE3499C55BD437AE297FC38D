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
        public ActionResult<IEnumerable<GameState>> GetGames()
        {
            return Ok(_gameService.GetAllGames());
        }

        // GET: api/v1/games/{gameId}
        [HttpGet("{gameId}")]
        public ActionResult<GameState> GetGame(string gameId)
        {
            var game = _gameService.GettGame(gameId);

            if (game == null) return NotFound();

            return Ok(game);
        }

        // POST: api/v1/games
        [HttpPost]
        public ActionResult<GameState> CreateGame(
            [FromBody] GameOptions options)
        {
            var game = _gameService.CreateGame(options);

            return CreatedAtAction(
                nameof(GetGame),
                new { gameId = game.GameId },
                game
            );
        }
    }
}
