using AutoClicker.Models.Mouse;
using AutoClicker.Models.MouseClass.UnsafeCode;
using AutoClicker.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using static AutoClicker.Infrastructure.Constans.HotkeysClass.GlobalHotKeyConstance;

namespace AutoClicker.Models.Hotkeys
{
    internal static class GlobalHotKey
    {
        private static readonly ViewModelLocator Locator = new();
        private static IntPtr s_handle;
        internal const string DefaultStartHotKey = "F3";
        internal const string DefaultStopHotKey = "F3";

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

            Locator.HotKeyWindowModel.StartHotkey = DefaultStartHotKey;
            Locator.HotKeyWindowModel.StopHotKey = DefaultStopHotKey;

            Registration();
        }

        private static void Registration()
        {
            try
            {
                User32.RegisterHotKey(s_handle, HOTKEY_ID, MOD_NONE, GetVirtualKeyStates(Locator.HotKeyWindowModel.StartHotkey));
                User32.RegisterHotKey(s_handle, HOTKEY_ID, MOD_NONE, GetVirtualKeyStates(Locator.HotKeyWindowModel.StopHotKey));
            }
            catch (ArgumentException ex)
            {

                MessageBox.Show(ex.Message + "\n\nTo specify some keys, you need to write them using a specific name. Examples for some keys:\n" +
                                "0-9 --> D0-D9\n" +
                                "Num0-Num9 --> NumPad0-NumPad9\n" +
                                "CTRL --> ControlKey\n" +
                                "For more information google 'Keys Enum'", 
                    ex.ParamName, 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);

                ResetHotKeys();
            }
        }
    }
}