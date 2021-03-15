using System;
using System.Collections.Generic;

#nullable disable

namespace TriviaServer.DAL
{
    public partial class Friend
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int FriendId { get; set; }

        public virtual Player FriendNavigation { get; set; }
        public virtual Player Player { get; set; }
    }
}
