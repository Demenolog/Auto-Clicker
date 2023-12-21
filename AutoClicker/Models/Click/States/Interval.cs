using AutoClicker.Services.WindowHelper;
using AutoClicker.ViewModels;
using System;

namespace AutoClicker.Models.Click.States
{
    internal class Interval
    {
        private string _hours;
        private string _minutes;
        private string _seconds;
        private string _milliseconds;
        private static readonly MainWindowViewModel MainWindow = ViewModelLocatorProvider.MainWindow;

        public Interval()
        {
            _hours = MainWindow.HoursTextBox;
            _minutes = MainWindow.MinutesTextBox;
            _seconds = MainWindow.SecondsTextBox;
            _milliseconds = MainWindow.MillisecondsTextBox;
        }

        public int GetIntervalTime()
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