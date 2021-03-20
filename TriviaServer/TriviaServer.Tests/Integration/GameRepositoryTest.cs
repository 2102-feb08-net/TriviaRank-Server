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
        public async Task CreateGame_Success()
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
            
            //act
            var repo = new GameRepository(context);
            var insertedGame = await repo.CreateGame(insertOwner.Id, "some game", 10, true, 20.0);


            //assert
            var dbGame = await context.Games.OrderBy(x => x.Id).LastAsync();

            Assert.Equal(insertedGame.Id, dbGame.Id);
            Assert.Equal(insertedGame.GameName, dbGame.GameName);
            Assert.Equal(insertedGame.OwnerId, dbGame.OwnerId);
            Assert.Equal(insertedGame.StartDate, dbGame.StartDate);
            Assert.Equal(insertedGame.EndDate, dbGame.EndDate);
            Assert.Equal(insertedGame.GameMode, dbGame.GameMode);
            Assert.Equal(insertedGame.TotalQuestions, dbGame.TotalQuestions);
            Assert.Equal(insertedGame.IsPublic, dbGame.IsPublic);
        }
    
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
            var insertedGame = await repo.CreateGame(insertOwner.Id, "some game", 10, true, 20.0);

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

        [Fact]
        public async Task EndGame_Success()
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
            var insertedGame = await repo.CreateGame(insertOwner.Id, "some game", 10, true, 20.0);

            //act
            await repo.EndGame(insertedGame);

            //assert
            var dbGame = await context.Games.Where(x => x.Id == insertedGame.Id).FirstAsync();

            Assert.Equal(insertedGame.Id, dbGame.Id);
            Assert.Equal(insertedGame.GameName, dbGame.GameName);
            Assert.Equal(insertedGame.OwnerId, dbGame.OwnerId);
            Assert.Equal(insertedGame.StartDate, dbGame.StartDate);
            Assert.NotEqual(insertedGame.EndDate, dbGame.EndDate);
            Assert.Equal(insertedGame.GameMode, dbGame.GameMode);
            Assert.Equal(insertedGame.TotalQuestions, dbGame.TotalQuestions);
            Assert.Equal(insertedGame.IsPublic, dbGame.IsPublic);
        }

        [Fact]
        public async Task SearchAllGames_GameIsValid_Success()
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

            await context.AddAsync(insertOwner);
            context.SaveChanges();

            var insertedGames = new List<Game>();

            for(var i = 1; i < 5; i++)
            {
                var game = new Game
                {
                    GameName = "CoolGameDude" + i,
                    GameMode = true,
                    OwnerId = 1,
                    StartDate = DateTimeOffset.Now,
                    EndDate = DateTimeOffset.Now.AddMinutes(20.0),
                    TotalQuestions = i + 10,
                    IsPublic = true
                };
                await context.AddAsync(game);
            }

            for (var i = 1; i < 5; i++)
            {
                var game = new Game
                {
                    GameName = "CoolGameDude" + i,
                    GameMode = true,
                    OwnerId = 1,
                    StartDate = DateTimeOffset.Now,
                    EndDate = DateTimeOffset.Now,
                    TotalQuestions = i + 10,
                    IsPublic = true
                };
                await context.AddAsync(game);
            }

            context.SaveChanges();

            var repo = new GameRepository(context);
            var dbGames = new List<GameModel>();

            //act
            dbGames = await repo.SearchAllGames();

            //assert
            Assert.NotEmpty(dbGames);

        }

        [Fact]
        public async Task CreatePlayer_PlayerIsValid_Success()
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
            var insertPlayer = new Player
            {
                Username = "gamePlayer1",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 100,
                FirstName = "TestPlayer",
                LastName = "Playerlastname"

            };

            await context.AddAsync(insertOwner);
            await context.AddAsync(insertPlayer);
            await context.SaveChangesAsync();

            var insertPlayerModel = new PlayerModel
            {
                Id = insertPlayer.Id,
                Username = insertPlayer.Username,
                Password = insertPlayer.Password,
                Birthday = insertPlayer.Birthday,
                Points = insertPlayer.Points,
                FirstName = insertPlayer.FirstName,
                LastName = insertPlayer.LastName
            };

            var game = new Game
            {
                GameName = "testgame1",
                GameMode = true,
                OwnerId = insertOwner.Id,
                StartDate = DateTimeOffset.Now,
                EndDate = DateTimeOffset.Now.AddMinutes(20.0),
                TotalQuestions = 10,
                IsPublic = true
            };
            await context.AddAsync(game);
            await context.SaveChangesAsync();

            var repo = new GameRepository(context);

            //act

            var insertedId = await repo.AddPlayerToGame(game.Id, insertPlayerModel);
            var gp = await context.GamePlayers.Where(gp => gp.PlayerId == insertPlayer.Id).FirstOrDefaultAsync();

            //assert
            Assert.Equal(insertedId, gp.Id);

        }
    }
}