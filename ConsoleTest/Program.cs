using System.Runtime.InteropServices;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MouseClicks.LeftClick(50, 50);
        }
    }

    internal class MouseClicks
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int xAxis, int yAxis, int dwData, int dwExtraInfo);

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

        public static void LeftClick(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event((int)MouseEventFlags.Leftdown, x, y, 0, 0);
            mouse_event((int)MouseEventFlags.Leftup, x, y, 0, 0);
        }
    }
}