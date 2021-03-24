using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaServer.Models
{
    public class GameModel
    {
        const int NAME_LENGTH_LIMIT = 50;
        const int MAX_NUMBER_OF_QUESTIONS = 50; // API allows up to 50 questions max
        private int _id;
        private string _gameName;

        private int _ownerId;
        private DateTimeOffset _startDate;
        private DateTimeOffset _endDate;
        private bool _gameMode;

        private int _totalQuestions;
        private bool _isPublic;

        private List<QuestionsModel> _questions;

        public GameModel() { }
        public GameModel(
          int id,
          string gameName,
          int ownerId,
          DateTimeOffset startDate,
          DateTimeOffset endDate,
          bool gameMode,
          int totalQuestions,
          bool isPublic,
          List<QuestionsModel> questions)
        {
            this._id = id;
            this._gameName = gameName;
            this._ownerId = ownerId;
            this._startDate = startDate;
            this._endDate = endDate;
            this._gameMode = gameMode;
            this._totalQuestions = totalQuestions;
            this._isPublic = isPublic;
            this._questions = questions;
        }
        public int Id
        {
            get => _id;
            set
            {
                if (value > 0)
                {
                    _id = value;
                }
                else
                {
                    throw new InvalidOperationException("Invalid game Id");
                }
            }
        }
        public string GameName
        {
            get => _gameName;
            set
            {
                if (isGameNameOkay(value))
                {
                    _gameName = value;
                }
                else
                {
                    throw new InvalidOperationException("Invalid Game Name");
                }
            }
        }
        public int OwnerId
        {
            get => _ownerId;
            set
            {
                if (value > 0)
                {
                    _ownerId = value;
                }
                else
                {
                    throw new InvalidOperationException("Invalid owner Id");
                }
            }
        }
        public DateTimeOffset StartDate
        {
            get => _startDate;
            set => _startDate = value;
        }
        public DateTimeOffset EndDate
        {
            get => _endDate;
            set => _endDate = value;
        }
        /// <summary>
        /// Game mode true = multiple choice false = true/false
        /// </summary>
        /// <value></value>
        public bool GameMode
        {
            get => _gameMode;
            set => _gameMode = value;
        }
        public int TotalQuestions
        {
            get => _totalQuestions;
            set
            {
                if (IsNumberOfQuestionsInGameValid(value))
                {
                    _totalQuestions = value;
                }
                else
                {
                    // game has zero or negative number of questions
                    if (value == 0)
                    {
                        throw new InvalidOperationException("Game does not have questions");
                    }
                    // game tries to have more questions than an api can give
                    else
                    {
                        throw new InvalidOperationException("Game has invalid number of questions");
                    }
                }
            }
        }

        public bool IsPublic
        {
            get => _isPublic;
            set => _isPublic = value;
        }

        public double Duration { get; set; }
        public List<QuestionsModel> Questions
        {
            get => _questions;
            set => _questions = value;
        }
        public int PlayerId { get; set; }


        // public List<QuestionModel> Questions
        // {
        //   get => _questions;
        // }

        public bool isGameNameOkay(string gameName)
        {
            if (gameName.Length == 0 || gameName.Length > NAME_LENGTH_LIMIT)
            {
                return false;
            }
            return true;
        }

        public bool isIdFromDataBaseValid(int number)
        {
            if (number < 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks to see if a question can be added to a game
        /// </summary>
        /// <param name="question"></param>
        public void AddQuestionToGame(QuestionsModel question)
        {
            if (IsNumberOfQuestionsInGameValid(_totalQuestions))
            {
                _questions.Add(question);
            }
            else
            {
                throw new Exception("Exceeded number of questions in game");
            }
        }
        /// <summary>
        /// Checks for a valid number of questions in a game
        /// </summary>
        /// <param name="numberOfQuestions"></param>
        /// <returns></returns>
        public bool IsNumberOfQuestionsInGameValid(int numberOfQuestions)
        {
            if (numberOfQuestions > 0 && numberOfQuestions <= MAX_NUMBER_OF_QUESTIONS)
            {
                return true;
            }

            return false;
            // throw new NotImplementedException("Not implemented");
        }
    }
}
