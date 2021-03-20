using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace TriviaServer.Models
{
  public class Question
  {
    private int _id;
    private string _category;
    // true = multiple choice, false = true/false
    private bool _type;
    // 0 = easy, 1 = medium, 2 = hard
    private int _difficulty;
    private string _statement;

    private int _answer;


    public Question() { }

    public int Id
    {
      get => _id;
      set
      {
        if (IdIsOkayFromDatabase(value))
        {
          _id = value;
        }
        else
        {
          throw new InvalidOperationException("Invalid question Id from database");
        }
      }
    }





    public bool IdIsOkayFromDatabase(int value)
    {
      if (value < 1 || value > Int32.MaxValue)
      {
        return false;
      }

      return true;

      //throw new NotImplementedException("Not done");
    }



  }
}