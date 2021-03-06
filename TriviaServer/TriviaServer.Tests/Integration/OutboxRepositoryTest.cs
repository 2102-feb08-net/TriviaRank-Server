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
    public class OutboxRepositoryTest
    {
        [Fact]
        public async Task CheckCreateValidFriendInvite()
        {
            using var contextFactory = new TestTriviaGameContextFactory();
            using TriviaRankContext context = contextFactory.CreateContext();

            var insertedPlayer1 = new Player
            {
                Username = "testusername010101@revature.com",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 100,
                FirstName = "Bart",
                LastName = "Simpson"
            };
            var insertedPlayer2 = new Player
            {
                Username = "testusername020202@revature.com",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 200,
                FirstName = "Milhouse",
                LastName = "Vanhouten"
            };
            await context.Players.AddAsync(insertedPlayer1);
            await context.Players.AddAsync(insertedPlayer2);
            context.SaveChanges();

            var repo = new OutboxRepository(context);
            await repo.createFriendInvite(insertedPlayer1.Id, insertedPlayer2.Id);

            var inviterPlayer = (await repo.getFriendInvites(insertedPlayer2.Id)).FirstOrDefault();
            Assert.Equal(insertedPlayer1.Id, inviterPlayer.Id);
        }

        [Fact]
        public async Task CheckCreateValidGameInvite()
        {
            using var contextFactory = new TestTriviaGameContextFactory();
            using TriviaRankContext context = contextFactory.CreateContext();

            var insertedPlayer1 = new Player
            {
                Username = "testusername010101@revature.com",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 100,
                FirstName = "Bart",
                LastName = "Simpson"
            };
            var insertedPlayer2 = new Player
            {
                Username = "testusername020202@revature.com",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 200,
                FirstName = "Milhouse",
                LastName = "Vanhouten"
            };
            await context.Players.AddAsync(insertedPlayer1);
            await context.Players.AddAsync(insertedPlayer2);
            context.SaveChanges();
            var game1 = new Game
            {
                GameName = "new game 1",
                OwnerId = insertedPlayer1.Id,
                StartDate = DateTimeOffset.Now,
                EndDate = DateTimeOffset.Now,
                GameMode = true,
                TotalQuestions = 10,
                IsPublic = true
            };
            await context.Games.AddAsync(game1);
            context.SaveChanges();

            var repo = new OutboxRepository(context);
            await repo.createGameInvite(game1.Id, insertedPlayer2.Id);

            var invitedGame = (await repo.getGameInvites(insertedPlayer2.Id)).FirstOrDefault();
            Assert.Equal(game1.Id, invitedGame.Id);
        }

        [Fact]
        public async Task CheckCreateValidFriendInviteUsername()
        {
            using var contextFactory = new TestTriviaGameContextFactory();
            using TriviaRankContext context = contextFactory.CreateContext();

            var insertedPlayer1 = new Player
            {
                Username = "testusername010101@revature.com",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 100,
                FirstName = "Bart",
                LastName = "Simpson"
            };
            var insertedPlayer2 = new Player
            {
                Username = "testusername020202@revature.com",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 200,
                FirstName = "Milhouse",
                LastName = "Vanhouten"
            };
            await context.Players.AddAsync(insertedPlayer1);
            await context.Players.AddAsync(insertedPlayer2);
            context.SaveChanges();

            var repo = new OutboxRepository(context);
            await repo.createFriendInvite(insertedPlayer1.Username, insertedPlayer2.Username);

            var inviterPlayer = (await repo.getFriendInvites(insertedPlayer2.Id)).FirstOrDefault();
            Assert.Equal(insertedPlayer1.Id, inviterPlayer.Id);
        }

        [Fact]
        public async Task DeleteFriendInvite()
        {
            using var contextFactory = new TestTriviaGameContextFactory();
            using TriviaRankContext context = contextFactory.CreateContext();

            var insertedPlayer1 = new Player
            {
                Username = "testusername010101@revature.com",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 100,
                FirstName = "Bart",
                LastName = "Simpson"
            };
            var insertedPlayer2 = new Player
            {
                Username = "testusername020202@revature.com",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 200,
                FirstName = "Milhouse",
                LastName = "Vanhouten"
            };
            await context.Players.AddAsync(insertedPlayer1);
            await context.Players.AddAsync(insertedPlayer2);
            context.SaveChanges();
            var repo = new OutboxRepository(context);
            await repo.createFriendInvite(insertedPlayer1.Username, insertedPlayer2.Username);

            await repo.deleteFriendInvite(insertedPlayer1.Username, insertedPlayer2.Username);

            Assert.Empty((await repo.getFriendInvites(insertedPlayer2.Id)).Where(p => p.Id == insertedPlayer1.Id));
        }
    }
}