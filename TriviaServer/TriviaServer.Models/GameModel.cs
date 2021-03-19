using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaServer.Models
{
  public class GameModel
  {
    private int _id;
    private string _gameName;

    private int _ownerId;
    private DateTimeOffset _startDate;
    private DateTimeOffset _endDate;
    private bool _gameMode;

    private int _totalQuestions;
    private bool _isPublic;

    private Dictionary<string, Question> _questions;

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
      Dictionary<string, Question> questions)
    {
      this._id = id;
      this._gameName = gameName;
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
        if (value.Length > 0)
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
        if (value > 0)
        {
          _totalQuestions = value;
        }
        else
        {
          throw new InvalidOperationException("Game does not have questions");
        }
      }
    }
    public bool IsPublic
    {
      get => _isPublic;
      set => _isPublic = value;
    }
  }
}
