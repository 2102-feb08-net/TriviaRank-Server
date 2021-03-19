using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaServer.Models.Repositories
{

    public interface IGameRepository
    {
        public Task<GameModel> SearchGames(int appGameID);
        public Task<List<GameModel>> GetAllGames();
        public Task<List<GameModel>> SearchAllGames();
        public Task<GameModel> CreateGame(int ownerId, string gameName, int totalQuestions, bool isPublic, double duration);
        public Task AddPlayerToGame(int gameId, int playerId);
        public Task EndGame(GameModel appGame);
        public Task UpdatePlayerScore(int gameId, int playerId, int score);
    }
}
