using System;
using System.Collections.Generic;

#nullable disable

namespace TriviaServer.DAL
{
    public partial class GameInviteOutbox
    {
        public int Id { get; set; }
        public int InvitedId { get; set; }
        public DateTimeOffset Date { get; set; }
        public int GameId { get; set; }

        public virtual Game Game { get; set; }
        public virtual Player Invited { get; set; }
    }
}
