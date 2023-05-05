using System;
using System.Windows;
using System.Windows.Input;
using AutoClicker.Models.MouseClass;
using AutoClicker.Models.MouseClass.UnsafeCode;
using AutoClicker.ViewModels;
using static AutoClicker.Infrastructure.Constans.HotkeysClass.GlobalHotKeyConstance;

namespace AutoClicker.Models.Hotkeys
{
    internal static class GlobalHotKey
    {
        private static readonly ViewModelLocator Locator = new();
        private static IntPtr s_handle;

        public static void ChangeHotKeys()
        {
            User32.UnregisterHotKey(s_handle, HOTKEY_ID);

            Registration();
        }

        public static uint GetVirtualKeyStates(string str)
        {
            Key key = (Key)Enum.Parse(typeof(Key), str, true);

            var virtualKey = KeyInterop.VirtualKeyFromKey(key);

            return (uint)virtualKey;
        }

        public static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:

                            int vkey = (((int)lParam >> 16) & 0xFFFF);

                            if (vkey == GetVirtualKeyStates(Locator.HotKeyWindowModel.StartHotkey) && MouseClicks.Cts == null)
                            {
                                Locator.MainWindowModel.OnStartClickingExecute(null);
                            }
                            else if (vkey == GetVirtualKeyStates(Locator.HotKeyWindowModel.StopHotKey) && MouseClicks.Cts != null)
                            {
                                Locator.MainWindowModel.OnStopClickingExecute(null);
                            }

                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        public static void RegisterHotKeys(IntPtr handle)
        {
            s_handle = handle;

            Registration();
        }

        public static void ResetHotKeys()
        {
            User32.UnregisterHotKey(s_handle, HOTKEY_ID);

            Locator.HotKeyWindowModel.StartHotkey = "F3";
            Locator.HotKeyWindowModel.StopHotKey= "F4";

            Registration();
        }

        private static void Registration()
        {
            User32.RegisterHotKey(s_handle, HOTKEY_ID, MOD_NONE, GetVirtualKeyStates(Locator.HotKeyWindowModel.StartHotkey));
            User32.RegisterHotKey(s_handle, HOTKEY_ID, MOD_NONE, GetVirtualKeyStates(Locator.HotKeyWindowModel.StopHotKey));
        }
    }
}