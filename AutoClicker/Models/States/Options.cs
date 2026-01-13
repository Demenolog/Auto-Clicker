using System;
using AutoClicker.Models.Clicks;
using static AutoClicker.Infrastructure.Constants.MouseClass.MouseClassConstans;

namespace AutoClicker.Models.States
{
    internal class Options
    {
        private readonly MouseButtonKind _button;
        private readonly ClickBurstKind _buttonMode;
        private readonly int _downMouseEventFlag;
        private readonly int _upMouseEventFlag;

        public Options(ClickOptionsConfig config)
        {
            _button = config.Button;
            _buttonMode = config.ButtonMode;

            (_downMouseEventFlag, _upMouseEventFlag) = Button switch
            {
                MouseButtonKind.Left => ((int)MouseEventFlags.Leftdown, (int)MouseEventFlags.Leftup),
                MouseButtonKind.Right => ((int)MouseEventFlags.Rightdown, (int)MouseEventFlags.Rightup),
                MouseButtonKind.Middle => ((int)MouseEventFlags.Middledown, (int)MouseEventFlags.Middleup),
                _ => throw new ArgumentOutOfRangeException(nameof(config.Button))
            };
        }

        public MouseButtonKind Button => _button;

        public ClickBurstKind ButtonMode => _buttonMode;

        public int DownMouseEventFlag => _downMouseEventFlag;

        public int UpMouseEventFlag => _upMouseEventFlag;

        public int GetButtonMode() => (int)ButtonMode;
    }
}
