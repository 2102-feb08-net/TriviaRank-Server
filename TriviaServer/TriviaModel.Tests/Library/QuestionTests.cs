using System;
using TriviaServer.Models;
using Xunit;

namespace TriviaServer.Tests
{
  public class QuestionIdIsOkay
  {
    [Theory]
    [InlineData(1)]
    [InlineData(Int32.MaxValue)]

    public void QuestionId_IsWithInBounds_ReturnTrue(int id)
    {
      // arrange
      var question = new Question();
      int testId = id;

      // act
      bool isIdOkay = question.IdIsOkayFromDatabase(testId);
      // assert
      Assert.True(isIdOkay);
    }
  }
  public class QuestionIdIsNotOkay
  {
    [Theory]
    [InlineData(0)]
    // can't test Int32.MaxValue + 1
    [InlineData(Int32.MinValue)]
    public void QuestionId_IsNotWithInBounds_ReturnTrue(int id)
    {
      // arrange
      var question = new Question();
      int testId = id;

      // act
      bool isIdOkay = question.IdIsOkayFromDatabase(testId);
      // assert
      Assert.False(isIdOkay);
    }
  }

}


