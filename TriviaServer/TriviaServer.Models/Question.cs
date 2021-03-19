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







  }
}