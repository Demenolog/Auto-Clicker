using AutoClicker.Services.WindowHelper;
using AutoClicker.ViewModels;
using System;
using static AutoClicker.Infrastructure.Constans.MouseClass.MouseClassConstans;

namespace AutoClicker.Models.Click.States
{
    internal class Options
    {
        private string _mouseButton;
        private string _mouseButtonMode;
        private static readonly MainWindowViewModel MainWindow = ViewModelLocatorProvider.MainWindow;

        public Options()
        {
            _mouseButton = MainWindow.SelectedMouseButton;
            _mouseButtonMode = MainWindow.SelectedMouseButtonMode;
        }

        public int GetButtonMode()
        {
            return (int)Enum.Parse(typeof(ClickModes), _mouseButtonMode);
        }
    }
}