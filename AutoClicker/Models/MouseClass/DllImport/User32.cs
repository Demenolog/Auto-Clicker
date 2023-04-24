using AutoClicker.Infrastructure.Constans.MouseClass;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AutoClicker.Models.MouseClass.DllImport
{
    internal static class User32
    {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point point);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(MouseClassConstans.VirtualKeyStates nVirtKey);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int xAxis, int yAxis, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);
    }
}