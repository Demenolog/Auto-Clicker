using AutoClicker.Models.Clicks;
using AutoClicker.Models.Parsing;

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

        private int GetIntervalTime()
        {
            return IntervalParser.CalculateMilliseconds(_hours, _minutes, _seconds, _milliseconds);
        }
    }
}
