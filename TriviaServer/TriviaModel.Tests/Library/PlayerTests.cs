using System;
using TriviaServer.Models;
using Xunit;

namespace TriviaServer.Tests
{
  public class PlayerFirstNameOkay
  {
    [Fact]
    public void PlayerFirstName_HasAlphaNumericChars_ReturnTrue()
    {
      // arrange
      var player = new TriviaPlayer();

      // act
      bool isFirstNameOkay = player.StringIsOnlyAlphaNumberic("Jack");
      // assert
      Assert.True(isFirstNameOkay);
    }
  }
}
