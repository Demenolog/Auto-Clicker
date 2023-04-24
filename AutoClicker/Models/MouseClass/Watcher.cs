using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoClicker.Infrastructure.Constans.MouseClass;
using AutoClicker.Models.MouseClass.DllImport;
using static AutoClicker.Infrastructure.Constans.MouseClass.MouseClassConstans;

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
                    if (Convert.ToBoolean(User32.GetKeyState(VirtualKeyStates.VK_F4) & KEY_PRESSED))
                    {
                        MouseClicks.Cts.Cancel();
                        break;
                    }
                }
            });
        }
    }
}
