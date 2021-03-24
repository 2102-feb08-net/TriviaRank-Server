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
        public Task<PlayerModel> getPlayerByUsername(string username);
        public Task<IEnumerable<PlayerModel>> getAllPlayers();
        public Task<IEnumerable<int>> getFriendsOfPlayer(int id);
        public Task<PlayerModel> createPlayer(PlayerModel player);
        public Task createFriend(int playerId, int friendId);
        public Task deleteFriend(int playerId, int friendId);
        public Task<IEnumerable<GameModel>> getPlayerGames(int playerId, bool? isActive);
        public Task<IEnumerable<PlayerModel>> getNPlayers(int numPlayers, int page);

        /// <summary>
        /// Save changes async
        /// </summary>
        public Task saveAsync();
    }
}
