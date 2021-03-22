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
  public class PlayerRepositoryTest
  {
    [Fact]
    public async Task CheckValidExisitingPlayer()
    {
      using var contextFactory = new TestTriviaGameContextFactory();
      using TriviaRankContext context = contextFactory.CreateContext();

      var insertedPlayer = new Player
      {
        Username = "testusername010101@revature.com",
        Password = "password",
        Birthday = DateTime.Now,
        Points = 100,
        FirstName = "Test",
        LastName = "Player"
      };
      await context.Players.AddAsync(insertedPlayer);
      context.SaveChanges();
      var repo = new PlayerRepository(context);


      PlayerModel player = await repo.getPlayerById(insertedPlayer.Id);


      Assert.Equal(insertedPlayer.Id, player.Id);
      Assert.Equal(insertedPlayer.Username, player.Username);
      Assert.Equal(insertedPlayer.Birthday, player.Birthday);
      Assert.Equal(insertedPlayer.Points, player.Points);
      Assert.Equal(insertedPlayer.FirstName, player.FirstName);
      Assert.Equal(insertedPlayer.LastName, player.LastName);
    }

    [Fact]
    public async Task CheckValidAddedPlayers()
    {
      using var contextFactory = new TestTriviaGameContextFactory();
      using TriviaRankContext context = contextFactory.CreateContext();

      var insertedPlayer1 = new Player
      {
        Username = "testusername010101@revature.com",
        Password = "password",
        Birthday = DateTime.Now,
        Points = 100,
        FirstName = "Bob",
        LastName = "Smith"
      };
      var insertedPlayer2 = new Player
      {
        Username = "testusername020202@revature.com",
        Password = "password",
        Birthday = DateTime.Now,
        Points = 200,
        FirstName = "Jim",
        LastName = "Jones"
      };
      var insertedPlayer3 = new Player
      {
        Username = "testusername030303@revature.com",
        Password = "password",
        Birthday = DateTime.Now,
        Points = 300,
        FirstName = "Bill",
        LastName = "Johnson"
      };
      var numPlayersBefore = context.Players.Count();
      await context.Players.AddAsync(insertedPlayer1);
      await context.Players.AddAsync(insertedPlayer2);
      await context.Players.AddAsync(insertedPlayer3);
      context.SaveChanges();

      var repo = new PlayerRepository(context);

      Assert.Equal(numPlayersBefore, (await repo.getAllPlayers()).Count() - 3);
    }

    [Fact]
    public async Task CheckValidFriendsAdded()
    {
      using var contextFactory = new TestTriviaGameContextFactory();
      using TriviaRankContext context = contextFactory.CreateContext();

      var insertedPlayer1 = new Player
      {
        Username = "testusername010101@revature.com",
        Password = "password",
        Birthday = DateTime.Now,
        Points = 100,
        FirstName = "Bob",
        LastName = "Smith"
      };
      var insertedFriend2 = new Player
      {
        Username = "testusername020202@revature.com",
        Password = "password",
        Birthday = DateTime.Now,
        Points = 200,
        FirstName = "Jim",
        LastName = "Jones"
      };
      var insertedFriend3 = new Player
      {
        Username = "testusername030303@revature.com",
        Password = "password",
        Birthday = DateTime.Now,
        Points = 300,
        FirstName = "Bill",
        LastName = "Johnson"
      };
      var numPlayersBefore = context.Players.Count();
      await context.Players.AddAsync(insertedPlayer1);
      await context.Players.AddAsync(insertedFriend2);
      await context.Players.AddAsync(insertedFriend3);
      context.SaveChanges();


      var repo = new PlayerRepository(context);
      await repo.createFriend(insertedPlayer1.Id, insertedFriend2.Id);
      await repo.createFriend(insertedFriend2.Id, insertedFriend3.Id);


      Assert.Equal(2, (await repo.getFriendsOfPlayer(insertedFriend2.Id)).Count());
    }

    [Fact]
    public async Task CheckValidPlayerGames()
    {
      using var contextFactory = new TestTriviaGameContextFactory();
      using TriviaRankContext context = contextFactory.CreateContext();

      var insertedPlayer1 = new Player
      {
        Username = "testusername010101@revature.com",
        Password = "password",
        Birthday = DateTime.Now,
        Points = 100,
        FirstName = "Bob",
        LastName = "Smith"
      };
      var insertedPlayer2 = new Player
      {
        Username = "testusername020202@revature.com",
        Password = "password",
        Birthday = DateTime.Now,
        Points = 100,
        FirstName = "Bob",
        LastName = "Smith"
      };
      await context.Players.AddAsync(insertedPlayer1);
      await context.Players.AddAsync(insertedPlayer2);
      context.SaveChanges();
      var insertedGame1 = new Game
      {
        GameName = "randomGame",
        OwnerId = insertedPlayer1.Id,
        StartDate = DateTimeOffset.Now,
        EndDate = DateTimeOffset.Now,
        GameMode = true,
        TotalQuestions = 10,
        IsPublic = true
      };
      await context.Games.AddAsync(insertedGame1);
      context.SaveChanges();
      var insertedGamePlayer1 = new GamePlayer
      {
        GameId = insertedGame1.Id,
        PlayerId = insertedPlayer2.Id,
        TotalCorrect = 8
      };
      await context.GamePlayers.AddAsync(insertedGamePlayer1);
      context.SaveChanges();

      var repo = new PlayerRepository(context);
      GameModel createdGame = (await repo.getPlayerGames(insertedPlayer2.Id, null)).FirstOrDefault();


      Assert.Equal(insertedGame1.Id, createdGame.Id);
      Assert.Equal(insertedGame1.GameName, createdGame.GameName);
      Assert.Equal(insertedGame1.OwnerId, createdGame.OwnerId);
      Assert.Equal(insertedGame1.StartDate, createdGame.StartDate);
      Assert.Equal(insertedGame1.EndDate, createdGame.EndDate);
      Assert.Equal(insertedGame1.GameMode, createdGame.GameMode);
      Assert.Equal(insertedGame1.TotalQuestions, createdGame.TotalQuestions);
      Assert.Equal(insertedGame1.IsPublic, createdGame.IsPublic);
    }

  }
}
