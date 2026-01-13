using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoClicker.Services.Interfaces
{
    internal interface IClickerTiming
    {
        Task Delay(TimeSpan delay, CancellationToken token);

        Task Yield();

        CancellationTokenSource CreateStopAfterCancellationTokenSource(TimeSpan stopAfter);
    }
}
