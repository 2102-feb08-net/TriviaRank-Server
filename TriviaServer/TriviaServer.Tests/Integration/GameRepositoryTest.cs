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

            var insertedGame = new Game
            {
                Id = 3,
                GameName = "Good Times",
                OwnerId = 2,
                StartDate = DateTime.Now,
                GameMode = true,
                TotalQuestions = 10,
                IsPublic = true
            };

            await context.Games.AddAsync(insertedGame);
            context.SaveChanges();
            var repo = new GameRepository(context);

            //act
            GameModel game = await repo.SearchGames(insertedGame.Id);

            //assert
            Assert.Equal(insertedGame.Id, game.Id);
            Assert.Equal(insertedGame.GameName, game.GameName);
            Assert.Equal(insertedGame.OwnerId, game.OwnerId);
            Assert.Equal(insertedGame.StartDate, game.StartDate);
            Assert.Equal(insertedGame.GameMode, game.GameMode);
            Assert.Equal(insertedGame.TotalQuestions, game.TotalQuestions);
            Assert.Equal(insertedGame.IsPublic, game.IsPublic);
        }
    }
}