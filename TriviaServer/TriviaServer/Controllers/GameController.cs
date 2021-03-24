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
            return Ok();

        }

        [HttpPost("api/game")]
        public async Task<IActionResult> CreateGame(GameModel newGame)
        {
            try
            {
                //returns IActionResult for pulling questions from external DB   
                var questions = await _questionController.RetrieveQuestions(newGame.TotalQuestions);

                //creates game in localDB and pulls back GameId
                var game = await _gameRepo.CreateGame(newGame.OwnerId, newGame.GameName, newGame.TotalQuestions, newGame.IsPublic, newGame.Duration);

                //adds questions to the question table
                questions = await _gameRepo.AddQuestions(questions);

                //creates question list for game, stripping correct and incorrect tags on answers.
                List<QuestionsModel> appQuestions = QuestionsModel.CreateAndShuffle(questions);

                //assigns the question to the game.
                game.Questions = appQuestions;

                //inserts gameid, playerid, and questionid to answer table.
                await _gameRepo.LinkGame(game);

                return Ok(game);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            //   return Ok(gamePlayerId);
        }

        [HttpGet("/api/game/{gameId}/{playerId}")]
        public async Task<GameModel> AddPlayerToGame(int gameId, int playerId)
        {
            var appGame = await _gameRepo.AddPlayerToGame(gameId, playerId);
            await _gameRepo.LinkGame(appGame);
            return appGame;
        }

        [HttpPut("api/game/end")]
        public async Task EndGame([FromBody] GameModel appGame)
        {
            try
            {
                await _gameRepo.EndGame(appGame);
            }
            catch (Exception)
            {

            }

        }
    }
}