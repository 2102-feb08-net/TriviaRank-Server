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
        public void getPlayerById(int id);


        public Task<IEnumerable<TriviaPlayer>> getAllPlayers();

        public void getFriendsOfPlayer(int id);

        /// <summary>
        /// Save changes async
        /// </summary>
        public Task saveAsync();
    }
}
