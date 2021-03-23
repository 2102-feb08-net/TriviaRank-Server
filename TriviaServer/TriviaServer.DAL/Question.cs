using System;
using System.Collections.Generic;

#nullable disable

namespace TriviaServer.DAL
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int Id { get; set; }
        public string Category { get; set; }
        public bool Type { get; set; }
        public string Difficulty { get; set; }
        public string Question1 { get; set; }
        public string CorrectAnswer { get; set; }
        public string IncorrectAnswer1 { get; set; }
        public string IncorrectAnswer2 { get; set; }
        public string IncorrectAnswer3 { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
