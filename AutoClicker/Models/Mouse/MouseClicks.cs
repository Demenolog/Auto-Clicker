using AutoClicker.Models.Clicks;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using static AutoClicker.Infrastructure.Constans.MouseClass.MouseClassConstans;
using static AutoClicker.Infrastructure.UnsafeCode.User32;

namespace AutoClicker.Models.Mouse
{
    internal static class MouseClicks
    {
        #region [Properties]

        private static readonly object LockObject = new(); // Lock for thread safety
        private static bool isRunning; // Tracks if a task is running or stopping
        public static CancellationTokenSource? Cts { get; private set; }

        #endregion [Properties]

        #region [Methods]

        public static Point GetCurrentCursorPosition()
        {
            GetCursorPos(out Point result);
            return result;
        }

        public static Point GetCursorPosition()
        {
            while (true)
            {
                if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_LBUTTON) & KeyPressed))
                {
                    GetCursorPos(out Point point);
                    return point;
                }
                else if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_ESCAPE) & KeyPressed))
                {
                    return new Point(0, 0);
                }
            }
        }

        public static async Task StartClicking(Click click)
        {
            lock (LockObject)
            {
                if (isRunning) return; // Prevent multiple tasks
                isRunning = true;
                Cts = new CancellationTokenSource();
            }

            var token = Cts.Token;
            var repeats = click.Repeats.TotalTimes;

            try
            {
                await Task.Run(() =>
                {
                    if (repeats >= 0)
                    {
                        for (int i = 0; i < repeats; i++)
                        {
                            ExecuteClicking(click, token);
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            ExecuteClicking(click, token);
                        }
                    }
                }, token);
            }
            catch (OperationCanceledException)
            {
                // Expected when stopping the task
            }
            finally
            {
                lock (LockObject)
                {
                    Cts = null;
                    isRunning = false; // Allow new tasks to start
                }
            }
        }

        public static void StopClicking()
        {
            lock (LockObject)
            {
                if (!isRunning) return; // No task is running
                Cts?.Cancel();
                isRunning = false; // Mark as not running
            }
        }

        private static void ExecuteClicking(Click click, CancellationToken token)
        {
            var clicks = click.Options.GetButtonMode();
            var sleepInterval = click.Interval.TotalTime;
            var x = click.Position.CurrentPosition.X;
            var y = click.Position.CurrentPosition.Y;
            var downFlag = click.Options.DownMouseEventFlag;
            var upFlag = click.Options.UpMouseEventFlag;

            for (int i = 0; i < clicks; i++)
            {
                SetCursorPos(x, y);
                Click(downFlag);
                Click(upFlag);
            }

            Thread.Sleep(sleepInterval);

            token.ThrowIfCancellationRequested();
        }

        private static void Click(int action, int x = 0, int y = 0, int dwData = 0, int dwExtraInfo = 0)
        {
            mouse_event(action, x, y, dwData, dwExtraInfo);
        }

        #endregion [Methods]
    }
}