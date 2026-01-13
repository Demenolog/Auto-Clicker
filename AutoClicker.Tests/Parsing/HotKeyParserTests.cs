using AutoClicker.Models.Parsing;
using Xunit;

namespace AutoClicker.Tests.Parsing
{
    public class HotKeyParserTests
    {
        [Theory]
        [InlineData("Control+Shift+A", "Control+Shift+A")]
        [InlineData("shift+ctrl+a", "Control+Shift+A")]
        [InlineData("Alt+F4", "Alt+F4")]
        [InlineData("Win+X", "Windows+X")]
        public void TryNormalize_ReturnsNormalizedDisplay(string input, string expected)
        {
            var success = HotKeyParser.TryNormalize(input, out var normalized);

            Assert.True(success);
            Assert.Equal(expected, normalized);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Ctrl")]
        [InlineData("Ctrl+UnknownKey")]
        [InlineData("A+B")]
        public void TryNormalize_ReturnsFalseForInvalidInputs(string input)
        {
            var success = HotKeyParser.TryNormalize(input, out _);

            Assert.False(success);
        }
    }
}
