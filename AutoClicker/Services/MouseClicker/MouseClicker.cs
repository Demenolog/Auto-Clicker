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
        private readonly IClickerTiming _timing;
        private readonly IClickExecutor _clickExecutor;
        private bool _isRunning;
        private CancellationTokenSource? _cts;

        public MouseClicker(IClickerTiming timing, IClickExecutor clickExecutor)
        {
            _timing = timing ?? throw new ArgumentNullException(nameof(timing));
            _clickExecutor = clickExecutor ?? throw new ArgumentNullException(nameof(clickExecutor));
        }

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
                _cts = new CancellationTokenSource();
            }

            var startDelay = click.StartDelay < TimeSpan.Zero ? TimeSpan.Zero : click.StartDelay;
            var stopAfter = click.StopAfter;

            var token = _cts.Token;
            if (stopAfter > TimeSpan.Zero)
            {
                stopAfterCts = _timing.CreateStopAfterCancellationTokenSource(stopAfter);
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
            var intervalDelay = TimeSpan.FromMilliseconds(intervalMs);
            var position = click.Position.CurrentPosition;
            var downFlag = click.Options.DownMouseEventFlag;
            var upFlag = click.Options.UpMouseEventFlag;

            try
            {
                await _timing.Delay(startDelay, linkedToken).ConfigureAwait(false);
                await Task.Run(async () =>
                {
                    if (repeats >= 0)
                    {
                        // Finite number of bursts
                        for (var i = 0; i < repeats; i++)
                        {
                            linkedToken.ThrowIfCancellationRequested();
                            ExecuteClicking(clicksPerBurst, position, downFlag, upFlag, linkedToken);

                            // No delay after last burst
                            if (i < repeats - 1 && intervalMs > 0)
                            {
                                await _timing.Delay(intervalDelay, linkedToken).ConfigureAwait(false);
                            }
                        }
                    }
                    else
                    {
                        // Endless bursts until cancelled
                        while (true)
                        {
                            linkedToken.ThrowIfCancellationRequested();
                            ExecuteClicking(clicksPerBurst, position, downFlag, upFlag, linkedToken);

                            if (intervalMs > 0)
                            {
                                await _timing.Delay(intervalDelay, linkedToken).ConfigureAwait(false);
                            }
                            else
                            {
                                // Zero interval -> yield to avoid tight CPU spin
                                await _timing.Yield();
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
                }
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

        private void ExecuteClicking(
            int clicksPerBurst,
            Point position,
            int downFlag,
            int upFlag,
            CancellationToken token)
        {
            _clickExecutor.ExecuteClicking(clicksPerBurst, position, downFlag, upFlag, token);
        }
    }
}
