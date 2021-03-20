using System;
using TriviaServer.Models;
using Xunit;

namespace TriviaServer.Model.Tests
{
  public class GameNameIsOkay
  {
    [Theory]
    [InlineData("I")]
    [InlineData("i")]
    [InlineData("I made a game name that is fifty characters long..")]
    [InlineData("8")]
    [InlineData("A9")]
    [InlineData("a9")]

    public void GameName_IsInNameLengthRange1to50Inclusive_ReturnTrue(string gameName)
    {
      // arrange
      var game = new GameModel();
      string testString = gameName;
      // act
      bool isGameNameOkay = game.isGameNameOkay(gameName);
      // assert
      Assert.True(isGameNameOkay);
    }
  }

  public class GameNameIsNotOkay
  {
    [Theory]
    [InlineData("")]
    [InlineData("I made a game name that is fiftyone characters long")]
    public void GameName_IsOutOfLengthRange1to50Include_ReturnFalse(string gameName)
    {
      // arrange
      var game = new GameModel();
      string testString = gameName;
      // act assert
      bool isGameNameOkay = game.isGameNameOkay(gameName);
      // assert
      Assert.False(isGameNameOkay);
    }
  }
}