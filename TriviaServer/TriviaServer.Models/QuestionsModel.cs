using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TriviaServer.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace TriviaServer.Models
{
  public class QuestionsModel
  {
    public int Id { get; set; }
    public string Question { get; set; }
    public List<string> Answers { get; set; }
        public string PlayerAnswer { get; set; }

        public QuestionsModel()
    {
      Answers = new List<string>();
    }


    public static List<QuestionsModel> CreateAndShuffle(QuestionsDTO questions)
    {

      List<QuestionsModel> questionList = new List<QuestionsModel>();

      foreach (var question in questions.Results)
      {
        QuestionsModel appQuestion = new QuestionsModel();

        appQuestion.Id = question.Id;

        appQuestion.Question = question.Question;

        appQuestion.Answers.Add(question.CorrectAnswer);

        foreach (var answer in question.IncorrectAnswers)
        {
          string appAnswer = answer;

          appQuestion.Answers.Add(appAnswer);
        }

        appQuestion.Answers = appQuestion.Answers.OrderBy(x => Guid.NewGuid()).ToList();

        questionList.Add(appQuestion);
      }
      return questionList;
    }

    public bool IdIsOkayFromDatabase(int id)
    {
      if (id < 1)
      {
        return false;
      }

      return true;
    }
        public int CalculateScore(GameModel appGame, List<QuestionsModel> dbQuestions)
        {
            int playerScore = 0;

            foreach(var question in appGame.Questions)
            {
                foreach(var answer in dbQuestions)
                {
                    if (question.Id == answer.Id)
                    {
                        if (question.PlayerAnswer.Equals(answer.Answers[0]))
                        {
                            playerScore++;
                        }
                    }
                }
            }
            return playerScore;
        }
  }
}
