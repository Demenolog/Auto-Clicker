using System;
using System.IO;
using System.Windows.Media.Imaging;
using AutoClicker.Views;

namespace AutoClicker.Services
{
    internal static class HotKeysWindowService
    {
        private static HotKeyWindow? s_hotKeyWindow;

        public static bool IsCreated => s_hotKeyWindow != null;

        public static HotKeyWindow? HotKeyWindow
        {
            get => s_hotKeyWindow;
            set => s_hotKeyWindow = value;
        }

        public static void Create()
        {
            if (HotKeyWindow != null) return;

            var window = new HotKeyWindow();
            HotKeyWindow = window;
            EventHandler? closedHandler = null;
            closedHandler = (o, args) =>
            {
                if (closedHandler != null)
                {
                    window.Closed -= closedHandler;
                }
                ChildWindowsService.Remove(window);
                HotKeyWindow = null;
            };
            window.Closed += closedHandler;
            window.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/Secondary/Gear.ico"));

            ChildWindowsService.Add(window);
        }

        public static bool Show()
        {
            var window = HotKeyWindow;
            if (window == null)
            {
                return false;
            }

            window.Show();
            window.Focus();
            return true;
        }
    }
}
