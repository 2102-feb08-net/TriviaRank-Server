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
        public Task<GameModel> CreateGame(int ownerId, string gameName, int totalQuestions, bool isPublic);
        public Task AddPlayerToGame(int gameId, int playerId);
        public void EndGame(GameModel appGame);

    }
}
