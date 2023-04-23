using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using static AutoClicker.Infrastructure.Constans.MouseClass.MouseClassConstans;

namespace AutoClicker.Models.MouseClass
{
    internal static class MouseClicks
    {
        public static int GetClickMode(string clickMode)
        {
            return (int)Enum.Parse(typeof(ClickModes), clickMode);
        }

        public static Point GetCurrentCursorPosition()
        {
            GetCursorPos(out Point result);
            return result;
        }

        public static Point GetCursorPosition()
        {
            while (true)
            {
                if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_LBUTTON) & KEY_PRESSED))
                {
                    GetCursorPos(out Point point);
                    return point;
                }
            }
        }

        public static void StartClicking(int intervalTime, string selectedBtn, int selectedBtnMode, int repeatMode, Point cursorPosition)
        {
            if (selectedBtn == "Left")
            {
                for (int i = 0; i < repeatMode; i++)
                {
                    RunLeftClicking(cursorPosition, selectedBtnMode);
                    Thread.Sleep(intervalTime);
                }
            }
            else if (selectedBtn == "Right")
            {
                for (int i = 0; i < repeatMode; i++)
                {
                    RunRightClicking(cursorPosition, selectedBtnMode);
                    Thread.Sleep(intervalTime);
                }
            }
        }

        private static void Click(MouseEventFlags action, int x = 0, int y = 0, int dwData = 0, int dwExtraInfo = 0)
        {
            mouse_event((int)action, x, y, dwData, dwExtraInfo);
        }

        private static void RunLeftClicking(Point cursorPosition, int clicksMode)
        {
            int x = cursorPosition.X;
            int y = cursorPosition.Y;

            for (int i = 0; i < clicksMode; i++)
            {
                SetCursorPos(x, y);
                Click(MouseEventFlags.Leftdown);
                Click(MouseEventFlags.Leftup);
            }
        }

        private static void RunRightClicking(Point cursorPosition, int clicksMode)
        {
            int x = cursorPosition.X;
            int y = cursorPosition.Y;

            for (int i = 0; i < clicksMode; i++)
            {
                SetCursorPos(x, y);
                Click(MouseEventFlags.Rightdown);
                Click(MouseEventFlags.Rightup);
            }
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point point);

        [DllImport("user32.dll")]
        private static extern short GetKeyState(VirtualKeyStates nVirtKey);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int xAxis, int yAxis, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);
    }
}