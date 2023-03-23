using System.Drawing;
using System.Runtime.InteropServices;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Start");

            var task = new Task<Point>(MouseClicks.PickLocation);

            task.Start();

            Console.WriteLine("Wait to pick");
            
            var result = task.WaitAsync(CancellationToken.None).Result;

            Console.WriteLine("Area picked");

            Console.WriteLine($"{result.X} and {result.Y}");
        }
    }

    internal class MouseClicks
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point point);

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
            Point pos = new();
            GetCursorPos(out pos);

            Console.WriteLine($"{pos.X} and {pos.Y}");

            SetCursorPos(x, y);
            mouse_event((int)MouseEventFlags.Leftdown, x, y, 0, 0);
            mouse_event((int)MouseEventFlags.Leftup, x, y, 0, 0);
        }

        public static Point PickLocation()
        {
            Thread.Sleep(5000);

            Point pos = new();

            GetCursorPos(out pos);

            return pos;
        }
    }
}