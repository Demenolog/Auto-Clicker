using AutoClicker.Models.Clicks;
using AutoClicker.Services.Interfaces;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using static AutoClicker.Infrastructure.Constants.MouseClass.MouseClassConstans;
using static AutoClicker.Infrastructure.UnsafeCode.User32;

namespace AutoClicker.Services.MouseClicker
{
    internal sealed class MouseClicker : IMouseClicker
    {
        private readonly object _lockObject = new(); // protects isRunning + Cts
        private bool _isRunning;
        private CancellationTokenSource? _cts;

        public bool IsRunning
        {
            get
            {
                lock (_lockObject)
                {
                    return _isRunning;
                }
            }
        }

        public event Action? ClickingStopped;

        public Point GetCurrentCursorPosition()
        {
            GetCursorPos(out Point result);
            return result;
        }

        public Point GetCursorPosition()
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

        public async Task StartClicking(Click click)
        {
            if (click == null) throw new ArgumentNullException(nameof(click));

            // Prevent multiple concurrent click loops
            lock (_lockObject)
            {
                if (_isRunning)
                {
                    return;
                }

                _isRunning = true;
                _cts = new CancellationTokenSource();
            }

            var token = _cts.Token;

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
                lock (_lockObject)
                {
                    _cts?.Dispose();
                    _cts = null;
                    _isRunning = false;
                }

                ClickingStopped?.Invoke();
            }
        }

        public void StopClicking()
        {
            lock (_lockObject)
            {
                if (!_isRunning)
                {
                    return;
                }

                try
                {
                    _cts?.Cancel();
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
    }
}
