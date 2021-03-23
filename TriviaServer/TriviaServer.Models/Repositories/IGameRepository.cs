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

    //     public Task EndGame(GameModel appGame);
    public Task<int> AddPlayerToGame(int gameId, PlayerModel player);
    public Task<GameModel> getAnyGame(int gameId);
    public Task<GameModel> CreateGame(int ownerId, string gameName, int totalQuestions, bool isPublic, double duration);
    public Task<GameModel> AddPlayerToGame(int gameId, int playerId);
    public Task EndGame(int Id);
    public Task UpdatePlayerScore(int gameId, int playerId, int score);
    public Task<QuestionsDTO> AddQuestions(QuestionsDTO questions);
    public Task LinkGame(GameModel game);
  }
}
