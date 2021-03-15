using System;
using System.Collections.Generic;

#nullable disable

namespace TriviaServer.DAL
{
    public partial class GamePlayer
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int TotalCorrect { get; set; }

        public virtual Game Game { get; set; }
        public virtual Player Player { get; set; }
    }
}
