using System;
using System.Collections.Generic;

#nullable disable

namespace TriviaServer.DAL
{
    public partial class FriendInviteOutbox
    {
        public int Id { get; set; }
        public int InviterId { get; set; }
        public int InvitedId { get; set; }
        public DateTimeOffset Date { get; set; }

        public virtual Player Invited { get; set; }
        public virtual Player Inviter { get; set; }
    }
}
