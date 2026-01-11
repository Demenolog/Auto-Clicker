using AutoClicker;
using AutoClicker.Infrastructure.UnsafeCode;
using AutoClicker.Services.Interfaces;
using AutoClicker.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Input;
using static AutoClicker.Infrastructure.Constants.HotkeysClass.GlobalHotKeyConstance;

namespace AutoClicker.Models.Hotkeys
{
    internal static class GlobalHotKey
    {
        private static readonly ViewModelLocator Locator = new();
        private static IMouseClicker MouseClicker => App.Services.GetRequiredService<IMouseClicker>();
        private static IntPtr s_handle;
        internal static readonly HotKeyDefinition DefaultStartHotKey = new(ModifierKeys.None, Key.F3);
        internal static readonly HotKeyDefinition DefaultStopHotKey = new(ModifierKeys.None, Key.F4);

        public static void ChangeHotKeys()
        {
            User32.UnregisterHotKey(s_handle, START_HOTKEY_ID);
            User32.UnregisterHotKey(s_handle, STOP_HOTKEY_ID);

            Registration();
        }

        private static (uint Modifiers, uint VirtualKey) GetHotKeyRegistration(HotKeyDefinition hotKey)
        {
            var virtualKey = (uint)KeyInterop.VirtualKeyFromKey(hotKey.Key);
            uint flags = MOD_NONE;

            if (hotKey.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                flags |= MOD_ALT;
            }

            if (hotKey.Modifiers.HasFlag(ModifierKeys.Control))
            {
                flags |= MOD_CONTROL;
            }

            if (hotKey.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                flags |= MOD_SHIFT;
            }

            if (hotKey.Modifiers.HasFlag(ModifierKeys.Windows))
            {
                flags |= MOD_WIN;
            }

            return (flags, virtualKey);
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
                            var startRegistration = GetHotKeyRegistration(startBinding);

                            if (vkey == startRegistration.VirtualKey
                                && startModifiers == startRegistration.Modifiers
                                && !MouseClicker.IsRunning)
                            {
                                Locator.MainWindowModel.OnStartClickingExecute(null);
                            }
                            handled = true;
                            break;
                        case STOP_HOTKEY_ID:
                            uint stopModifiers = (uint)((int)lParam & 0xFFFF);
                            uint stopVKey = (uint)(((int)lParam >> 16) & 0xFFFF);
                            var stopBinding = Locator.HotKeyWindowModel.StopHotKey;
                            var stopRegistration = GetHotKeyRegistration(stopBinding);

                            if (stopVKey == stopRegistration.VirtualKey
                                && stopModifiers == stopRegistration.Modifiers
                                && MouseClicker.IsRunning)
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
            var startRegistration = GetHotKeyRegistration(startBinding);
            var stopRegistration = GetHotKeyRegistration(stopBinding);

            User32.RegisterHotKey(s_handle, START_HOTKEY_ID, startRegistration.Modifiers, startRegistration.VirtualKey);
            User32.RegisterHotKey(s_handle, STOP_HOTKEY_ID, stopRegistration.Modifiers, stopRegistration.VirtualKey);
        }
    }
}
