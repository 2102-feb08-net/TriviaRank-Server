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
    public class OutboxRepository : IOutboxRepository
    {
        private readonly TriviaRankContext _context;
        public OutboxRepository(TriviaRankContext context)
        {
            _context = context;
        }

        public async Task<int> createFriendInvite(int playerId, int friendId)
        {
            FriendInviteOutbox fObx = new FriendInviteOutbox()
            {
                InviterId = playerId,
                InvitedId = friendId,
                Date = DateTimeOffset.Now
            };

            await _context.AddAsync(fObx);
            await saveAsync();
            return fObx.Id;
        }

        public async Task<int> createFriendInvite(string playerUsername, string friendUsername)
        {
            Player player = await _context.Players.FirstOrDefaultAsync(p => p.Username == playerUsername);
            Player friend = await _context.Players.FirstOrDefaultAsync(p => p.Username == friendUsername);
            FriendInviteOutbox fObx = new FriendInviteOutbox()
            {
                InviterId = player.Id,
                InvitedId = friend.Id,
                Date = DateTimeOffset.Now
            };

            await _context.AddAsync(fObx);
            await saveAsync();
            return fObx.Id;
        }

        public async Task deleteFriendInvite(string playerUsername, string friendUsername)
        {
            Player player = await _context.Players.FirstOrDefaultAsync(p => p.Username == playerUsername);
            Player friend = await _context.Players.FirstOrDefaultAsync(p => p.Username == friendUsername);
            List<FriendInviteOutbox> invites = await _context.FriendInviteOutboxes
                .Where(o => o.InvitedId == player.Id && o.InviterId == friend.Id)
                    .Union(_context.FriendInviteOutboxes.Where(o => o.InviterId == player.Id && o.InvitedId == friend.Id))
                    .ToListAsync();

            foreach (var invite in invites)
            {
                _context.FriendInviteOutboxes.Remove(invite);
            }

            await saveAsync();
        }

        public async Task<int> createGameInvite(int gameId, int playerId)
        {
            GameInviteOutbox gObx = new GameInviteOutbox()
            {
                GameId = gameId,
                InvitedId = playerId,
                Date = DateTimeOffset.Now
            };

            await _context.AddAsync(gObx);
            await saveAsync();
            return gObx.Id;
        }

        public async Task<List<PlayerModel>> getFriendInvites(int currentPlayer)
        {
            return await _context.FriendInviteOutboxes
                .Include(o => o.Inviter)
                .Where(o => o.InvitedId == currentPlayer)
                .Select(o => new PlayerModel()
                {
                    Id = o.Inviter.Id,
                    FirstName = o.Inviter.FirstName,
                    LastName = o.Inviter.LastName,
                    Birthday = o.Inviter.Birthday,
                    Username = o.Inviter.Username,
                    Points = o.Inviter.Points
                })
                .ToListAsync();
        }

        public async Task<List<GameModel>> getGameInvites(int currentPlayer)
        {
            return await _context.GameInviteOutboxes
                .Include(o => o.Game)
                .Where(o => o.InvitedId == currentPlayer)
                .Select(o => new GameModel()
                {
                    Id = o.Game.Id,
                    OwnerId = o.Game.OwnerId,
                    GameName = o.Game.GameName,
                    StartDate = o.Game.StartDate,
                    EndDate = o.Game.EndDate,
                    GameMode = o.Game.GameMode,
                    TotalQuestions = o.Game.TotalQuestions,
                    IsPublic = o.Game.IsPublic
                })
                .ToListAsync();
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
