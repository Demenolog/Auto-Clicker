using System;
using System.Threading.Tasks;
using static AutoClicker.Infrastructure.Constans.MouseClass.MouseClassConstans;
using static AutoClicker.Models.MouseClass.DllImport.User32;

namespace AutoClicker.Models.MouseClass
{
    internal static class Watcher
    {
        public static async void WatchToStopClicking()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_F4) & KEY_PRESSED))
                    {
                        MouseClicks.Cts.Cancel();
                        break;
                    }
                }
            });
        }
    }
}