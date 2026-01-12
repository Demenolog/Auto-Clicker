using AutoClicker.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoClicker.Services.MouseClicker
{
    internal sealed class ClickerTiming : IClickerTiming
    {
        public Task Delay(TimeSpan delay, CancellationToken token) => Task.Delay(delay, token);

        public async Task Yield() => await Task.Yield();

        public CancellationTokenSource CreateStopAfterCancellationTokenSource(TimeSpan stopAfter)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(stopAfter);
            return cts;
        }
    }
}
