using System;
using System.Collections.Generic;

#nullable disable

namespace TriviaServer.DAL
{
    public partial class Message
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Date { get; set; }

        public virtual Player From { get; set; }
        public virtual Player To { get; set; }
    }
}
