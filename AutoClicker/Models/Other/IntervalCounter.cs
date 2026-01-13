using System;

namespace AutoClicker.Models.Other
{
    internal static class IntervalCounter
    {
        public static int GetTotalIntervalTime(string hours, string minutes, string seconds, string milliseconds)
        {
            var interval = (int)new TimeSpan(0, int.Parse(hours), int.Parse(minutes), int.Parse(seconds), int.Parse(milliseconds)).TotalMilliseconds;

            return interval;
        }
    }
}