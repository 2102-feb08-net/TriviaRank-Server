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

        public async Task<PlayerModel> getPlayerByUsername(string username)
        {
            Player p = await _context.Players.FirstOrDefaultAsync(p => p.Username == username);
            return new PlayerModel()
            {
                Id = p.Id,
                Username = p.Username,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Points = p.Points,
                Birthday = p.Birthday
            };
        }

        /// <summary>
        /// Async function to find a player by his id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Player if found else Null</returns>
        public async Task<PlayerModel> getPlayerById(int id)
        {
            Player p = await _context.Players.FirstOrDefaultAsync(p => p.Id == id);
            return new PlayerModel()
            {
                Id = p.Id,
                Username = p.Username,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Points = p.Points,
                Birthday = p.Birthday
            };
        }


        public async Task<IEnumerable<PlayerModel>> getAllPlayers()
        {
            return await _context.Players.Select(p => new PlayerModel() 
            { 
                Id = p.Id, 
                Username = p.Username, 
                FirstName = p.FirstName, 
                LastName = p.LastName,
                Points = p.Points,
                Birthday = p.Birthday
            }).ToListAsync();
        }

        public async Task<IEnumerable<int>> getFriendsOfPlayer(int id)
        {
            var players = new List<int>();
            var currentPlayer = await _context.Players
                .Include(p => p.FriendPlayers)
                .FirstOrDefaultAsync(p => p.Id == id);
            return currentPlayer.FriendPlayers.Select(p => p.FriendId);
        }

        public async Task<int> createPlayer(PlayerModel player)
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

        public async Task createFriend(int playerId, int friendId)
        {
            Friend friend = new Friend()
            {
                PlayerId = playerId,
                FriendId = friendId,
            };
            await _context.AddAsync(friend);
            await saveAsync();
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
