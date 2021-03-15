using System;
using System.Collections.Generic;

#nullable disable

namespace TriviaServer.DAL
{
    public partial class Player
    {
        public Player()
        {
            FriendFriendNavigations = new HashSet<Friend>();
            FriendInviteOutboxInviteds = new HashSet<FriendInviteOutbox>();
            FriendInviteOutboxInviters = new HashSet<FriendInviteOutbox>();
            FriendPlayers = new HashSet<Friend>();
            GameInviteOutboxes = new HashSet<GameInviteOutbox>();
            GamePlayers = new HashSet<GamePlayer>();
            Games = new HashSet<Game>();
            MessageFroms = new HashSet<Message>();
            MessageTos = new HashSet<Message>();
            PlayerStatistics = new HashSet<PlayerStatistic>();
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTimeOffset Birthday { get; set; }
        public int Points { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Friend> FriendFriendNavigations { get; set; }
        public virtual ICollection<FriendInviteOutbox> FriendInviteOutboxInviteds { get; set; }
        public virtual ICollection<FriendInviteOutbox> FriendInviteOutboxInviters { get; set; }
        public virtual ICollection<Friend> FriendPlayers { get; set; }
        public virtual ICollection<GameInviteOutbox> GameInviteOutboxes { get; set; }
        public virtual ICollection<GamePlayer> GamePlayers { get; set; }
        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<Message> MessageFroms { get; set; }
        public virtual ICollection<Message> MessageTos { get; set; }
        public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
