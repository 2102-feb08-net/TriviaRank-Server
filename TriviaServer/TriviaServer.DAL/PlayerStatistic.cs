using System;
using System.Collections.Generic;

#nullable disable

namespace TriviaServer.DAL
{
    public partial class PlayerStatistic
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string Selection { get; set; }
        public int Frequency { get; set; }

        public virtual Player Player { get; set; }
    }
}
