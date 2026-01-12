using AutoClicker.Models.Clicks;
using AutoClicker.Services.Interfaces;
using AutoClicker.Services.MouseClicker;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoClicker.Tests.MouseClicker
{
    public class MouseClickerTests
    {
        [Fact]
        public async Task CancelDuringStartDelayPreventsClickLoop()
        {
            var timing = new ControlledClickerTiming();
            var executor = new RecordingClickExecutor();
            var clicker = new MouseClicker(timing, executor);
            var click = CreateClick(TimeSpan.FromSeconds(5), TimeSpan.Zero, repeatUntilStopped: false, repeatTimes: "1", intervalMilliseconds: "0");
            var stoppedTcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            clicker.ClickingStopped += () => stoppedTcs.TrySetResult();

            var startTask = clicker.StartClicking(click);

            clicker.StopClicking();

            await Task.WhenAll(startTask, stoppedTcs.Task);

            Assert.False(clicker.IsRunning);
            Assert.Equal(0, executor.CallCount);
        }

        [Fact]
        public async Task AutoStopTriggersClickingStoppedAndResetsIsRunning()
        {
            var timing = new ControlledClickerTiming();
            var executor = new RecordingClickExecutor();
            var clicker = new MouseClicker(timing, executor);
            var click = CreateClick(TimeSpan.Zero, TimeSpan.FromSeconds(5), repeatUntilStopped: true, repeatTimes: string.Empty, intervalMilliseconds: "100");
            var stoppedTcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            clicker.ClickingStopped += () => stoppedTcs.TrySetResult();

            var startTask = clicker.StartClicking(click);

            timing.CompleteNextDelay();

            Assert.True(clicker.IsRunning);

            timing.StopAfterSource?.Cancel();

            await Task.WhenAll(startTask, stoppedTcs.Task);

            Assert.False(clicker.IsRunning);
        }

        private static Click CreateClick(TimeSpan startDelay, TimeSpan stopAfter, bool repeatUntilStopped, string repeatTimes, string intervalMilliseconds)
        {
            var interval = new ClickIntervalConfig("0", "0", "0", intervalMilliseconds);
            var options = new ClickOptionsConfig(MouseButtonKind.Left, ClickBurstKind.Single);
            var repeats = new ClickRepeatsConfig(repeatUntilStopped, repeatTimes);
            var position = new ClickPositionConfig(true, "0", "0");
            var config = new ClickConfig(interval, options, repeats, position, startDelay, stopAfter);

            return new Click(config);
        }

        private sealed class ControlledClickerTiming : IClickerTiming
        {
            private readonly Queue<TaskCompletionSource> _delays = new();

            public CancellationTokenSource? StopAfterSource { get; private set; }

            public Task Delay(TimeSpan delay, CancellationToken token)
            {
                if (token.IsCancellationRequested)
                {
                    return Task.FromCanceled(token);
                }

                var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
                token.Register(() => tcs.TrySetCanceled(token));
                _delays.Enqueue(tcs);
                return tcs.Task;
            }

            public Task Yield() => Task.CompletedTask;

            public CancellationTokenSource CreateStopAfterCancellationTokenSource(TimeSpan stopAfter)
            {
                StopAfterSource = new CancellationTokenSource();
                return StopAfterSource;
            }

            public void CompleteNextDelay()
            {
                if (_delays.Count == 0)
                {
                    throw new InvalidOperationException("No pending delays to complete.");
                }

                _delays.Dequeue().TrySetResult();
            }
        }

        private sealed class RecordingClickExecutor : IClickExecutor
        {
            private int _callCount;

            public int CallCount => _callCount;

            public void ExecuteClicking(int clicksPerBurst, System.Drawing.Point position, int downFlag, int upFlag, CancellationToken token)
            {
                Interlocked.Increment(ref _callCount);
            }
        }
    }
}
