using System;
using System.IO;
using System.Windows.Media.Imaging;
using AutoClicker.Views;

namespace AutoClicker.Services
{
    internal static class HotKeysWindowService
    {
        private static HotKeyWindow s_hotKeyWindow = null;

        public static bool IsCreated => s_hotKeyWindow != null;

        public static HotKeyWindow HotKeyWindow
        {
            get => s_hotKeyWindow;
            set => s_hotKeyWindow = value;
        }

        public static void Create()
        {
            if (HotKeyWindow != null) return;

            HotKeyWindow = new HotKeyWindow();
            HotKeyWindow.Closed += (o, args) => HotKeyWindow = null;
            HotKeyWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/Icons/Secondary/Gear.ico"));

            ChildWindowsService.Add(HotKeyWindow);
        }

        public static bool Show()
        {
            if (HotKeyWindow != null)
            {
                HotKeyWindow.Show();
                HotKeyWindow.Focus();
                return true;
            }

            return false;
        }
    }
}