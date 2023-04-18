using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static WpfTest.Infrastructure.Constans.MouseClass.MouseClassConstans;

namespace AutoClicker.Models.MouseClass
{
    internal static class MouseClicks
    {
        [DllImport("user32.dll")]
        private static extern short GetKeyState(VirtualKeyStates nVirtKey);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point point);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int xAxis, int yAxis, int dwData, int dwExtraInfo);

        public static Point GetCursorPosition()
        {
            Point point;

            while (true)
            {
                if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_LBUTTON) & KEY_PRESSED))
                {
                    GetCursorPos(out point);
                    return point;
                }
            }
        }
    }
}