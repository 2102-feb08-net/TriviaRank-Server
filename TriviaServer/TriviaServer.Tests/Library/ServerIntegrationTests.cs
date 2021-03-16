using System;
using TriviaServer.Models;
using Xunit;

namespace TriviaServer.Tests
{
  public class PlayerUsernameIsOkay
  {
    [Fact]
    public void PlayerUserName_HasAlphaNumericChars_ReturnTrue()
    {

    }
  }
  public class PlayerUserNameIsNotOkay
  {
    [Fact]
    public void PlayerUserName_HasAlphaNumericChars_ReturnFalse()
    {

    }
  }
}
