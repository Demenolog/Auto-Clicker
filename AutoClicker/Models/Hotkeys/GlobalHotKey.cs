using System;
using System.Windows;
using System.Windows.Input;
using AutoClicker.Models.MouseClass.UnsafeCode;
using AutoClicker.ViewModels;

namespace AutoClicker.Models.Hotkeys
{
    internal static class GlobalHotKey
    {
        public const int HOTKEY_ID = 9000;

        //Modifiers:
        private const uint MOD_NONE = 0x0000; //[NONE]

        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS

        private static readonly ViewModelLocator s_locator = new();
        private static IntPtr s_handle;
        
        public static int GetVirtualKeyStates(string str)
        {
            Key key = (Key)Enum.Parse(typeof(Key), str, true);

            var virtualKey = KeyInterop.VirtualKeyFromKey(key);

            return virtualKey;
        }

        public static void ChangeHotKeys()
        {
            User32.UnregisterHotKey(s_handle, HOTKEY_ID);

            User32.RegisterHotKey(s_handle, HOTKEY_ID, MOD_NONE, (uint)GetVirtualKeyStates(s_locator.HotKeyWindowModel.StartHotkey));
            User32.RegisterHotKey(s_handle, HOTKEY_ID, MOD_NONE, (uint)GetVirtualKeyStates(s_locator.HotKeyWindowModel.StopHotKey));
        }

        public static void RegisterHotKey(IntPtr handle)
        {
            s_handle = handle;

            User32.RegisterHotKey(handle, HOTKEY_ID, MOD_NONE, (uint)GetVirtualKeyStates(s_locator.HotKeyWindowModel.StartHotkey));
            User32.RegisterHotKey(handle, HOTKEY_ID, MOD_NONE, (uint)GetVirtualKeyStates(s_locator.HotKeyWindowModel.StopHotKey));
        }

        public static void ResetHotKeys()
        {
            User32.UnregisterHotKey(s_handle, HOTKEY_ID);

            User32.RegisterHotKey(s_handle, HOTKEY_ID, MOD_NONE, (uint)GetVirtualKeyStates(s_locator.HotKeyWindowModel.StartHotkey));
            User32.RegisterHotKey(s_handle, HOTKEY_ID, MOD_NONE, (uint)GetVirtualKeyStates(s_locator.HotKeyWindowModel.StopHotKey));
        }

        public static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;

            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:

                            int vkey = (((int)lParam >> 16) & 0xFFFF);

                            if (vkey == GetVirtualKeyStates(s_locator.HotKeyWindowModel.StartHotkey))
                            {
                                s_locator.MainWindowModel.OnStartClickingExecute(null);
                            }
                            else if (vkey == GetVirtualKeyStates(s_locator.HotKeyWindowModel.StopHotKey))
                            {
                                s_locator.MainWindowModel.OnStopClickingExecute(null);
                            }

                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }
    }
}