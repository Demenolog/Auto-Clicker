using System.Runtime.InteropServices;
using static AutoClicker.Infrastructure.Constans.MouseClass.MouseConstans;

namespace AutoClicker.Models.MouseClass
{
    internal class MouseClicks
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPosition(int x, int y);

        [DllImport("user32.dll")]
        private static extern void MouseEventHandler(int dwFlags, int xAxis, int yAxis, int dwData, int dwExtraInfo);

        public static void LeftClick(int x, int y)
        {
            SetCursorPosition(x, y);
            MouseEventHandler((int)MouseEventFlags.Leftdown, x, y, 0, 0);
            MouseEventHandler((int)MouseEventFlags.Leftup, x, y, 0, 0);
        }
    }
}