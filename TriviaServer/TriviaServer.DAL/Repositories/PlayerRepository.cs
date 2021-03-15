using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaServer.Models;
using TriviaServer.Models.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TriviaServer.DAL.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly TriviaRankContext _context;
        public PlayerRepository(TriviaRankContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Async function to find a player by his id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Player if found else Null</returns>
        public void getPlayerById(int id)
        {
        }


        public async Task<IEnumerable<TriviaPlayer>> getAllPlayers()
        {
            return await _context.Players.Select(p => new TriviaPlayer() 
            { 
                Id = p.Id, 
                Username = p.Username, 
                FirstName = p.FirstName, 
                LastName = p.LastName,
                Points = p.Points,
                Birthday = p.Birthday
            }).ToListAsync();
        }

        public void getFriendsOfPlayer(int id)
        {
        }

        public async Task<int> createPlayer(TriviaPlayer player)
        {
            Player newPlayer = new Player()
            {
                FirstName = player.FirstName,
                LastName = player.LastName,
                Username = player.Username,
                Points = player.Points,
                Birthday = player.Birthday
            };
            await _context.AddAsync(newPlayer);
            await saveAsync();
            return newPlayer.Id;
        }

        /// <summary>
        /// Save changes async
        /// </summary>
        public async Task saveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
