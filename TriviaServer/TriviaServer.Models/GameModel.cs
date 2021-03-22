using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaServer.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public int OwnerId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public bool GameMode { get; set; }
        public int TotalQuestions { get; set; }
        public bool IsPublic { get; set; }
        public double Duration { get; set; }
        public List<QuestionsModel> Questions { get; set; }
    }
}
