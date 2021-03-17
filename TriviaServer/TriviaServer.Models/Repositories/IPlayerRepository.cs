using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaServer.Models.Repositories
{
  public interface IPlayerRepository
  {
    /// <summary>
    /// Async function to find a player by his id. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Player if found else Null</returns>
    public Task<PlayerModel> getPlayerById(int id);


    public Task<IEnumerable<PlayerModel>> getAllPlayers();
    /// <summary>
    /// Finds all friends of a player
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<IEnumerable<int>> getFriendsOfPlayer(int id);
    public Task<int> createPlayer(PlayerModel player);
    /// <summary>
    /// Create a Friendship between two players
    /// </summary>
    /// <param name="playerId"></param>
    /// <param name="friendId"></param>
    /// <returns></returns>
    public Task createFriend(int playerId, int friendId);

    /// <summary>
    /// Save changes async
    /// </summary>
    public Task saveAsync();
  }
}
