using AutoClicker.Models.Parsing;
using Xunit;

namespace AutoClicker.Tests.Parsing
{
    public class IntervalParserTests
    {
        [Theory]
        [InlineData("0", "0", "1", "0", 1000)]
        [InlineData("0", "1", "2", "3", 62003)]
        [InlineData("1", "0", "0", "0", 3600000)]
        [InlineData("", "", "", "", 0)]
        [InlineData("-1", "0", "0", "0", 0)]
        [InlineData("0", "-1", "0", "0", 0)]
        [InlineData("0", "0", "-1", "0", 0)]
        [InlineData("0", "0", "0", "-5", 0)]
        [InlineData("abc", "0", "0", "0", 0)]
        public void CalculateMilliseconds_ReturnsExpected(
            string hours,
            string minutes,
            string seconds,
            string milliseconds,
            int expected)
        {
            var result = IntervalParser.CalculateMilliseconds(hours, minutes, seconds, milliseconds);

            Assert.Equal(expected, result);
        }
    }
}
