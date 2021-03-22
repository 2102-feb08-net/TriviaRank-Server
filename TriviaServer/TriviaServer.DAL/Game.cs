using System;
using System.Collections.Generic;

#nullable disable

namespace TriviaServer.DAL
{
    public partial class Game
    {
        public Game()
        {
            Answers = new HashSet<Answer>();
            GameInviteOutboxes = new HashSet<GameInviteOutbox>();
            GamePlayers = new HashSet<GamePlayer>();
        }

        public int Id { get; set; }
        public string GameName { get; set; }
        public int OwnerId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public bool GameMode { get; set; }
        public int TotalQuestions { get; set; }
        public bool IsPublic { get; set; }

        public virtual Player Owner { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<GameInviteOutbox> GameInviteOutboxes { get; set; }
        public virtual ICollection<GamePlayer> GamePlayers { get; set; }
    }
}
