using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaServer.Models
{
    public class GameDTO
    {
        public int OwnerId { get; set; }
        public string GameName { get; set; }
        public int TotalQuestions { get; set; }
        public bool IsPublic { get; set; }
        public double Duration { get; set; }

    }
}
