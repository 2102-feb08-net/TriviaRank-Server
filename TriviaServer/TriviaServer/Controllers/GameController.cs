using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TriviaServer.Models;
using TriviaServer.Models.Repositories;

namespace TriviaServer.Controllers
{
    [ApiController]
    public class GameContoller : ControllerBase
    {
        private readonly IGameRepository _gameRepo;
        private readonly OpenTriviaDBController _questionController;

        public GameContoller(IGameRepository gameRepo)
        {
            _gameRepo = gameRepo;
            _questionController = new OpenTriviaDBController();
        }

        [HttpGet("/api/games")]
        public async Task<IActionResult> GetAllGames()
        {
            List<GameModel> dbGames;
            try
            {
                dbGames = await _gameRepo.GetAllGames();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            
            return Ok(dbGames);
        }


        [HttpGet("api/game/{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            GameModel game;
            try
            {
                game = await _gameRepo.SearchGames(id);
            }

            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok(game);
        }

        [HttpPost("api/game/new/{newGame}")]
        public async Task<IActionResult> CreateGame(GameDTO newGame)
        {
            try
            {
                
                return Ok(await _questionController.RetrieveQuestions(newGame.TotalQuestions));
                //return Ok(await _gameRepo.CreateGame(ownerId, gameName, totalQuestions, isPublic, duration));
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("api/game/end")]
        public async Task EndGame([FromBody] int Id)
        {
            try
            {
                await _gameRepo.EndGame(Id);
            }
            catch (Exception)
            {
                
            }
        }
    }
}