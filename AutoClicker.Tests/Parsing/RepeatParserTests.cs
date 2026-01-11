using AutoClicker.Models.Parsing;
using Xunit;

namespace AutoClicker.Tests.Parsing
{
    public class RepeatParserTests
    {
        [Theory]
        [InlineData(true, "5", -1)]
        [InlineData(true, "", -1)]
        [InlineData(false, "10", 10)]
        [InlineData(false, "-5", 0)]
        [InlineData(false, "", 0)]
        [InlineData(false, "abc", 0)]
        public void ParseRepeatCount_ReturnsExpected(bool isRepeatUntilStopped, string repeatTimes, int expected)
        {
            var result = RepeatParser.ParseRepeatCount(isRepeatUntilStopped, repeatTimes);

            Assert.Equal(expected, result);
        }
    }
}
