using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static WpfTest.Infrastructure.Constans.MouseClass.MouseClassConstans;
using Point = System.Drawing.Point;

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
    }
}