using AutoClicker.Services.WindowHelper;
using AutoClicker.ViewModels;
using System;

namespace AutoClicker.Models.Click.States
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

        private int GetIntervalTime()
        {
            var days = 0;
            var hours = int.Parse(_hours);
            var minutes = int.Parse(_minutes);
            var seconds = int.Parse(_seconds);
            var milliseconds = int.Parse(_milliseconds);

            var interval = (int)new TimeSpan(days, hours, minutes, seconds, milliseconds).TotalMilliseconds;

            return interval;
        }
    }
}