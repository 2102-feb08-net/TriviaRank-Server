using System;
using TriviaServer.Models;
using Xunit;

namespace TriviaServer.Tests
{
  public class PlayerUsernameIsOkay
  {
    [Theory]
    [InlineData("jack")]
    [InlineData("123")]
    [InlineData("jack123")]
    [InlineData("JACK")]
    [InlineData("JACK123")]
    [InlineData("Jack123")]
    [InlineData("")]

    public void PlayerUserName_HasOnlyAlphaNumericChars_ReturnTrue(string userName)
    {
      // arrange
      var player = new TriviaPlayer();
      string testString = userName;

      // act
      bool isFirstNameOkay = player.StringIsOnlyAlphaNumberic(testString);
      // assert
      Assert.True(isFirstNameOkay);
    }
  }
  public class PlayerUserNameIsNotOkay
  {
    [Theory]
    [InlineData("@")]
    [InlineData("!23")]
    [InlineData("j@ck")]
    [InlineData("J@CK")]
    [InlineData("J@CK!23")]
    [InlineData("J@ck!23")]
    public void PlayerUserName_HasOnlyAlphaNumericChars_ReturnFalse(string userName)
    {
      // arrange

      var player = new TriviaPlayer();
      string testString = userName;

      // act
      bool isFirstNameOkay = player.StringIsOnlyAlphaNumberic(testString);
      // assert
      Assert.False(isFirstNameOkay);
    }
  }

  public class FirstOrLastNameForPlayerIsOkay
  {
    [Theory]
    [InlineData("Bob")]

    public void StringisValidName_OnlyCharsFirstCapitalized_ReturnTrue(string userName)
    {
      // arrange
      var player = new TriviaPlayer();
      string testString = userName;

      // act
      bool isNameOkay = player.StringisValidName(testString);
      // assert
      Assert.True(isNameOkay);
    }
  }
  public class FirstOrLastNameForPlayerIsNotOkay
  {
    [Theory]
    [InlineData("jack")]
    [InlineData("Jack1")]
    [InlineData("")]

    public void StringisValidName_NoCapitalLettersAndNumbersLength0_ReturnFalse(string userName)
    {
      // arrange
      var player = new TriviaPlayer();
      string testString = userName;

      // act
      bool isNameOkay = player.StringisValidName(testString);
      // assert
      Assert.False(isNameOkay);
    }
  }

}
