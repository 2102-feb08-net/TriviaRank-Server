using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriviaServer.Models;
using Moq;
using Xunit;
using TriviaServer.DAL;
using TriviaServer.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TriviaServer.Tests.Integration
{

  public class GameRepositoryTests
  {
    [Fact]
    public async Task SearchGame_ByValidGameId_Success()
    {
      //arrange
      using var contextfactory = new TestTriviaGameContextFactory();
      using TriviaRankContext context = contextfactory.CreateContext();


      var insertOwner = new Player
      {
        Username = "gameOwner1",
        Password = "password",
        Birthday = DateTime.Now,
        Points = 100,
        FirstName = "Test",
        LastName = "Player"

      };

      await context.Players.AddAsync(insertOwner);
      context.SaveChanges();

      var repo = new GameRepository(context);
      var insertedGame = await repo.CreateGame(insertOwner.Id, "some game", 10, true);

      //act
      var game = (await repo.SearchGames(1));

      //assert
      Assert.Equal(insertedGame.Id, game.Id);
      Assert.Equal(insertedGame.GameName, game.GameName);
      Assert.Equal(insertedGame.OwnerId, game.OwnerId);
      Assert.Equal(insertedGame.StartDate, game.StartDate);
      Assert.Equal(insertedGame.EndDate, game.EndDate);
      Assert.Equal(insertedGame.GameMode, game.GameMode);
      Assert.Equal(insertedGame.TotalQuestions, game.TotalQuestions);
      Assert.Equal(insertedGame.IsPublic, game.IsPublic);
    }
  }
}