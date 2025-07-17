using System;
using AutoClicker.Services.WindowHelper;
using AutoClicker.ViewModels;
using static AutoClicker.Infrastructure.Constans.MouseClass.MouseClassConstans;

namespace AutoClicker.Models.Clicks.States
{
    internal class Options
    {
        private readonly string _button;
        private readonly string _buttonMode;
        private readonly int _downMouseEventFlag;
        private readonly int _upMouseEventFlag;
        private static readonly MainWindowViewModel MainWindow = ViewModelLocatorProvider.MainWindow;

        public Options()
        {
            _button = MainWindow.SelectedMouseButton;
            _buttonMode = MainWindow.SelectedMouseButtonMode;

            (_downMouseEventFlag, _upMouseEventFlag) = Button switch
            {
                "Left" => ((int)MouseEventFlags.Leftdown, (int)MouseEventFlags.Leftup),
                "Right" => ((int)MouseEventFlags.Rightdown, (int)MouseEventFlags.Rightup),
                _ => throw new ArgumentException()
            };
        }

        public string Button => _button;

        public string ButtonMode => _buttonMode;

        public int DownMouseEventFlag => _downMouseEventFlag;

        public int UpMouseEventFlag => _upMouseEventFlag;

        public int GetButtonMode()
        {
            return (int)Enum.Parse(typeof(ClickModes), ButtonMode);
        }
    }
}