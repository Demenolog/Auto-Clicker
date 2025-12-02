using System;
using AutoClicker.Services.WindowHelper;
using AutoClicker.ViewModels;

namespace AutoClicker.Models.States
{
    internal class Interval
    {
        private readonly string _hours;
        private readonly string _minutes;
        private readonly string _seconds;
        private readonly string _milliseconds;
        private readonly int _totalTime;

        private static readonly MainWindowViewModel MainWindow = ViewModelLocatorProvider.MainWindow;

        public Interval()
        {
            _hours = MainWindow.HoursTextBox;
            _minutes = MainWindow.MinutesTextBox;
            _seconds = MainWindow.SecondsTextBox;
            _milliseconds = MainWindow.MillisecondsTextBox;
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
