﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoClicker.Services.WindowHelper;
using AutoClicker.ViewModels;

namespace AutoClicker.Models.Click.States
{
    internal class Repeats
    {
        private readonly int _repeats;
        private static readonly MainWindowViewModel MainWindow = ViewModelLocatorProvider.MainWindow;

        public Repeats()
        {
            _repeats = GetRepeats();
        }

        public int TotalTimes => _repeats;

        private int GetRepeats()
        {
            var isEndless = MainWindow.IsRepeatUntilStoppedSelected;

            return isEndless 
                ? -1 
                : int.Parse(MainWindow.RepeatTimesTextBox);
        }
    }
}
