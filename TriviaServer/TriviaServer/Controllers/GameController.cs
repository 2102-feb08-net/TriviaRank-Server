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
        public async Task<IActionResult> CreateGame(GameModel newGame)
        {
            try
            {
                //returns IActionResult for pulling questions from external DB   
                var questions = await _questionController.RetrieveQuestions(newGame.TotalQuestions);
                
                //creates game in localDB and pulls back GameId
                var game = await _gameRepo.CreateGame(newGame.OwnerId, newGame.GameName, newGame.TotalQuestions, newGame.IsPublic, newGame.Duration);

                //adds questions to the question table
                await _gameRepo.AddQuestions(questions);

                //creates question list for game, stripping correct and incorrect tags on answers.
                List<QuestionsModel> appQuestions = QuestionsModel.CreateAndShuffle(questions);
                
                //assigns the question to the game.
                game.Questions = appQuestions;
                return Ok(game);
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