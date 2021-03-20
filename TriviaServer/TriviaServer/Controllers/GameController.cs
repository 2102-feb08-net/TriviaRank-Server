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



  }
}