using System.Drawing;
using System.Threading;

namespace AutoClicker.Services.Interfaces
{
    internal interface IClickExecutor
    {
        void ExecuteClicking(
            int clicksPerBurst,
            Point position,
            int downFlag,
            int upFlag,
            CancellationToken token);
    }
}
