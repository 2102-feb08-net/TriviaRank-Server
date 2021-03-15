using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaServer.Models.Repositories
{
    public interface IOutboxRepository
    {
        public Task<List<PlayerModel>> getFriendInvites(int currentPlayer);
        public Task<List<GameModel>> getGameInvites(int currentPlayer);
        public Task<int> createFriendInvite(int playerId, int friendId);
        public Task<int> createGameInvite(int gameId, int playerId);
    }
}
