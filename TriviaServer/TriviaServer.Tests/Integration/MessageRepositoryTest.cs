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
    public class MessageRepositoryTest
    {
        [Fact]
        public async Task CheckValidAddedMessage()
        {
            using var contextFactory = new TestTriviaGameContextFactory();
            using TriviaRankContext context = contextFactory.CreateContext();
            var testBody = "Hello Test2!";

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
                Points = 100,
                FirstName = "Test2",
                LastName = "Player2"
            };
            await context.Players.AddAsync(insertedPlayer1);
            await context.Players.AddAsync(insertedPlayer2);
            context.SaveChanges();

            var repo = new MessageRepository(context);
            await repo.createMessage(insertedPlayer1.Id, insertedPlayer2.Id, testBody);

            var body = (await repo.getLastNMessages(insertedPlayer1.Id, insertedPlayer2.Id, 1)).FirstOrDefault().Body;
            Assert.Equal(testBody, body);
        }

        [Fact]
        public async Task CheckValidLastNMessages()
        {
            using var contextFactory = new TestTriviaGameContextFactory();
            using TriviaRankContext context = contextFactory.CreateContext();
            var messages = new String[]{ "Hello!", "Hello Again!", "Hello Again2!" };

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
                Points = 100,
                FirstName = "Test2",
                LastName = "Player2"
            };
            await context.Players.AddAsync(insertedPlayer1);
            await context.Players.AddAsync(insertedPlayer2);
            context.SaveChanges();

            var repo = new MessageRepository(context);
            await repo.createMessage(insertedPlayer1.Id, insertedPlayer2.Id, messages[0]);
            await repo.createMessage(insertedPlayer1.Id, insertedPlayer2.Id, messages[1]);
            await repo.createMessage(insertedPlayer1.Id, insertedPlayer2.Id, messages[2]);

            var repoMessages = (await repo.getLastNMessages(insertedPlayer1.Id, insertedPlayer2.Id, 3)).ToArray();
            foreach (var index in Enumerable.Range(0,3))
            {
                Assert.Equal(messages[index], repoMessages[index].Body);
            }
        }
    }
}
