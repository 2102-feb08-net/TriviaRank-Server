using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriviaServer.Models;
using TriviaServer.Models.Repositories;

namespace TriviaServer.Controllers
{
    [ApiController]
    public class GameContoller : ControllerBase
    {
        private readonly IGameRepository _gameRepo;

        public GameContoller(IGameRepository gameRepo)
        {
            _gameRepo = gameRepo;
        }

        [HttpGet("/api/games")]
        public async Task<IActionResult> GetAllGames()
        {
            List<GameModel> dbGames = await _gameRepo.GetAllGames();

            return Ok(dbGames);
        }


        [HttpGet("api/game/{gameId}")]
        public async Task<IActionResult> getAnyGame(int gameId)
        {
            GameModel game;
            try
            {
                game = await _gameRepo.getAnyGame(gameId);
            }

            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok(game);
        }

        [HttpPost("api/game/{gameId}/player")]
        public async Task<IActionResult> AddPlayerToGame(int gameId, PlayerModel player)
        {
            int gamePlayerId = -1;
            try
            {
                gamePlayerId = await _gameRepo.AddPlayerToGame(gameId, player);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok(gamePlayerId);
        }

        [HttpPost("api/game")]
        public async Task<IActionResult> newGame(GameModel game)
        {
            GameModel newGame;
            try
            {
                newGame = await _gameRepo.CreateGame(game);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok(newGame);
        }

    }
}