using System;
using System.Runtime.InteropServices;

namespace AutoClicker.Models.MouseClass
{
    internal class MouseClicks
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPosition(int x, int y);

        [DllImport("user32.dll")]
        private static extern void MouseEventHandler(int dwFlags, int xAxis, int yAxis, int dwData, int dwExtraInfo);

        [Flags]
        public enum MouseEventFlags
        {
            Leftdown = 0x00000002,
            Leftup = 0x00000004,
            Middledown = 0x00000020,
            Middleup = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            Rightdown = 0x00000008,
            Rightup = 0x00000010
        }

        public static void LeftClick(int x, int y)
        {
            SetCursorPosition(x, y);
            MouseEventHandler((int)MouseEventFlags.Leftdown, x, y, 0, 0);
            MouseEventHandler((int)MouseEventFlags.Leftup, x, y, 0, 0);
        }
    }
}

// https://stackoverflow.com/questions/8272681/how-can-i-simulate-a-mouse-click-at-a-certain-position-on-the-screen