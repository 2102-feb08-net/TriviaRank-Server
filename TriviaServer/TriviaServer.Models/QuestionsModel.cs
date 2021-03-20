using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TriviaServer.Models
{
    public class QuestionsModel
    {
        public string Question { get; set; }
        public List<string> Answers { get; set; }

        //public QuestionsModel BuildQuestionList(HttpWebRequest questionList)
        //{
            
        //}

        //public QuestionsModel ShuffleAnswers(QuestionsModel appQuestions)
        //{

        //    return appQuestions;
        //}
    }
}
