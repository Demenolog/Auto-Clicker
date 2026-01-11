using System;
using AutoClicker.Models.Clicks;
using static AutoClicker.Infrastructure.Constants.MouseClass.MouseClassConstans;

namespace AutoClicker.Models.States
{
    internal class Options
    {
        private readonly string _button;
        private readonly string _buttonMode;
        private readonly int _downMouseEventFlag;
        private readonly int _upMouseEventFlag;

        public Options(ClickOptionsConfig config)
        {
            _button = config.Button;
            _buttonMode = config.ButtonMode;

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
