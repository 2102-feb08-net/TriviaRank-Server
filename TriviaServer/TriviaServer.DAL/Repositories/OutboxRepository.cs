﻿using System;
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
            bool invalidPlayer = await _context.Players.FindAsync(playerId) == null;
            bool invalidFriend = await _context.Players.FindAsync(friendId) == null;
            bool inviteExists = await _context.FriendInviteOutboxes.FirstOrDefaultAsync(o => o.InviterId == playerId && o.InvitedId == friendId) != null;
            if (invalidPlayer || invalidFriend || inviteExists)
            {
                throw new InvalidOperationException("invalid client operation");
            }

            FriendInviteOutbox fObx = new FriendInviteOutbox()
            {
                InviterId = playerId,
                InvitedId = friendId,
                Date = DateTimeOffset.Now
            };

            _context.Add(fObx);
            await saveAsync();
            return fObx.Id;
        }

        public async Task<int> createGameInvite(int gameId, int playerId)
        {
            bool invalidPlayer = await _context.Players.FindAsync(playerId) == null;
            bool invalidGame = await _context.Games.FindAsync(gameId) == null;
            bool inviteExists = await _context.GameInviteOutboxes.FirstOrDefaultAsync(o => o.GameId == gameId && o.InvitedId == playerId) != null;
            if (invalidPlayer || invalidGame || inviteExists)
            {
                throw new InvalidOperationException("invalid client operation");
            }

            GameInviteOutbox gObx = new GameInviteOutbox()
            {
                GameId = gameId,
                InvitedId = playerId,
                Date = DateTimeOffset.Now
            };

            _context.Add(gObx);
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
