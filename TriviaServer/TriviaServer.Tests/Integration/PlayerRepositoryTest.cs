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
                Username = "testusername010101",
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
                Username = "testusername010101",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 100,
                FirstName = "Test1",
                LastName = "Player1"
            };
            var insertedPlayer2 = new Player
            {
                Username = "testusername020202",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 200,
                FirstName = "Test2",
                LastName = "Player2"
            };
            var insertedPlayer3 = new Player
            {
                Username = "testusername030303",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 300,
                FirstName = "Test3",
                LastName = "Player3"
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
                Username = "testusername010101",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 100,
                FirstName = "Test1",
                LastName = "Player1"
            };
            var insertedFriend1 = new Player
            {
                Username = "testusername020202",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 200,
                FirstName = "Test2",
                LastName = "Player2"
            };
            var insertedFriend2 = new Player
            {
                Username = "testusername030303",
                Password = "password",
                Birthday = DateTime.Now,
                Points = 300,
                FirstName = "Test3",
                LastName = "Player3"
            };
            var numPlayersBefore = context.Players.Count();
            await context.Players.AddAsync(insertedPlayer1);
            await context.Players.AddAsync(insertedFriend1);
            await context.Players.AddAsync(insertedFriend2);
            context.SaveChanges();


            var repo = new PlayerRepository(context);
            await repo.createFriend(insertedPlayer1.Id, insertedFriend1.Id);
            await repo.createFriend(insertedPlayer1.Id, insertedFriend2.Id);


            Assert.Equal(2, (await repo.getFriendsOfPlayer(insertedPlayer1.Id)).Count());
        }

    }
}
