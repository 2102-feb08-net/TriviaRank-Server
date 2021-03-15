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
        public Task<TriviaPlayer> getPlayerById(int id);


        public Task<IEnumerable<TriviaPlayer>> getAllPlayers();

        public Task<IEnumerable<int>> getFriendsOfPlayer(int id);
        public Task<int> createPlayer(TriviaPlayer player);
        public Task createFriend(int playerId, int friendId);

        /// <summary>
        /// Save changes async
        /// </summary>
        public Task saveAsync();
    }
}
