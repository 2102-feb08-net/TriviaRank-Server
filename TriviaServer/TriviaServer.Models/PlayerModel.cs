using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace TriviaServer.Models
{
  public class PlayerModel
  {
    const int NAME_LIMIT = 50;
    private int _id;
    private string _userName;
    private string _password;
    private string _firstName;
    private string _lastName;
    private DateTimeOffset _birthday;
    private int _points;

    public PlayerModel() { }

    public PlayerModel(int id, string userName, string password, string firstName, string lastName, DateTimeOffset birthday, int points)
    {
      this._id = id;
      this._userName = userName;
      this._password = password;
      this._firstName = firstName;
      this._lastName = lastName;
      this._birthday = birthday;
      this._points = points;
    }

    public string Username
    {
      get => _userName;
      set
      {
        if (StringIsOnlyAlphaNumberic(value))
        {
          _userName = value;
        }
        else
        {
          throw new InvalidOperationException("Name can only contain alphabetic letters.");
        }
      }
    }

    public string Password
    {
      get => _password;
      set
      {
        if (value.Length > 0)
        {
          _password = value;
        }
      }
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
          throw new InvalidOperationException("Id must be greater than zero");
        }
      }
    }
    public string FirstName
    {
      get => _firstName;
      set
      {
        if (StringIsValidName(value))
        {
          _firstName = value;
        }
        else
        {
          throw new InvalidOperationException("Invalid first Name");
        }
      }
    }
    public string LastName
    {
      get => _lastName;
      set
      {
        if (StringIsValidName(value))
        {
          _lastName = value;
        }
        else
        {
          throw new InvalidOperationException("Invalid last Name");
        }
      }
    }
    public DateTimeOffset Birthday
    {
      get => _birthday;
      set => _birthday = value;
    }
    public int Points
    {
      get => _points;
      set
      {
        // What to do about the MAXINT
        if (value > -1)
        {
          _points = value;
        }
        else
        {
          throw new InvalidOperationException("Points must be greater than zero");
        }
      }
    }

    /// <summary>
    /// Returns true is string is alphanumberic and is one character or longer
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public bool StringIsOnlyAlphaNumberic(string str)
    {
      // length of username must be > 0 and < 51
      if (str.Length == 0 || str.Length > NAME_LIMIT)
      {
        return false;
      }


      if (!Regex.IsMatch(str, "^[a-zA-Z0-9]*$"))
      {
        return false;
      }

      return true;
    }
    /// <summary>
    /// Returns true for a string longer than 1 and upto 50 characters, is only made of letters, and the first letter is capitalized.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public bool StringIsValidName(string str)
    {
      // check length
      if (str.Length == 0 || str.Length > NAME_LIMIT)
      {
        return false;
      }
      // check if all letters
      if (!Regex.IsMatch(str, "^[a-zA-Z]*$"))
      {
        return false;
      }
      // check if first letter is capitalized
      if (!char.IsUpper(str[0]))
      {
        return false;
      }

      return true;
    }

  }
}
