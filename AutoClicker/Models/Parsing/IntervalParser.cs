using System;

namespace AutoClicker.Models.Parsing
{
    internal static class IntervalParser
    {
        public static int CalculateMilliseconds(string? hours, string? minutes, string? seconds, string? milliseconds)
        {
            var safeHours = ParseNonNegativeInt(hours);
            var safeMinutes = ParseNonNegativeInt(minutes);
            var safeSeconds = ParseNonNegativeInt(seconds);
            var safeMilliseconds = ParseNonNegativeInt(milliseconds);

            var totalMilliseconds = (long)safeHours * 60 * 60 * 1000
                                    + (long)safeMinutes * 60 * 1000
                                    + (long)safeSeconds * 1000
                                    + safeMilliseconds;

            if (totalMilliseconds <= 0)
            {
                return 0;
            }

            return totalMilliseconds > int.MaxValue ? int.MaxValue : (int)totalMilliseconds;
        }

        private static int ParseNonNegativeInt(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            if (!int.TryParse(text, out var value))
            {
                return 0;
            }

            return value < 0 ? 0 : value;
        }
    }
}
