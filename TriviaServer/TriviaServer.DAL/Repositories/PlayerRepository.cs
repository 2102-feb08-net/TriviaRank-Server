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

        public async Task<IEnumerable<GameModel>> getPlayerGames(int playerId, bool? isActive)
        {
            IEnumerable<GameModel> games = (await _context.Players.Include(p => p.GamePlayers).ThenInclude(gp => gp.Game).FirstOrDefaultAsync(p => p.Id == playerId))
                .GamePlayers
                .Select(g => new GameModel
                {
                    Id = g.Game.Id,
                    GameName = g.Game.GameName,
                    OwnerId = g.Game.OwnerId,
                    StartDate = g.Game.StartDate,
                    EndDate = g.Game.EndDate,
                    GameMode = g.Game.GameMode,
                    TotalQuestions = g.Game.TotalQuestions,
                    IsPublic = g.Game.IsPublic
                });
            if (isActive != null && isActive == true)
            {
                games = games.Where(g => g.EndDate < DateTimeOffset.Now);
            }
            return games;
        }

        public async Task<PlayerModel> getPlayerByUsername(string username)
        {
            Player p = await _context.Players.FirstOrDefaultAsync(p => p.Username == username);
            if (p != null)
            {
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
            return null;
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
            var friendsOfPlayer = await _context.Friends
                .Where(f => f.FriendId == id)
                .Select(f => f.PlayerId)
                .ToListAsync();

            return friendsOfPlayer.Union(currentPlayer.FriendPlayers.Select(p => p.FriendId));
        }

        public async Task<PlayerModel> createPlayer(PlayerModel player)
        {
            PlayerModel searchPlayer = (await getPlayerByUsername(player.Username));
            if (searchPlayer != null)
            {
                return searchPlayer;
            }

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
            player.Id = newPlayer.Id;
            return player;
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

        public async Task deleteFriend(int playerId, int friendId)
        {
            var playerSide = _context.Friends.FirstOrDefault(f => f.PlayerId == playerId && f.FriendId == friendId);
            var friendSide = _context.Friends.FirstOrDefault(f => f.PlayerId == friendId && f.FriendId == playerId);

            if (playerSide != null)
            {
                _context.Friends.Remove(playerSide);
            }
            if (friendSide != null)
            {
                _context.Friends.Remove(friendSide);
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Save changes async
        /// </summary>
        public async Task saveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PlayerModel>> getNPlayers(int numPlayers, int index)
        {
            List<PlayerModel> players = (await getAllPlayers()).ToList();
            players.Sort((p1, p2) => p2.Points - p1.Points);
            return players.GetRange(index * numPlayers, Math.Min(numPlayers, Math.Abs((numPlayers*(index)) - players.Count()) ));
        }
    }
}
