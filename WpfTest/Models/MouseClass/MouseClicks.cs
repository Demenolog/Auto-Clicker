using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfTest.ViewModels;
using static WpfTest.Infrastructure.Constans.MouseClass.MouseClassConstans;
using Point = System.Drawing.Point;

namespace AutoClicker.Models.MouseClass
{
    internal static class MouseClicks
    {
        private static bool IsStopped { get; set; }

        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] keystate);

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
            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            Thread.Sleep(TimeSpan.FromSeconds(5));

            return new Point(0, 0);
        }

        private static void timer_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("Test");
        }

        public static void WatchToStartClicking()
        {
            while (true)
            {
                var pressedF4 = GetKeyState(VirtualKeyStates.VK_F4);
                var pressedKey = KEY_PRESSED;
                var status = Convert.ToBoolean(pressedF4 & pressedKey);

                //if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_F4) & KEY_PRESSED) && !IsStopped)
                if (status && !IsStopped)
                {
                    new MainWindowViewModel().OnTestExecute(null);
                    IsStopped = true;
                }
            }
        }
    }
}