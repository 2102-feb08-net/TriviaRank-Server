using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriviaServer.Models;
using TriviaServer.Models.Repositories;

namespace TriviaServer.DAL.Repositories
{
  public class GameRepository : IGameRepository
  {
    private readonly TriviaRankContext _context;
    public GameRepository(TriviaRankContext context)
    {
      _context = context;
    }

    public async Task<GameModel> AddPlayerToGame(int gameId, int playerId)
    {
      DateTimeOffset _now = DateTimeOffset.Now;

      var searchGame = await _context.Games.Where(x => x.Id == gameId && DateTimeOffset.Compare(x.EndDate, _now) > 0 && x.IsPublic == true).FirstAsync();

      if (searchGame != null)
      {
        GameModel appGame = new GameModel
        {
          Id = searchGame.Id,
          GameName = searchGame.GameName,
          OwnerId = searchGame.OwnerId,
          StartDate = searchGame.StartDate,
          EndDate = searchGame.EndDate,
          GameMode = searchGame.GameMode,
          IsPublic = searchGame.IsPublic,
          TotalQuestions = searchGame.TotalQuestions,
          PlayerId = playerId
        };

        appGame.Questions = new List<QuestionsModel>();

        List<int> questionList = await _context.Answers
            .Where(x => x.GameId == searchGame.Id)
            .Select(x => x.QuestionId)
            .Distinct()
            .ToListAsync();


        foreach (var question in questionList)
        {
          Question dbQuestion = await _context.Questions
              .Where(x => x.Id == question).FirstAsync();

          var appQuestion = new QuestionsModel();

          appQuestion.Answers.Add(dbQuestion.CorrectAnswer);
          appQuestion.Answers.Add(dbQuestion.IncorrectAnswer1);
          appQuestion.Answers.Add(dbQuestion.IncorrectAnswer2);
          appQuestion.Answers.Add(dbQuestion.IncorrectAnswer3);
          appQuestion.Id = dbQuestion.Id;
          appQuestion.Question = dbQuestion.Question1;

          appQuestion.Answers = appQuestion.Answers.Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => Guid.NewGuid()).ToList();

          appGame.Questions.Add(appQuestion);

        }
        return appGame;
      }
      else
      {
        throw new ArgumentException("Game not found");
      }

    }

    public async Task UpdatePlayerScore(int gameId, int playerId, int score)
    {
      var player = await _context.GamePlayers.FirstOrDefaultAsync(p => p.PlayerId == playerId);
      player.TotalCorrect = score;
      _context.Update(player);

      await _context.SaveChangesAsync();
    }

    public async Task<List<GameModel>> GetAllGames()
    {
      var dbGames = await _context.Games.ToListAsync();

      List<GameModel> gameList = new List<GameModel>();

      foreach (var game in dbGames)
      {
        var eachGame = new GameModel
        {
          Id = game.Id,
          GameName = game.GameName,
          OwnerId = game.OwnerId,
          StartDate = game.StartDate,
          EndDate = game.EndDate,
          GameMode = game.GameMode,
          IsPublic = game.IsPublic,
          TotalQuestions = game.TotalQuestions
        };
        gameList.Add(eachGame);
      }
      return gameList;

    }

    public async Task<List<GameModel>> SearchAllGames()
    {
      DateTimeOffset _now = DateTimeOffset.Now;

      var dbGames = await _context.Games
          .Where(x => DateTimeOffset.Compare(x.EndDate, _now) > 0 && x.IsPublic == true)
          .ToListAsync();

      List<GameModel> gameList = new List<GameModel>();

      foreach (var game in dbGames)
      {
        var eachGame = new GameModel
        {
          Id = game.Id,
          GameName = game.GameName,
          OwnerId = game.OwnerId,
          StartDate = game.StartDate,
          EndDate = game.EndDate,
          GameMode = game.GameMode,
          IsPublic = game.IsPublic,
          TotalQuestions = game.TotalQuestions
        };
        gameList.Add(eachGame);
      }
      return gameList;
    }

    public async Task<Models.GameModel> CreateGame(GameModel game)
    {
      Game newGame = new Game
      {
        GameName = game.GameName,
        OwnerId = game.OwnerId,
        StartDate = DateTime.Now,
        EndDate = game.EndDate,
        TotalQuestions = game.TotalQuestions,
        IsPublic = game.IsPublic
      };

      await _context.AddAsync(newGame);
      await _context.SaveChangesAsync();

      var owner = await _context.GamePlayers.ToListAsync();

      var player = new GamePlayer
      {
        GameId = newGame.Id,
        PlayerId = game.OwnerId
      };

      await _context.AddAsync(player);
      await _context.SaveChangesAsync();

      Models.GameModel appGame = new Models.GameModel
      {
        Id = newGame.Id,
        GameName = newGame.GameName,
        OwnerId = newGame.OwnerId,
        StartDate = newGame.StartDate,
        EndDate = newGame.EndDate,
        TotalQuestions = newGame.TotalQuestions,
        IsPublic = newGame.IsPublic
      };
      return appGame;
    }

        public async Task<int> EndGame(GameModel appGame)
        {
            List<Answer> answers = await _context.Answers
                .Where(x => x.GameId == appGame.Id)
                .Where(x => x.PlayerId == appGame.PlayerId)
                .ToListAsync();

            foreach(var question in appGame.Questions)
            {
                foreach(var answer in answers)
                {
                    if(answer.QuestionId == question.Id)
                    {
                        answer.PlayerAnswer = question.PlayerAnswer;
                        _context.Update(answer.PlayerAnswer);
                    }
                }
            }

            _context.Update(answers);
            await _context.SaveChangesAsync();
            return await ScoreCalculation(appGame);
        }

        public async Task<int> ScoreCalculation(GameModel appGame)
        {
            List<QuestionsModel> dbQuestions = new List<QuestionsModel>();

            foreach(var question in appGame.Questions)
            {
                Question query = await _context.Questions
                    .Where(x => x.Id == question.Id).FirstAsync();

                QuestionsModel dbQuestion = new QuestionsModel();

                dbQuestion.Id = query.Id;
                dbQuestion.Answers[0] = query.CorrectAnswer;

                dbQuestions.Add(dbQuestion);
                
            }
            return CalculateScore(appGame, dbQuestions);
        }

        public async Task<GameModel> getAnyGame(int gameId)
    {
      try
      {
        DateTimeOffset _now = DateTimeOffset.Now;

        Game game = await _context.Games.Where(x => x.Id == gameId).FirstOrDefaultAsync();

        if (game != null)
        {
          Models.GameModel appGame = new Models.GameModel
          {
            Id = game.Id,
            GameName = game.GameName,
            OwnerId = game.OwnerId,
            StartDate = game.StartDate,
            EndDate = game.EndDate,
            TotalQuestions = game.TotalQuestions,
            IsPublic = game.IsPublic
          };
          return appGame;
        }
        else
        {
          throw new ArgumentException("No game exists.");
        }
      }
      catch (Exception e)
      {
        throw new Exception("Something Happened", e);
      }
    }

    public async Task<Models.GameModel> SearchGames(int appGameID)
    {
      try
      {
        DateTimeOffset _now = DateTimeOffset.Now;

        Game gameQuery = await _context.Games.Where(x => x.Id == appGameID).FirstAsync();

        if (DateTimeOffset.Compare(gameQuery.EndDate, _now) > 0 && gameQuery.IsPublic == true)
        {
          Models.GameModel appGame = new Models.GameModel
          {
            Id = gameQuery.Id,
            GameName = gameQuery.GameName,
            OwnerId = gameQuery.OwnerId,
            StartDate = gameQuery.StartDate,
            EndDate = gameQuery.EndDate,
            TotalQuestions = gameQuery.TotalQuestions,
            IsPublic = gameQuery.IsPublic
          };
          return appGame;
        }
        else
        {
          throw new ArgumentException("No game exists.");
        }
      }
      catch (Exception e)
      {
        throw new Exception("Something Happened", e);
      }
    }

    public async Task<int> AddPlayerToGame(int gameId, PlayerModel player)
    {
      int gamePlayerId = -1;
      try
      {
        GamePlayer gp = new GamePlayer
        {
          GameId = gameId,
          PlayerId = player.Id,
          TotalCorrect = 0,
        };
        await _context.GamePlayers.AddAsync(gp);
        await _context.SaveChangesAsync();
        gamePlayerId = gp.Id;
      }
      catch (Exception e)
      {
        throw new Exception("Something Happened", e);
      }
      return gamePlayerId;
    }
    public async Task<QuestionsDTO> AddQuestions(QuestionsDTO questions)
    {
      foreach (var question in questions.Results)
      {
        Question dbQuestion = new Question()
        {
          Category = question.Category,
          Difficulty = question.Difficulty,
          Question1 = question.Question,
          CorrectAnswer = question.CorrectAnswer
        };

        if (question.Type.Equals("boolean"))
        {
          dbQuestion.Type = false;
        }
        else
        {
          dbQuestion.Type = true;
        }

        if (question.IncorrectAnswers.Count > 1)
        {
          dbQuestion.IncorrectAnswer1 = question.IncorrectAnswers[0];
          dbQuestion.IncorrectAnswer2 = question.IncorrectAnswers[1];
          dbQuestion.IncorrectAnswer3 = question.IncorrectAnswers[2];
        }
        else
        {
          dbQuestion.IncorrectAnswer1 = question.IncorrectAnswers[0];
        }
        await _context.AddAsync(dbQuestion);
      }

      _context.SaveChanges();

      foreach (var question in questions.Results)
      {
        question.Id = _context.Questions.Where(x => x.Question1.Equals(question.Question)).Select(x => x.Id).First();
      }

      return questions;
    }

    public async Task LinkGame(GameModel game)
    {
      if (game.PlayerId == 0)
      {
        foreach (var question in game.Questions)
        {
          Answer gameLink = new Answer
          {
            PlayerId = game.OwnerId,
            GameId = game.Id,
            QuestionId = question.Id
          };
          await _context.AddAsync(gameLink);
        }
      }
      else
      {
        foreach (var question in game.Questions)
        {
          Answer gameLink = new Answer
          {
            PlayerId = game.PlayerId,
            GameId = game.Id,
            QuestionId = question.Id
          };
          await _context.AddAsync(gameLink);
        }
      }
      await _context.SaveChangesAsync();
    }

    public async Task<Models.GameModel> CreateGame(int ownerId, string gameName, int totalQuestions, bool isPublic, double duration)
    {
      Game newGame = new Game
      {
        GameName = gameName,
        OwnerId = ownerId,
        StartDate = DateTime.Now,
        EndDate = DateTimeOffset.Now.AddMinutes(duration),
        TotalQuestions = totalQuestions,
        IsPublic = isPublic
      };

      await _context.AddAsync(newGame);
      await _context.SaveChangesAsync();

      var owner = await _context.GamePlayers.ToListAsync();

      var player = new GamePlayer
      {
        GameId = newGame.Id,
        PlayerId = ownerId
      };

      await _context.AddAsync(player);
      await _context.SaveChangesAsync();

      Models.GameModel appGame = new Models.GameModel
      {
        Id = newGame.Id,
        GameName = newGame.GameName,
        OwnerId = newGame.OwnerId,
        StartDate = newGame.StartDate,
        EndDate = newGame.EndDate,
        TotalQuestions = newGame.TotalQuestions,
        IsPublic = newGame.IsPublic
      };
      return appGame;
    }
  }
}
