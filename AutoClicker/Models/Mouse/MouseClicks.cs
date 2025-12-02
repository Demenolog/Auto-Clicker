using AutoClicker.Models.Clicks;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using static AutoClicker.Infrastructure.Constants.MouseClass.MouseClassConstans;
using static AutoClicker.Infrastructure.UnsafeCode.User32;

namespace AutoClicker.Models.Mouse
{
    internal static class MouseClicks
    {
        #region [Properties]

        private static readonly object LockObject = new(); // protects isRunning + Cts
        private static bool isRunning;

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
            // Busy-wait with a small sleep to avoid high CPU when picking a point
            while (true)
            {
                // Left mouse button -> capture and return
                if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_LBUTTON) & KeyPressed))
                {
                    GetCursorPos(out Point point);
                    return point;
                }

                // Esc -> cancel picking, return (0,0) as before
                if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_ESCAPE) & KeyPressed))
                {
                    return new Point(0, 0);
                }

                Thread.Sleep(10);
            }
        }

        public static async Task StartClicking(Click click)
        {
            if (click == null) throw new ArgumentNullException(nameof(click));

            // Prevent multiple concurrent click loops
            lock (LockObject)
            {
                if (isRunning)
                    return;

                isRunning = true;
                Cts = new CancellationTokenSource();
            }

            var token = Cts.Token;

            // Snapshot click configuration at start
            var repeats = click.Repeats.TotalTimes;             // -1 => endless
            var clicksPerBurst = click.Options.GetButtonMode(); // single/double/triple
            var intervalMs = Math.Max(0, click.Interval.TotalTime);
            var position = click.Position.CurrentPosition;
            var downFlag = click.Options.DownMouseEventFlag;
            var upFlag = click.Options.UpMouseEventFlag;

            try
            {
                await Task.Run(async () =>
                {
                    if (repeats >= 0)
                    {
                        // Finite number of bursts
                        for (var i = 0; i < repeats; i++)
                        {
                            token.ThrowIfCancellationRequested();

                            ExecuteClicking(clicksPerBurst, position, downFlag, upFlag, token);

                            // No delay after last burst
                            if (i < repeats - 1 && intervalMs > 0)
                            {
                                await Task.Delay(intervalMs, token).ConfigureAwait(false);
                            }
                        }
                    }
                    else
                    {
                        // Endless bursts until cancelled
                        while (true)
                        {
                            token.ThrowIfCancellationRequested();

                            ExecuteClicking(clicksPerBurst, position, downFlag, upFlag, token);

                            if (intervalMs > 0)
                            {
                                await Task.Delay(intervalMs, token).ConfigureAwait(false);
                            }
                            else
                            {
                                // Zero interval -> yield to avoid tight CPU spin
                                await Task.Yield();
                            }
                        }
                    }
                }, token).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Expected when StopClicking() cancels Cts.
            }
            finally
            {
                lock (LockObject)
                {
                    Cts?.Dispose();
                    Cts = null;
                    isRunning = false;
                }
            }
        }

        public static void StopClicking()
        {
            lock (LockObject)
            {
                if (!isRunning)
                    return;

                try
                {
                    Cts?.Cancel();
                }
                catch
                {
                    // Ignore races if Cts is already disposed/finished
                }
            }
        }

        private static void ExecuteClicking(
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
            mouse_event(action, x, y, dwData, dwExtraInfo);
        }

        #endregion [Methods]
    }
}
