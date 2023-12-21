using AutoClicker.Services.WindowHelper;
using AutoClicker.ViewModels;
using System;
using static AutoClicker.Infrastructure.Constans.MouseClass.MouseClassConstans;

namespace AutoClicker.Models.Click.States
{
    internal class Options
    {
        private readonly string _button;
        private readonly string _buttonMode;
        private static readonly MainWindowViewModel MainWindow = ViewModelLocatorProvider.MainWindow;

        public Options()
        {
            _button = MainWindow.SelectedMouseButton;
            _buttonMode = MainWindow.SelectedMouseButtonMode;
        }

        public string Button => _button;

        public string ButtonMode => _buttonMode;

        public int GetButtonMode()
        {
            return (int)Enum.Parse(typeof(ClickModes), ButtonMode);
        }
    }
}