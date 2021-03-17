using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaServer.Models;
using TriviaServer.Models.Repositories;

namespace TriviaServer.DAL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly TriviaRankContext _context;
        public GameRepository(TriviaRankContext context)
        {
            _context = context;
        }

        public async Task AddPlayerToGame(int gameId, int playerId)
        {
            GamePlayer gamePlayer = new GamePlayer
            {
                GameId = gameId,
                PlayerId = playerId,
                TotalCorrect = 0
            };

            await _context.GamePlayers.AddAsync(gamePlayer);
        }

        public async Task UpdatePlayerScore(int gameId, int playerId, int score)
        {
            var player = await _context.GamePlayers.FirstOrDefaultAsync(p => p.PlayerId == playerId);
            player.TotalCorrect = score;
            _context.Update(player);

            await _context.SaveChangesAsync();
        }

        public async Task<Models.GameModel> CreateGame(int ownerId, string gameName, int totalQuestions, bool isPublic)
        {
            Game newGame = new Game
            {
                GameName = gameName,
                OwnerId = ownerId,
                StartDate = DateTime.Now,
                TotalQuestions = totalQuestions,
                IsPublic = isPublic
            };

            await _context.AddAsync(newGame);
            await _context.SaveChangesAsync();

            Models.GameModel appGame = new Models.GameModel
            {
                Id = newGame.Id,
                GameName = newGame.GameName,
                OwnerId = newGame.OwnerId,
                StartDate = newGame.StartDate,
                TotalQuestions = newGame.TotalQuestions,
                IsPublic = newGame.IsPublic
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
