namespace AutoClicker.Models.Parsing
{
    internal static class RepeatParser
    {
        public static int ParseRepeatCount(bool isRepeatUntilStopped, string? repeatTimes)
        {
            if (isRepeatUntilStopped)
            {
                return -1;
            }

            if (string.IsNullOrWhiteSpace(repeatTimes))
            {
                return 0;
            }

            if (!int.TryParse(repeatTimes, out var times))
            {
                return 0;
            }

            return times < 0 ? 0 : times;
        }
    }
}
