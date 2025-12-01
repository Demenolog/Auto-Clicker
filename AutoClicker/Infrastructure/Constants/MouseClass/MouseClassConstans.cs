using System;

namespace AutoClicker.Infrastructure.Constans.MouseClass
{
    public static class MouseClassConstans
    {
        public const int KeyPressed = 0x8000;

        public enum ClickModes
        {
            Single = 1,
            Double = 2,
            Triple = 3
        }

        [Flags]
        public enum MouseEventFlags
        {
            Leftdown = 0x02,
            Leftup = 0x04,
            Middledown = 0x020,
            Middleup = 0x40,
            Move = 0x01,
            Absolute = 0x8000,
            Rightdown = 0x08,
            Rightup = 0x10
        }

        [Flags]
        public enum VirtualKeyStates
        {
            VK_LBUTTON = 0x01,
            VK_RBUTTON = 0x02,
            VK_CANCEL = 0x03,
            VK_MBUTTON = 0x04,

            VK_ESCAPE = 0x1B
        }
    }
}