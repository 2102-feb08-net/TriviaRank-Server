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


    [HttpGet("api/games/")]
    public async Task<IActionResult> GetAllGames()
    {
      IEnumerable<GameModel> sortedGames;
      try
      {
        var games = await _gameRepo.GetAllGames();
        sortedGames = games.OrderBy(g => g.Id);
      }

      catch (Exception e)
      {
        return StatusCode(500);
      }

      return Ok(sortedGames);
    }



    [HttpGet("api/game/{id}")]
    public async Task<IActionResult> GetGameById(int id)
    {
      GameModel game;
      try
      {
        game = await _gameRepo.SearchGames(id);
      }

      catch (Exception e)
      {
        return StatusCode(500);
      }
      return Ok(game);
    }

  }
}