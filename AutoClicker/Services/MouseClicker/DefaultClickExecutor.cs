using AutoClicker.Services.Interfaces;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using static AutoClicker.Infrastructure.Constants.MouseClass.MouseClassConstans;
using static AutoClicker.Infrastructure.UnsafeCode.User32;

namespace AutoClicker.Services.MouseClicker
{
    internal sealed class DefaultClickExecutor : IClickExecutor
    {
        public void ExecuteClicking(
            int clicksPerBurst,
            Point position,
            int downFlag,
            int upFlag,
            CancellationToken token)
        {
            // One "burst" = 1/2/3 clicks as configured
            for (var i = 0; i < clicksPerBurst; i++)
            {
                token.ThrowIfCancellationRequested();

                SetCursorPos(position.X, position.Y);
                Click(downFlag);
                Click(upFlag);
            }
        }

        private static void Click(int action, int x = 0, int y = 0, int dwData = 0, int dwExtraInfo = 0)
        {
            var input = new INPUT
            {
                type = INPUT_MOUSE,
                U = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dx = x,
                        dy = y,
                        mouseData = (uint)dwData,
                        dwFlags = (uint)action,
                        time = 0,
                        dwExtraInfo = (nuint)dwExtraInfo
                    }
                }
            };

            _ = SendInput(1, new[] { input }, Marshal.SizeOf<INPUT>());
        }
    }
}
