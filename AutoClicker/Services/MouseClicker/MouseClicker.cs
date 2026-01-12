using AutoClicker.Models.Clicks;
using AutoClicker.Services.Interfaces;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static AutoClicker.Infrastructure.Constants.MouseClass.MouseClassConstans;
using static AutoClicker.Infrastructure.UnsafeCode.User32;

namespace AutoClicker.Services.MouseClicker
{
    internal sealed class MouseClicker : IMouseClicker
    {
        private readonly object _lockObject = new(); // protects isRunning + Cts
        private readonly ManualResetEventSlim _pauseGate = new(true);
        private bool _isRunning;
        private bool _isPaused;
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

        public bool IsPaused
        {
            get
            {
                lock (_lockObject)
                {
                    return _isPaused;
                }
            }
        }

        public event Action? ClickingStopped;

        public Point GetCurrentCursorPosition()
        {
            GetCursorPos(out Point result);
            return result;
        }

        public bool TryGetCursorPosition(out Point position)
        {
            // Busy-wait with a small sleep to avoid high CPU when picking a point
            while (true)
            {
                // Left mouse button -> capture and return
                if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_LBUTTON) & KeyPressed))
                {
                    GetCursorPos(out position);
                    return true;
                }

                // Esc -> cancel picking
                if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_ESCAPE) & KeyPressed))
                {
                    position = default;
                    return false;
                }

                Thread.Sleep(10);
            }
        }

        public async Task StartClicking(Click click)
        {
            if (click == null) throw new ArgumentNullException(nameof(click));

            CancellationTokenSource? linkedCts = null;
            CancellationTokenSource? stopAfterCts = null;

            // Prevent multiple concurrent click loops
            lock (_lockObject)
            {
                if (_isRunning)
                {
                    return;
                }

                _isRunning = true;
                _isPaused = false;
                _pauseGate.Set();
                _cts = new CancellationTokenSource();
            }

            var startDelay = click.StartDelay < TimeSpan.Zero ? TimeSpan.Zero : click.StartDelay;
            var stopAfter = click.StopAfter;

            var token = _cts.Token;
            if (stopAfter > TimeSpan.Zero)
            {
                stopAfterCts = new CancellationTokenSource();
                stopAfterCts.CancelAfter(stopAfter);
                linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, stopAfterCts.Token);
            }
            else
            {
                linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token);
            }

            var linkedToken = linkedCts.Token;

            // Snapshot click configuration at start
            var repeats = click.Repeats.TotalTimes;             // -1 => endless
            var clicksPerBurst = click.Options.GetButtonMode(); // single/double/triple
            var intervalMs = Math.Max(0, click.Interval.TotalTime);
            var position = click.Position.CurrentPosition;
            var downFlag = click.Options.DownMouseEventFlag;
            var upFlag = click.Options.UpMouseEventFlag;

            try
            {
                await Task.Delay(startDelay, linkedToken).ConfigureAwait(false);
                await Task.Run(async () =>
                {
                    if (repeats >= 0)
                    {
                        // Finite number of bursts
                        for (var i = 0; i < repeats; i++)
                        {
                            linkedToken.ThrowIfCancellationRequested();
                            _pauseGate.Wait(linkedToken);

                            ExecuteClicking(clicksPerBurst, position, downFlag, upFlag, linkedToken);

                            // No delay after last burst
                            if (i < repeats - 1 && intervalMs > 0)
                            {
                                await Task.Delay(intervalMs, linkedToken).ConfigureAwait(false);
                            }
                        }
                    }
                    else
                    {
                        // Endless bursts until cancelled
                        while (true)
                        {
                            linkedToken.ThrowIfCancellationRequested();
                            _pauseGate.Wait(linkedToken);

                            ExecuteClicking(clicksPerBurst, position, downFlag, upFlag, linkedToken);

                            if (intervalMs > 0)
                            {
                                await Task.Delay(intervalMs, linkedToken).ConfigureAwait(false);
                            }
                            else
                            {
                                // Zero interval -> yield to avoid tight CPU spin
                                await Task.Yield();
                            }
                        }
                    }
                }, linkedToken).ConfigureAwait(false);
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
                    _isPaused = false;
                }

                _pauseGate.Set();
                linkedCts?.Dispose();
                stopAfterCts?.Dispose();
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

                _isPaused = false;
                _pauseGate.Set();
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

        public void PauseClicking()
        {
            lock (_lockObject)
            {
                if (!_isRunning || _isPaused)
                {
                    return;
                }

                _isPaused = true;
                _pauseGate.Reset();
            }
        }

        public void ResumeClicking()
        {
            lock (_lockObject)
            {
                if (!_isPaused)
                {
                    return;
                }

                _isPaused = false;
                _pauseGate.Set();
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
