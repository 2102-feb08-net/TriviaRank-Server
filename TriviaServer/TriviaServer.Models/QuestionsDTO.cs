using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TriviaServer.Models.Repositories
{
    public class QuestionsDTO
    {
        public int ReponseCode { get; set; }
        public List<Questions> Results { get; set; }
    }

    public class Questions
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string Question { get; set; }

        [JsonProperty(PropertyName = "correct_answer")]
        public string CorrectAnswer { get; set; }

        [JsonProperty(PropertyName = "incorrect_answers")]
        public List<string> IncorrectAnswers { get; set; }
    }
}
