using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoClicker.ViewModels;
using static AutoClicker.Infrastructure.Constans.MouseClass.MouseClassConstans;
using static AutoClicker.Models.MouseClass.DllImport.User32;

namespace AutoClicker.Models.MouseClass
{
    internal static class Watcher
    {
        private static bool IsStopped { get; set; }

        public static void WatchToStopClicking()
        {
            Task.Run(() =>
            {
                IsStopped = false;

                Thread.Sleep(500);

                while (true)
                {
                    if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_F4) & KeyPressed))
                    {
                        MouseClicks.StopClicking();
                        IsStopped = true;
                        break;
                    }
                }
            });
        }

        public static async void WatchToStartClicking()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (Convert.ToBoolean(GetKeyState(VirtualKeyStates.VK_F4) & KeyPressed) && !IsStopped)
                    {
                        new MainWindowViewModel().OnStartClickingExecute(null);
                        IsStopped = true;
                        break;
                    }
                }
            });
        }
    }
}