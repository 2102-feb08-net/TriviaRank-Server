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

        public async Task<List<GameModel>> SearchAllGames()
        {
            var dbGames = await _context.Games
                .Where(x => x.EndDate == DateTime.MinValue && x.IsPublic == true)
                .ToListAsync();

            List<GameModel> gameList = new List<GameModel>();

            foreach(var game in dbGames)
            {
                var eachGame = new GameModel
                {
                    Id = game.Id,
                    GameName = game.GameName,
                    OwnerId = game.OwnerId,
                    StartDate = game.StartDate,
                    EndDate = game.EndDate,
                    GameMode = game.GameMode,
                    IsPublic = game.IsPublic,
                    TotalQuestions = game.TotalQuestions
                };
                gameList.Add(eachGame);
            }
            return gameList;
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

        public async Task EndGame(Models.GameModel appGame)
        {
            Game endGame = _context.Games
                .Where(x => x.Id == appGame.Id).First();

            endGame.EndDate = DateTime.Now;

            _context.Update(endGame);
            await _context.SaveChangesAsync();
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
