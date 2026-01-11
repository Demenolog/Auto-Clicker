using System;
using AutoClicker.Models.Clicks;

namespace AutoClicker.Models.States
{
    internal class Interval
    {
        private readonly string _hours;
        private readonly string _minutes;
        private readonly string _seconds;
        private readonly string _milliseconds;
        private readonly int _totalTime;

        public Interval(ClickIntervalConfig config)
        {
            _hours = config.Hours;
            _minutes = config.Minutes;
            _seconds = config.Seconds;
            _milliseconds = config.Milliseconds;
            _totalTime = GetIntervalTime();
        }

        public int TotalTime => _totalTime;

        private static int SafeParseNonNegative(string text)
        {
            if (!int.TryParse(text, out var value))
                return 0;

            if (value < 0)
                return 0;

            return value;
        }

        private int GetIntervalTime()
        {
            var days = 0;
            var hours = SafeParseNonNegative(_hours);
            var minutes = SafeParseNonNegative(_minutes);
            var seconds = SafeParseNonNegative(_seconds);
            var milliseconds = SafeParseNonNegative(_milliseconds);

            var interval = (int)new TimeSpan(days, hours, minutes, seconds, milliseconds).TotalMilliseconds;

            // Guard against overflow or negative results
            if (interval < 0)
                interval = 0;

            return interval;
        }
    }
}
