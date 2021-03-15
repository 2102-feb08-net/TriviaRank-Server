using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaServer.Models
{
  public class TriviaPlayer
  {
    public string Username { get; set; }
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public int Points { get; set; }

    public bool StringIsOnlyAlphaNumberic(string str)
    {
      throw new NotImplementedException("NOT IMPLEMENTED");
    }
  }
}
