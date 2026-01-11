using AutoClicker.Infrastructure.UnsafeCode;
using AutoClicker.Models.Mouse;
using AutoClicker.ViewModels;
using System;
using System.Windows.Input;
using static AutoClicker.Infrastructure.Constants.HotkeysClass.GlobalHotKeyConstance;

namespace AutoClicker.Models.Hotkeys
{
    internal static class GlobalHotKey
    {
        private static readonly ViewModelLocator Locator = new();
        private static IntPtr s_handle;
        internal static readonly HotKeyDefinition DefaultStartHotKey = new(ModifierKeys.None, Key.F3);
        internal static readonly HotKeyDefinition DefaultStopHotKey = new(ModifierKeys.None, Key.F3);

        public static void ChangeHotKeys()
        {
            User32.UnregisterHotKey(s_handle, START_HOTKEY_ID);
            User32.UnregisterHotKey(s_handle, STOP_HOTKEY_ID);

            Registration();
        }

        public static uint GetVirtualKeyState(Key key)
        {
            var virtualKey = KeyInterop.VirtualKeyFromKey(key);

            return (uint)virtualKey;
        }

        public static uint GetModifierFlags(ModifierKeys modifiers)
        {
            uint flags = MOD_NONE;

            if (modifiers.HasFlag(ModifierKeys.Alt))
            {
                flags |= MOD_ALT;
            }

            if (modifiers.HasFlag(ModifierKeys.Control))
            {
                flags |= MOD_CONTROL;
            }

            if (modifiers.HasFlag(ModifierKeys.Shift))
            {
                flags |= MOD_SHIFT;
            }

            if (modifiers.HasFlag(ModifierKeys.Windows))
            {
                flags |= MOD_WIN;
            }

            return flags;
        }

        public static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case START_HOTKEY_ID:
                            uint startModifiers = (uint)((int)lParam & 0xFFFF);
                            uint vkey = (uint)(((int)lParam >> 16) & 0xFFFF);
                            var startBinding = Locator.HotKeyWindowModel.StartHotKey;

                            if (vkey == GetVirtualKeyState(startBinding.Key)
                                && startModifiers == GetModifierFlags(startBinding.Modifiers)
                                && MouseClicks.Cts == null)
                            {
                                Locator.MainWindowModel.OnStartClickingExecute(null);
                            }
                            handled = true;
                            break;
                        case STOP_HOTKEY_ID:
                            uint stopModifiers = (uint)((int)lParam & 0xFFFF);
                            uint stopVKey = (uint)(((int)lParam >> 16) & 0xFFFF);
                            var stopBinding = Locator.HotKeyWindowModel.StopHotKey;

                            if (stopVKey == GetVirtualKeyState(stopBinding.Key)
                                && stopModifiers == GetModifierFlags(stopBinding.Modifiers)
                                && MouseClicks.Cts != null)
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
            User32.UnregisterHotKey(s_handle, START_HOTKEY_ID);
            User32.UnregisterHotKey(s_handle, STOP_HOTKEY_ID);

            Locator.HotKeyWindowModel.SetStartHotKey(DefaultStartHotKey);
            Locator.HotKeyWindowModel.SetStopHotKey(DefaultStopHotKey);

            Registration();
        }

        private static void Registration()
        {
            var startBinding = Locator.HotKeyWindowModel.StartHotKey;
            var stopBinding = Locator.HotKeyWindowModel.StopHotKey;

            User32.RegisterHotKey(s_handle, START_HOTKEY_ID, GetModifierFlags(startBinding.Modifiers), GetVirtualKeyState(startBinding.Key));
            User32.RegisterHotKey(s_handle, STOP_HOTKEY_ID, GetModifierFlags(stopBinding.Modifiers), GetVirtualKeyState(stopBinding.Key));
        }
    }
}
