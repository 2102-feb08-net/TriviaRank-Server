using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaServer.Models.Repositories;

namespace TriviaServer.DAL.Repositories
{
    class GameRepository : IGameRepository
    {
        private readonly TriviaRankContext _context;
        public GameRepository(TriviaRankContext context)
        {
            _context = context;
        }
        public async Task<Models.GameModel> CreateGame(int ownerId, string gameName, int totalQuestions, bool isPublic)
        {
            Game createGame = await _context.Games
            .OrderBy(x => x.Id).LastAsync();

                createGame.GameName = gameName,
                createGame.OwnerId = ownerId,
                createGame.StartDate = DateTime.Now,
                createGame.TotalQuestions = totalQuestions,
                createGame.IsPublic = isPublic;

            await _context.AddAsync(createGame);
            await _context.SaveChangesAsync();

            Models.GameModel appGame = new Models.GameModel
            {
                Id = createGame.Id,
                GameName = createGame.GameName,
                OwnerId = createGame.OwnerId,
                StartDate = createGame.StartDate,
                TotalQuestions = createGame.TotalQuestions,
                IsPublic = createGame.IsPublic
            };
            return appGame;
        }

        public void EndGame(Models.GameModel appGame)
        {
            Game endGame = _context.Games
                .Where(x => x.Id == appGame.Id).First();

            endGame.EndDate = DateTime.Now;

            _context.Update(endGame);
            _context.SaveChanges();
        }

        public async Task<Models.GameModel> SearchGames(int appGameID)
        {
            try
            {
                Game gameQuery = await _context.Games.Where(x => x.Id == appGameID).FirstAsync();

                if (gameQuery.EndDate < DateTime.Now && gameQuery.IsPublic == true)
                {
                    Models.GameModel appGame = new Models.GameModel
                    {
                        Id = gameQuery.Id,
                        GameName = gameQuery.GameName,
                        OwnerId = gameQuery.OwnerId,
                        StartDate = gameQuery.StartDate,
                        TotalQuestions = gameQuery.TotalQuestions,
                        IsPublic = gameQuery.IsPublic
                    };
                    return appGame;
                }
                else
                {
                    throw new ArgumentException("No game exists.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Something Happened", e);
            }
        }
    }
}
