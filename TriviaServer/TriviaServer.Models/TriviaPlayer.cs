using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TriviaServer.Models
{

  public class TriviaPlayer
  {
    const int NAME_LIMIT = 50;
    public string Username { get; set; }
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public int Points { get; set; }

    /// <summary>
    /// Returns true is string is alphanumberic and is one character or longer
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public bool StringIsOnlyAlphaNumberic(string str)
    {
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
