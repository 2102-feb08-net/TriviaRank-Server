using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaServer.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string senderUsername { get; set; }
        public string receiverUsername { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
